// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;

namespace BamlTools
{
    public class BamlResource
    {
        string _bamlName;
        Stream _stream;
        Assembly _assembly;

        public BamlResource(string bamlName, Stream stream, Assembly assembly)
        {
            _bamlName = bamlName;
            _stream = stream;
            _assembly = assembly;
        }

        public static int SortCompare(BamlResource a, BamlResource b)
        {
            return String.Compare(a.BamlName, b.BamlName);
        }

        public string BamlName
        {
            get { return _bamlName; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }

        public bool IsBaml
        {
            get { return _bamlName.EndsWith(".baml", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}
