// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BamlTools;
using System.Collections.Generic;

namespace BamlReflector
{
    class NewDisassemblyCache
    {
        public string Disassembly { get; set; }

        public List<TypeInfoTableEntry> TypeInfoTable { get; set; }
        public List<AttributeInfoTableEntry> AttributeInfoTable { get; set; }
        //public List<AssemblyInfoTableEntry> AssemblyInfoTable { get; set; }
    }
}
