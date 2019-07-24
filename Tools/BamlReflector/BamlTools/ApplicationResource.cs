// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Resources;

namespace BamlTools
{
    public class ApplicationResource
    {
        private readonly string _resourceName;
        private readonly Assembly _assembly;
        private readonly ObservableCollection<BamlResource> _bamlResources = new ObservableCollection<BamlResource>();

        public ApplicationResource(Assembly assembly, string resourceName)
        {
            _assembly = assembly;
            _resourceName = resourceName;
            Stream resourceStream = assembly.GetManifestResourceStream(resourceName);

            using ResourceReader reader = new ResourceReader(resourceStream);
            List<BamlResource> bamlResourceList = new List<BamlResource>();

            if (reader != null)
            {
                foreach (DictionaryEntry entry in reader)
                {
                    string keyName = entry.Key as string;
                    if (keyName.EndsWith(".baml", StringComparison.InvariantCultureIgnoreCase)
                        || keyName.EndsWith(".xaml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Type valuesType = entry.Value.GetType();
                        if (typeof(Stream).IsAssignableFrom(valuesType))
                        {
                            Stream source = entry.Value as Stream;
                            bamlResourceList.Add(new BamlResource(keyName, source, assembly));
                        }
                    }
                }
            }
            bamlResourceList.Sort(BamlResource.SortCompare);
            foreach (BamlResource br in bamlResourceList)
            {
                BamlResources.Add(br);
            }
        }

        public string ResourceName { get { return _resourceName; } }

        public ObservableCollection<BamlResource> BamlResources
        {
            get { return _bamlResources; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }
    }
}
