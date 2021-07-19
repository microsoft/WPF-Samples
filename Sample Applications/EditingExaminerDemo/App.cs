// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;

namespace EditingExaminerDemo
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            if(framework != ".NETCoreApp,Version=v6.0")
            {
                StartupUri = new Uri("MainWindowOld.xaml", UriKind.Relative);
            }
        }
    }
}