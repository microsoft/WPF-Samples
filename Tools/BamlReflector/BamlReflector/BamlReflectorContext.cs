// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BamlTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xaml;

namespace BamlReflector
{
    public class BamlReflectorContext : INotifyPropertyChanged
    {
        Assembly _assembly = null;
        private readonly ObservableCollection<ReferenceAssembly_VM> _referenceAssemblies;
        private readonly ObservableCollection<ReferenceAssembly_VM> _loadedReferenceAssemblies;
        bool _hideTableDefinitions;
        BamlResource _currentBamlResource;
        bool _showAddresses;
        bool _showRecordNumbers;
        bool _showDebugRecords;
        bool _showTableRecords;

        NewDisassemblyCache _newDisassemblyCache;

        public BamlReflectorContext()
        {
            _referenceAssemblies = new ObservableCollection<ReferenceAssembly_VM>();
            _loadedReferenceAssemblies = new ObservableCollection<ReferenceAssembly_VM>();
            _referenceAssemblies.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ReferenceAssemblies_CollectionChanged);
            _showAddresses = true;
            _showRecordNumbers = true;
            _showDebugRecords = true;
            _showTableRecords = true;
        }

        //  INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public Assembly LocalAssembly
        {
            get { return _assembly; }
            set
            {
                if (_assembly != value)
                {
                    _assembly = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LocalAssembly"));
                }
            }
        }

        public ObservableCollection<ReferenceAssembly_VM> ReferenceAssemblies
        {
            get { return _referenceAssemblies; }
        }

        // Loaded Reference assemblies should be the same as reference asseblies  (todo remove dup functionality)
        public ObservableCollection<ReferenceAssembly_VM> LoadedReferenceAssemblies
        {
            get { return _loadedReferenceAssemblies; }
        }

        public bool HideTableDefinitions
        {
            get { return _hideTableDefinitions; }
            set
            {
                if (value != _hideTableDefinitions)
                {
                    _hideTableDefinitions = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("HideTableDefinitions"));
                }
            }
        }

        public BamlResource CurrentBamlResource
        {
            get { return _currentBamlResource; }
            set
            {
                _currentBamlResource = value;
                _newDisassemblyCache = null;
                PropertyChanged(this, new PropertyChangedEventArgs("HexDump"));
                PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_Old"));
                PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_New"));
                PropertyChanged(this, new PropertyChangedEventArgs("XamlNodeStream"));
                PropertyChanged(this, new PropertyChangedEventArgs("XamlText"));
                PropertyChanged(this, new PropertyChangedEventArgs("TypeInfoTable"));
                PropertyChanged(this, new PropertyChangedEventArgs("AttributeInfoTable"));
            }
        }

        public bool ShowAddresses
        {
            get { return _showAddresses; }
            set
            {
                if (_showAddresses != value)
                {
                    _showAddresses = value;
                    _newDisassemblyCache = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("HexDump"));
                    PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_Old"));
                    PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_New"));
                }
            }
        }

        public bool ShowRecordNumbers
        {
            get { return _showRecordNumbers; }
            set
            {
                if (_showRecordNumbers != value)
                {
                    _showRecordNumbers = value;
                    _newDisassemblyCache = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_Old"));
                    PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_New"));
                }
            }
        }

        public bool ShowDebugRecords
        {
            get { return _showDebugRecords; }
            set
            {
                if (_showDebugRecords != value)
                {
                    _showDebugRecords = value;
                    _newDisassemblyCache = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_New"));
                }
            }
        }

        public bool ShowTableRecords
        {
            get { return _showTableRecords; }
            set
            {
                if (_showTableRecords != value)
                {
                    _showTableRecords = value;
                    _newDisassemblyCache = null;
                    PropertyChanged(this, new PropertyChangedEventArgs("BamlDisassembly_New"));
                }
            }
        }

        public string HexDump
        {
            get
            {
                if (CurrentBamlResource != null)
                {
                    Stream stream = CurrentBamlResource.Stream;
                    stream.Seek(0, SeekOrigin.Begin);
                    HexDumper dumper = new HexDumper(stream)
                    {
                        ShowAddresses = ShowAddresses
                    };
                    //dumper.ShowRecordNumbers = ShowRecordNumbers;  not-implemented on HexDumper
                    //dumper.ShowDebugRecords = ShowDebugRecords;    not-implemented on HexDumper
                    //dumper.ShowTableRecords = ShowTableRecords;    not-implemented on HexDumper
                    return dumper.DumpOneBigString();
                }
                return String.Empty;
            }
        }

        public string BamlDisassembly_Old
        {
            get
            {
                if (CurrentBamlResource != null && CurrentBamlResource.IsBaml)
                {
                    Stream stream = CurrentBamlResource.Stream;
                    stream.Seek(0, SeekOrigin.Begin);
                    OldBamlDisassembler dasm = new OldBamlDisassembler(stream)
                    {
                        ShowAddresses = ShowAddresses,
                        ShowRecordNumbers = ShowRecordNumbers,
                        ShowDebugRecords = ShowDebugRecords,  // Not implemented
                        ShowTableRecords = ShowTableRecords  // Not implemented
                    };
                    return dasm.DasmOneBigString();
                }
                return String.Empty;
            }
        }

        public string BamlDisassembly_New
        {
            get
            {
                if (CurrentBamlResource != null && CurrentBamlResource.IsBaml)
                {
                    EnsureBamlResourceLoad();
                    return _newDisassemblyCache.Disassembly;
                }

                return String.Empty;
            }
        }

        public string XamlNodeStream
        {
            get
            {
                if (CurrentBamlResource != null)
                {
                    Stream stream = CurrentBamlResource.Stream;
                    stream.Seek(0, SeekOrigin.Begin);

                    if (CurrentBamlResource.IsBaml)
                    {
                        BamlNodeStreamReader bamlNodeStreamReader = new BamlNodeStreamReader(stream, LocalAssembly);
                        return bamlNodeStreamReader.OneBigString();
                    }
                    else
                    {
                        using XamlXmlReader reader = new XamlXmlReader(stream);
                        StringBuilder sb = new StringBuilder();
                        DiagnosticWriter writer = new DiagnosticWriter(sb, reader.SchemaContext);
                        try
                        {
                            XamlServices.Transform(reader, writer);
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(ex.ToString());
                        }
                        return sb.ToString();
                    }
                }
                return String.Empty;
            }
        }

        public string XamlText
        {
            get
            {
                if (CurrentBamlResource != null)
                {
                    Stream stream = CurrentBamlResource.Stream;
                    stream.Seek(0, SeekOrigin.Begin);

                    if (CurrentBamlResource.IsBaml)
                    {
                        BamlToXamlReader bamlToXamlReader = new BamlToXamlReader(stream, LocalAssembly);
                        string xamlOrError;
                        try
                        {
                            xamlOrError = bamlToXamlReader.OneBigString();
                        }
                        catch (Exception ex)
                        {
                            xamlOrError = ex.ToString();
                        }
                        return xamlOrError;
                    }
                    else
                    {
                        StreamReader sr = new StreamReader(stream);
                        return sr.ReadToEnd();
                    }
                }
                return String.Empty;
            }
        }


        private void EnsureBamlResourceLoad()
        {
            if (_newDisassemblyCache == null)
            {
                _newDisassemblyCache = new NewDisassemblyCache();

                if (CurrentBamlResource != null && CurrentBamlResource.IsBaml)
                {
                    Stream stream = CurrentBamlResource.Stream;
                    stream.Seek(0, SeekOrigin.Begin);
                    NewBamlDisassembler dasm = new NewBamlDisassembler(stream)
                    {
                        ShowAddresses = ShowAddresses,
                        ShowRecordNumbers = ShowRecordNumbers,
                        ShowDebugRecords = ShowDebugRecords,
                        ShowTableRecords = ShowTableRecords
                    };
                    _newDisassemblyCache.Disassembly = dasm.DasmOneBigString();
                    _newDisassemblyCache.TypeInfoTable = dasm.TypeInfoTable;
                    _newDisassemblyCache.AttributeInfoTable = dasm.AttributeInfoTable;
                }
            }
        }

        public List<TypeInfoTableEntry> TypeInfoTable
        {
            get
            {
                if (CurrentBamlResource != null && CurrentBamlResource.IsBaml)
                {
                    EnsureBamlResourceLoad();
                    return _newDisassemblyCache.TypeInfoTable;
                }
                return null;
            }
        }

        public List<AttributeInfoTableEntry> AttributeInfoTable
        {
            get
            {
                if (CurrentBamlResource != null && CurrentBamlResource.IsBaml)
                {
                    EnsureBamlResourceLoad();
                    return _newDisassemblyCache.AttributeInfoTable;
                }
                return null;
            }
        }

        // ===========================================

        private void ReferenceAssemblies_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ReferenceAssembly_VM refAsm in ReferenceAssemblies)
            {
                if (refAsm.IsLoaded)
                {
                    if (!_loadedReferenceAssemblies.Contains(refAsm))
                    {
                        _loadedReferenceAssemblies.Add(refAsm);
                    }
                }
            }
        }


    }
}
