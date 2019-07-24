// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace BamlTools
{
    public class TypeInfoTableEntry
    {
        int _typeId;
        String _fullName;

        public TypeInfoTableEntry(int typeId, string fullName)
        {
            _typeId = typeId;
            _fullName = fullName;
        }

        public int TypeId
        {
            get { return _typeId; }
        }

        public String FullName
        {
            get { return _fullName; }
        }

    }
}
