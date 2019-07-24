// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BamlTools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace BamlReflector
{
    public class ReferenceAssembly_VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly String _pathName;
        private Assembly _loadedAssembly;
        private String _errorMessage;
        private readonly int _hashCode;
        private readonly ObservableCollection<ApplicationResource> _applicationResources;

        public ReferenceAssembly_VM(string name)
        {
            _pathName = name;
            _errorMessage = "Have not attemped to load";
            _hashCode = name.GetHashCode();
            _applicationResources = new ObservableCollection<ApplicationResource>();
        }

        public ReferenceAssembly_VM(Assembly assembly)
        {
            _loadedAssembly = assembly;
            _pathName = String.Empty;
            _errorMessage = "Loaded by caller";
            _hashCode = assembly.GetHashCode();
            _applicationResources = LoadApplicationResources(assembly);
        }

        public bool IsLoaded
        {
            get { return _loadedAssembly != null; }
        }

        public String Name
        {
            get
            {
                if (IsLoaded)
                {
                    AssemblyName asmName = new AssemblyName(_loadedAssembly.FullName);
                    return asmName.Name;
                }
                return "Err: " + _pathName;
            }
        }

        public String AssemblyName
        {
            get
            {
                if (IsLoaded)
                    return _loadedAssembly.FullName;
                return _pathName;
            }
        }

        public Assembly LoadedAssembly
        {
            get { return _loadedAssembly; }
            set
            {
                _loadedAssembly = value;
                NotifyProperyChanged("AssemblyName");
                NotifyProperyChanged("IsLoaded");
            }
        }

        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                NotifyProperyChanged("ErrorMessage");
                _errorMessage = value;
            }
        }

        public ObservableCollection<ApplicationResource> ApplicationResources
        {
            get { return _applicationResources; }
        }

        public String PathName
        {
            get { return _pathName; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReferenceAssembly_VM that))
                return false;

            if (this.LoadedAssembly != null && that.LoadedAssembly != null)
            {
                if (this.LoadedAssembly.FullName.Equals(that.LoadedAssembly.FullName, StringComparison.OrdinalIgnoreCase))
                    return true;
                return false;
            }
            if (this.LoadedAssembly == null && that.LoadedAssembly == null)
            {
                if (this.PathName != null && that.PathName != null)
                {
                    if (this.PathName.Equals(that.PathName, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                }
                return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        //  ========= private =========================

        private void NotifyProperyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }

        private static ObservableCollection<ApplicationResource> LoadApplicationResources(Assembly assembly)
        {
            ObservableCollection<ApplicationResource> applicationResources = new ObservableCollection<ApplicationResource>();
            if (assembly != null)
            {
                string[] allResourceNames = assembly.GetManifestResourceNames();
                foreach (string resourceName in allResourceNames)
                {
                    if (resourceName.EndsWith(".resources", StringComparison.InvariantCultureIgnoreCase))
                    {
                        applicationResources.Add(new ApplicationResource(assembly, resourceName));
                    }
                }
            }
            return applicationResources;
        }
    }
}
