using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Documents.Serialization;

namespace DocumentSerialization
{
    class HtmlSerializerFactory :  ISerializerFactory
    {
        /// <summary>
        /// Create a SerializerWriter on the passed in stream
        /// </summary>
        public SerializerWriter CreateSerializerWriter(Stream stream)
        {
            return new HtmlSerializerWriter(stream);
        }
        /// <summary>
        /// Return the DisplayName of the serializer.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return "Html Document Writer";
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
                return ".html";
            }
        }
    }
}
