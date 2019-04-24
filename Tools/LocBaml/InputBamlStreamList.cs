// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//---------------------------------------------------------------------------
//
// Description: InputBamlStreamList class
//              It enumerates all the baml streams in the input file for parsing
//
//---------------------------------------------------------------------------
using System;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Resources;

namespace BamlLocalization
{
    /// <summary>
    /// Class that enumerates all the baml streams in the input file
    /// </summary>
    internal class InputBamlStreamList
    {
        /// <summary>
        /// constructor
        /// </summary>        
        internal InputBamlStreamList(LocBamlOptions options)
        {
            _bamlStreams  = new ArrayList();
            switch(options.InputType)
            {
                case FileType.BAML:
                {
                    _bamlStreams.Add(
                        new BamlStream(
                            Path.GetFileName(options.Input),
                            File.OpenRead(options.Input)
                            )
                    );
                    break;
                }
                case FileType.RESOURCES:
                {
                    using (ResourceReader resourceReader = new ResourceReader(options.Input))
                    {
                        // enumerate all bamls in a resources
                        EnumerateBamlInResources(resourceReader, options.Input);
                    }
                    break;
                }
		case FileType.EXE:
                case FileType.DLL:
                {
                    // for a dll, it is the same idea
                    Assembly assembly = Assembly.LoadFrom(options.Input);
                    foreach (string resourceName in assembly.GetManifestResourceNames())
                    {
                        ResourceLocation resourceLocation = assembly.GetManifestResourceInfo(resourceName).ResourceLocation;
                               
                        // if this resource is in another assemlby, we will skip it
                        if ((resourceLocation & ResourceLocation.ContainedInAnotherAssembly) != 0)
                        {
                            continue;   // in resource assembly, we don't have resource that is contained in another assembly
                        }
 
                        Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
                        using (ResourceReader reader = new ResourceReader(resourceStream))
                        {
                            EnumerateBamlInResources(reader, resourceName);                              
                        }
                    }                    
                    break;
                }
                default:
                {
                    
                    Debug.Assert(false, "Not supported type");
                    break;
                }                    
            }                  
        }

        /// <summary>
        /// return the number of baml streams found
        /// </summary>
        internal int Count
        {
            get{return _bamlStreams.Count;}
        }

        /// <summary>
        /// Gets the baml stream in the input file through indexer
        /// </summary>        
        internal BamlStream this[int i]
        {
            get { return (BamlStream) _bamlStreams[i];}
        }

        /// <summary>
        /// Close the baml streams enumerated
        /// </summary>
        internal void Close()
        {
            for (int i = 0; i < _bamlStreams.Count; i++)
            {
               ((BamlStream) _bamlStreams[i]).Close();
            }            
        }

        //--------------------------------
        // private function
        //--------------------------------
        /// <summary>
        /// Enumerate baml streams in a resources file
        /// </summary>        
        private void EnumerateBamlInResources(ResourceReader reader, string resourceName)
        {                       
            foreach (DictionaryEntry entry in reader)
            {
                string name = entry.Key as string;
                if (BamlStream.IsResourceEntryBamlStream(name, entry.Value))
                {    
                    _bamlStreams.Add( 
                        new BamlStream(
                            BamlStream.CombineBamlStreamName(resourceName, name),
                            (Stream) entry.Value
                        )
                    );
                }    
            }
        }
        
        ArrayList    _bamlStreams;
    }

    /// <summary>
    /// BamlStream class which represents a baml stream
    /// </summary>
    internal class BamlStream
    {
        private string _name;
        private Stream _stream;

        /// <summary>
        /// constructor
        /// </summary>
        internal BamlStream(string name, Stream stream)
        {
            _name = name;
            _stream = stream;
        }
        
        /// <summary>
        /// name of the baml 
        /// </summary>
        internal string Name 
        { 
            get { return _name;}
        }

        /// <summary>
        /// The actual Baml stream
        /// </summary>
        internal Stream Stream
        {
            get { return _stream;}
        }

        /// <summary>
        /// close the stream
        /// </summary>
        internal void Close()
        {
            if (_stream != null)
            {
                _stream.Close();
            }           
        }

        /// <summary>
        /// Helper method which determines whether a stream name and value pair indicates a baml stream
        /// </summary>
        internal static bool IsResourceEntryBamlStream(string name, object value)
        {             
            string extension = Path.GetExtension(name);
            if (string.Compare(
                    extension, 
                    "." + FileType.BAML.ToString(), 
                    true, 
                    CultureInfo.InvariantCulture
                    ) == 0
                )                            
            {
                   //it has .Baml at the end
                Type type = value.GetType();

                if (typeof(Stream).IsAssignableFrom(type))
                return true;
            }            
            return false;                
        }

        /// <summary>
        /// Combine baml stream name and resource name to uniquely identify a baml within a 
        /// localization project
        /// </summary>
        internal static string CombineBamlStreamName(string resource, string bamlName)
        {
            Debug.Assert(resource != null && bamlName != null, "Resource name and baml name can't be null");

            string suffix = Path.GetFileName(bamlName);
            string prefix = Path.GetFileName(resource);

            return prefix + LocBamlConst.BamlAndResourceSeperator + suffix;
        }
    }
}
