// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;


// runtimeconfig.template.json replaces App.Config based AppContext configuration
// App.config is still used for WPF's old style configuration switches (BaseCompatibilityPreferences class, for e.g.)

namespace Wpf_AppCompat_Quirks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public static Type GetAppContextSwitchType(string className)
        {
            Type tSwitch = null;
            switch (className)
            {
                case "BaseAppContextSwitches":
                    tSwitch = Assembly.Load("WindowsBase").GetType($"MS.Internal.BaseAppContextSwitches");
                    break;
                case "AccessibilitySwitches":
                    tSwitch = Assembly.Load("WindowsBase").GetType($"System.Windows.AccessibilitySwitches");
                    break;
                case "CoreAppContextSwitches":
                    tSwitch = Assembly.Load("PresentationCore").GetType("MS.Internal.CoreAppContextSwitches");
                    break;
                case "FrameworkAppContextSwitches":
                    tSwitch = Assembly.Load("PresentationFramework").GetType("MS.Internal.FrameworkAppContextSwitches");
                    break;
                case "BuildTasksAppContextSwitches":
                    tSwitch = Assembly.Load("PresentationBuildTasks").GetType("MS.Internal.BuildTasksAppContextSwitches");
                    break;
            }

            return tSwitch;
        }

        private Type GetCompatPreferenceType(string compatPreferenceClass)
        {
            Type tCompatPref = null;
            switch (compatPreferenceClass)
            {
                case "BaseCompatibilityPreferences":
                    tCompatPref = Assembly.Load("WindowsBase").GetType($"System.Windows.BaseCompatibilityPreferences");
                    break;
                case "CoreCompatibilityPreferences":
                    tCompatPref = Assembly.Load("PresentationCore").GetType($"System.Windows.CoreCompatibilityPreferences");
                    break;
                case "FrameworkCompatibilityPreferences":
                    tCompatPref = Assembly.Load("PresentationFramework").GetType($"System.Windows.FrameworkCompatibilityPreferences");
                    break;

            }
            return tCompatPref;
        }

        public static bool TryGetAppContextSwitchValue(string className, string switchName, out bool switchValue)
        {
            switchValue = false;

            Type tSwitch = GetAppContextSwitchType(className);

            if (tSwitch == null)
            {
                return false;
            }

            var pProperty = tSwitch.GetProperty(switchName, BindingFlags.Static | BindingFlags.Public);
            if (pProperty != null)
            {
                try
                {
                    switchValue = (bool)pProperty.GetValue(null);
                    return true;
                }
                catch { }
            }

            return false;
        }

        private bool TryGetCompatibilityPreferenceSwitchValue(string compatPreferenceClass, string switchName, out bool switchValue)
        {
            switchValue = false;
            Type tSwitch = GetCompatPreferenceType(compatPreferenceClass);
            if (tSwitch == null)
            {
                return false;
            }

            var pProperty = tSwitch.GetProperty(switchName, BindingFlags.Static | BindingFlags.Public);
            if (pProperty == null)
            {
                pProperty = tSwitch.GetProperty(switchName, BindingFlags.Static | BindingFlags.NonPublic);
            }

            if (pProperty != null)
            {
                try
                {
                    switchValue = (bool)pProperty.GetValue(null);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        private bool TryGetCompatibilityPreferenceSwitchValue(string compatPreferenceClass, string switchName, out string switchString)
        {
            switchString = null;
            Type tSwitch = GetCompatPreferenceType(compatPreferenceClass);
            if (tSwitch == null)
            {
                return false;
            }

            var pProperty = tSwitch.GetProperty(switchName, BindingFlags.Static | BindingFlags.Public);
            if (pProperty == null)
            {
                pProperty = tSwitch.GetProperty(switchName, BindingFlags.Static | BindingFlags.NonPublic);
            }

            if (pProperty != null)
            {
                try
                {
                    object o = pProperty.GetValue(null);
                    if (pProperty.PropertyType == typeof(bool?))
                    {
                        bool? b = (bool?)o;
                        if (!b.HasValue)
                        {
                            switchString = "(bool?)null";
                        }
                        else
                        {
                            switchString = $"(bool?){b.Value.ToString()}";
                        }
                        return true;
                    }

                    switchString = o.ToString();
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateAppContextInformation();
        }

        private void PopulateAppContextInformation()
        {
            string[] appContextClasses =
            {
                "BaseAppContextSwitches",
                "CoreAppContextSwitches",
                "FrameworkAppContextSwitches",
                "AccessibilitySwitches",
                //"BuildTasksAppContextSwitches"
            };

            foreach (var appContextClass in appContextClasses)
            {
                var tSwitch = GetAppContextSwitchType(appContextClass);
                var properties = tSwitch.GetProperties();
                foreach (var property in properties)
                {
                    if (TryGetAppContextSwitchValue(appContextClass, property.Name, out bool switchValue))
                    {
                        AppContextSwitches.Add(
                            new CompatSwitchInformation()
                            {
                                Class = appContextClass,
                                SwitchName = property.Name,
                                DefaultValue = switchValue.ToString(),
                                SettingsSource = "runtimeconfig.template.json"
                            });
                    }
                    else
                    {
                        Debug.WriteLine(property.Name);
                    }
                }
            }

            string[] compatibilityPreferenceClasses =
            {
                "CoreCompatibilityPreferences",
                "BaseCompatibilityPreferences",
                "FrameworkCompatibilityPreferences"
            };

            foreach (var compatPreferenceClass in compatibilityPreferenceClasses)
            {
                var tPref = GetCompatPreferenceType(compatPreferenceClass);
                var properties = tPref.GetProperties().Union(tPref.GetProperties(BindingFlags.Static | BindingFlags.NonPublic)).ToList() ;

                foreach (var property in properties)
                {
                    if (TryGetCompatibilityPreferenceSwitchValue(compatPreferenceClass, property.Name, out bool switchValue))
                    {
                        CompatibilityPreferences.Add(new CompatSwitchInformation()
                        {
                            Class = compatPreferenceClass,
                            SwitchName = property.Name, 
                            DefaultValue = switchValue.ToString(), 
                            SettingsSource = "App.config"
                        });
                    }
                    else if (TryGetCompatibilityPreferenceSwitchValue(compatPreferenceClass, property.Name, out string switchString))
                    {
                        CompatibilityPreferences.Add(new CompatSwitchInformation()
                        {
                            Class = compatPreferenceClass,
                            SwitchName = property.Name,
                            DefaultValue = switchString,
                            SettingsSource = "App.config"
                        });
                    }
                    else
                    {
                        Debug.WriteLine(property.Name);
                    }
                }
            }
        }



        private ObservableCollection<CompatSwitchInformation> _appContextSwitches;
        public ObservableCollection<CompatSwitchInformation> AppContextSwitches
        {
            get
            {
                if (_appContextSwitches == null)
                {
                    AppContextSwitches = new ObservableCollection<CompatSwitchInformation>();
                }
                return _appContextSwitches;
            }

            set
            {
                _appContextSwitches = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppContextSwitches)));
            }
        }

        private ObservableCollection<CompatSwitchInformation> _compatibilityPreferences;
        public ObservableCollection<CompatSwitchInformation> CompatibilityPreferences
        {
            get
            {
                if (_compatibilityPreferences == null)
                {
                    CompatibilityPreferences = new ObservableCollection<CompatSwitchInformation>();
                }
                return _compatibilityPreferences;
            }

            set
            {
                _compatibilityPreferences = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompatibilityPreferences)));
            }
        }
    }
}
