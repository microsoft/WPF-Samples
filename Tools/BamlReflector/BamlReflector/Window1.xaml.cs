// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BamlTools;
using Microsoft.Win32;
using System;
using System.Reflection;
using System.Windows;

namespace BamlReflector
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private readonly BamlReflectorContext _context;

        public Window1()
        {
            _context = new BamlReflectorContext();
            this.DataContext = _context;
            InitializeComponent();
        }

        private void OnOpen_Executed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "WPF Assemblies(*.exe *.dll)|*.exe;*.dll",
                Multiselect = true
            };
            bool isFileChoosen = (bool)dlg.ShowDialog(this);
            Assembly assembly = null;
            String[] assemblyNames;

            if (isFileChoosen)
            {
                assemblyNames = dlg.FileNames;
                foreach (String name in assemblyNames)
                {
                    try
                    {
                        assembly = Assembly.LoadFrom(name);
                    }
                    catch (Exception ex)
                    {
                        Window window = new Window
                        {
                            Content = ex
                        };
                        window.ShowDialog();
                    }
                    if (assembly != null)
                    {
                        _context.ReferenceAssemblies.Add(new ReferenceAssembly_VM(assembly));
                    }
                }
            }
        }

        private void OnExit_Executed(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is BamlResource baml)
            {
                // Set the Assembly before the Baml resource
                // so that the BAML read has a current assembly.
                _context.LocalAssembly = baml.Assembly;
                _context.CurrentBamlResource = baml;
            }
            else if (e.NewValue is ApplicationResource appResource)
            {
                _context.LocalAssembly = appResource.Assembly;
                _context.CurrentBamlResource = null;
            }
            else if (e.NewValue is ReferenceAssembly_VM refAsm)
            {
                _context.LocalAssembly = refAsm.LoadedAssembly;
                _context.CurrentBamlResource = null;
            }
        }
    }
}
