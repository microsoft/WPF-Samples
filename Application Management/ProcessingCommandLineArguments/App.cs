// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProcessingCommandLineArguments
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Hashtable CommandLineArgs = new Hashtable();

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // Don't bother if no command line args were passed
            // NOTE: e.Args is never null - if no command line args were passed, 
            //       the length of e.Args is 0.
            if (e.Args.Length == 0) return;

            // Parse command line args for args in the following format:
            //   /argname:argvalue /argname:argvalue /argname:argvalue ...
            //
            // Note: This sample uses regular expressions to parse the command line arguments.
            // For regular expressions, see:
            // http://msdn.microsoft.com/library/en-us/cpgenref/html/cpconRegularExpressionsLanguageElements.asp
            var pattern = @"(?<argname>/\w+):(?<argvalue>\w+)";
            foreach (var arg in e.Args)
            {
                var match = Regex.Match(arg, pattern);

                // If match not found, command line args are improperly formed.
                if (!match.Success)
                    throw new ArgumentException(
                        "The command line arguments are improperly formed. Use /argname:argvalue.");

                // Store command line arg and value
                CommandLineArgs[match.Groups["argname"].Value] = match.Groups["argvalue"].Value;
            }
        }
    }
}