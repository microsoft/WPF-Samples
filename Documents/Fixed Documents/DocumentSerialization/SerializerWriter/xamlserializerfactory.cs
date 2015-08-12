using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Documents.Serialization;

namespace DocumentSerialization
{
    class XamlSerializerFactory :  ISerializerFactory
    {
        /// <summary>
        /// Create a SerializerWriter on the passed in stream
        /// </summary>
        public SerializerWriter CreateSerializerWriter(Stream stream)
        {
            return new XamlSerializerWriter(stream);
        }
        /// <summary>
        /// Return the DisplayName of the serializer.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return "Xaml Document Writer";
            }
        }
        /// <summary>
        /// Return the ManufacturerName of the serializer.
        /// </summary>
        public string ManufacturerName
        {
            get
            {
                return "Microsoft";
            }
        }
        /// <summary>
        /// Return the ManufacturerWebsite of the serializer.
        /// </summary>
        public Uri ManufacturerWebsite
        {
            get
            {
                return new Uri("http://www.microsoft.com");
            }
        }
        /// <summary>
        /// Return the DefaultFileExtension of the serializer.
        /// </summary>
        public string DefaultFileExtension
        {
            get
            {
                return ".xaml";
            }
        }
    }
}
