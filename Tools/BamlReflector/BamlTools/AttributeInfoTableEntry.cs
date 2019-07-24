// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace BamlTools
{
    public class AttributeInfoTableEntry
    {
        private readonly int _attributeId;
        private readonly string _name;
        private readonly int _ownerTypeId;

        public AttributeInfoTableEntry(int attributeId, string name, int ownerTypeId)
        {
            _attributeId = attributeId;
            _name = name;
            _ownerTypeId = ownerTypeId;
        }

        public int AttributeId
        {
            get { return _attributeId; }
        }

        public String Name
        {
            get { return _name; }
        }

        public int OwnerTypeId
        {
            get { return _ownerTypeId; }
        }

    }
}
