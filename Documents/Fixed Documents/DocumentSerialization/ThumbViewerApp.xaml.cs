// DocumentSerialize SDK Sample - ThumbViewerApp.xaml.cs
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents.Serialization;

namespace DocumentSerialization
{
    public partial class ThumbViewerApp : Application
    {

        /// <summary>Handler called when the application starts up.</sumary>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            try
            {
                SerializerProvider.RegisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new XamlSerializerFactory()), false);
                SerializerProvider.RegisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new TxtSerializerFactory()), false);
                SerializerProvider.RegisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new RtfSerializerFactory()), false);
                SerializerProvider.RegisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new HtmlSerializerFactory()), false);
            }
            catch (ArgumentException)
            {
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                SerializerProvider.UnregisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new XamlSerializerFactory()));
                SerializerProvider.UnregisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new TxtSerializerFactory()));
                SerializerProvider.UnregisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new RtfSerializerFactory()));
                SerializerProvider.UnregisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new HtmlSerializerFactory()));
            }
            catch (ArgumentException)
            {
            }
            base.OnExit(e);
        }
        //
        //  Private Properties
        //
        //------------------------------------------------------

        #region Private Properties

        private string ApplicationName => "Thumbnail Viewer";

        #endregion Private Properties

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------
        #region Private fields.

        #endregion Private fields.
    }
}
