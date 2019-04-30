// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//---------------------------------------------------------------------------
//
// Description: LocBaml command line tool. 
//
//---------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Threading;

namespace BamlLocalization
{
    /// <summary>
    /// LocBaml tool: A command line tool to localize baml
    /// </summary>
    public static class LocBaml
    {
        private const int ErrorCode = 100;        
        private const int SuccessCode = 0;
        private static Dispatcher _dispatcher;

        //----------------------------------
        // Main
        //----------------------------------
        [System.STAThread()]
        public static int Main(string[] args)
        {
            LocBamlOptions options;
            string errorMessage;
            GetCommandLineOptions(args, out options, out errorMessage);

            if (errorMessage != null)
            {
                // there are errors                
                PrintLogo(options);
                Console.WriteLine(StringLoader.Get("ErrorMessage", errorMessage));                
                Console.WriteLine();
                PrintUsage();
                return ErrorCode;    // error
            }          

             // at this point, we obtain good options.
            if (options == null)            
            {
                // no option to process. Noop.
                return SuccessCode;
            }
            
            _dispatcher = Dispatcher.CurrentDispatcher;

            PrintLogo(options);

            try{
                // it is to parse
                if (options.ToParse)
                {
                    ParseBamlResources(options);
                }
                else
                {
                    GenerateBamlResources(options);
                }
            }
            catch(Exception e)                
            {
#if DEBUG
                throw e;
#else
                Console.WriteLine(e.Message);
                return ErrorCode;            
#endif
            }

            return SuccessCode;
        
        }        

         #region Private static methods
        //---------------------------------------------
        // Private static methods
        //---------------------------------------------

        /// <summary>
        /// Parse the baml resources given in the command line
        /// </summary>        
        private static void ParseBamlResources(LocBamlOptions options)
        {            
            TranslationDictionariesWriter.Write(options);         
        }

        /// <summary>
        /// Genereate localized baml 
        /// </summary>        
        private static void GenerateBamlResources(LocBamlOptions options)
        {   
            Stream input = File.OpenRead(options.Translations);
            using (ResourceTextReader reader = new ResourceTextReader(options.TranslationFileType, input))
            {   
                TranslationDictionariesReader dictionaries = new TranslationDictionariesReader(reader);                                                               
                ResourceGenerator.Generate(options, dictionaries);
            }         
        }
            
        /// <summary>
        /// get CommandLineOptions, return error message
        /// </summary>
        private static void GetCommandLineOptions(string[] args, out LocBamlOptions options, out string errorMessage)
        {
            CommandLine commandLine; 
            try{
                // "*" means the option must have a value. no "*" means the option can't have a value 
                 commandLine = new CommandLine(args, 
                                    new string[]{
                                            "parse",        // /parse for update
                                            "generate",     // /generate     for generate
                                            "*out",         // /out          for output .csv|.txt when parsing, for output directory when generating
                                            "*culture",     // /culture      for culture name
                                            "*translation", // /translation  for translation file, .csv|.txt
                                            "*asmpath",     // /asmpath,     for assembly path to look for references   (
                                            "nologo",       // /nologo       for not to print logo      
                                            "help",         // /help         for help
                                            "verbose"       // /verbose      for verbose output         
                                        }                
                                     );
           }            
           catch (ArgumentException e)
           {
               errorMessage = e.Message;
               options      = null;
               return;
           }

            if (commandLine.NumArgs + commandLine.NumOpts < 1)
            {
                PrintLogo(null);
                PrintUsage();
                errorMessage    = null;
                options         = null;
                return;
            }

            options = new LocBamlOptions();

            options.Input    = commandLine.GetNextArg();

            Option commandLineOption;

            while ( (commandLineOption = commandLine.GetNextOption()) != null)
            {
                if (commandLineOption.Name      == "parse")
                {
                    options.ToParse = true;
                }
                else if (commandLineOption.Name == "generate")
                {
                    options.ToGenerate = true;
                }
                else if (commandLineOption.Name == "nologo")
                {
                    options.HasNoLogo = true;                        
                }
                else if (commandLineOption.Name == "help")
                {
                    // we print usage and stop processing
                    PrintUsage();
                    errorMessage = null;
                    options = null;
                    return;
                }
                else if (commandLineOption.Name == "verbose")
                {
                    options.IsVerbose = true;
                }
                    // the following ones need value
                else if (commandLineOption.Name == "out")
                {
                    options.Output = commandLineOption.Value;
                }
                else if (commandLineOption.Name == "translation")
                {
                    options.Translations = commandLineOption.Value;
                }
                else if (commandLineOption.Name == "asmpath")
                {
                    if (options.AssemblyPaths == null)
                    {
                        options.AssemblyPaths = new ArrayList();
                    }

                    options.AssemblyPaths.Add(commandLineOption.Value);
                }
                else if (commandLineOption.Name == "culture")
                {
                    try
                    {
                        options.CultureInfo = new CultureInfo(commandLineOption.Value);
                    }
                    catch (ArgumentException e)
                    {
                        // Error
                        errorMessage = e.Message;
                        return;
                    }
                }
                else
                {
                    // something that we don't recognize
                    errorMessage = StringLoader.Get("Err_InvalidOption", commandLineOption.Name);
                    return;
                }
            }

            // we passed all the test till here. Now check the combinations of the options
            errorMessage = options.CheckAndSetDefault();       
        }

        private static void PrintLogo(LocBamlOptions option)
        {
            if (option == null || !option.HasNoLogo)
            {               
                Console.WriteLine(StringLoader.Get("Msg_Copyright", GetAssemblyVersion()));
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine(StringLoader.Get("Msg_Usage"));
        }         


        private static string GetAssemblyVersion()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();                                   
            return currentAssembly.GetName().Version.ToString(4);
        }
        
         #endregion
    }




    #region LocBamlOptions
    // the class that groups all the baml options together
    internal sealed class LocBamlOptions
    {    
        internal string         Input;
        internal string         Output;        
        internal CultureInfo    CultureInfo;
        internal string         Translations;        
        internal bool           ToParse;
        internal bool           ToGenerate;
        internal bool           HasNoLogo;
        internal bool           IsVerbose;
        internal FileType       TranslationFileType;
        internal FileType       InputType;
        internal ArrayList      AssemblyPaths;
        internal Assembly[]     Assemblies;

        /// <summary>
        /// return true if the operation succeeded.
        /// otherwise, return false
        /// </summary>
        internal  string  CheckAndSetDefault()
        {
            // we validate the options here and also set default
            // if we can

            // Rule #1: One and only one action at a time
            // i.e. Can't parse and generate at the same time
            //      Must do one of them
            if ((ToParse && ToGenerate) ||
                (!ToParse && !ToGenerate))
                return StringLoader.Get("MustChooseOneAction");

            // Rule #2: Must have an input 
            if (string.IsNullOrEmpty(Input))
            {
                return StringLoader.Get("InputFileRequired");
            }
            else
            {                
                if (!File.Exists(Input))
                {
                    return StringLoader.Get("FileNotFound", Input);
                }

                string extension = Path.GetExtension(Input);
             
                // Get the input file type.
                if (string.Compare(extension, "." + FileType.BAML.ToString(), true, CultureInfo.InvariantCulture) == 0)
                {
                    InputType = FileType.BAML;
                }
                else if (string.Compare(extension, "." + FileType.RESOURCES.ToString(), true, CultureInfo.InvariantCulture) == 0)
                {
                    InputType = FileType.RESOURCES;                    
                }
                else if (string.Compare(extension, "." + FileType.DLL.ToString(), true, CultureInfo.InvariantCulture) == 0)
                {
                    InputType = FileType.DLL;
                }
                else if (string.Compare(extension, "." + FileType.EXE.ToString(), true, CultureInfo.InvariantCulture) == 0)
                {
                    InputType = FileType.EXE;
                }
                else
                {
                    return StringLoader.Get("FileTypeNotSupported", extension);
                }                                
            }
            
            if (ToGenerate)
            {
                // Rule #3: before generation, we must have Culture string
                if (CultureInfo == null &&  InputType != FileType.BAML)
                {
                    // if we are not generating baml, 
                    return StringLoader.Get("CultureNameNeeded", InputType.ToString());
                }
                
                // Rule #4: before generation, we must have translation file
                if (string.IsNullOrEmpty(Translations))
                {

                    return StringLoader.Get("TranslationNeeded");
                }
                else
                {
                    string extension = Path.GetExtension(Translations);

                    if (!File.Exists(Translations))
                    {
                        return StringLoader.Get("TranslationNotFound", Translations);
                    }
                    else
                    {
                        if (string.Compare(extension, "." + FileType.CSV.ToString(), true, CultureInfo.InvariantCulture) == 0)
                        {
                            TranslationFileType = FileType.CSV;
                        }
                        else 
                        {
                            TranslationFileType = FileType.TXT;
                        }
                    }
                }
            }

            

            // Rule #5: If the output file name is empty, we act accordingly
            if (string.IsNullOrEmpty(Output))
            {
                // Rule #5.1: If it is parse, we default to [input file name].csv
                if (ToParse)
                {
                    string fileName = Path.GetFileNameWithoutExtension(Input);                    
                    Output = fileName + "." + FileType.CSV.ToString();
                    TranslationFileType = FileType.CSV;
                }
                else  
                {
                    // Rule #5.2: If it is generating, and the output can't be empty
                    return StringLoader.Get("OutputDirectoryNeeded");
                }
                
            }else
            {                
                // output isn't null, we will determind the Output file type                
                // Rule #6: if it is parsing. It will be .csv or .txt.
                if (ToParse)
                {
                    string fileName;
                    string outputDir;

                    if (Directory.Exists(Output))
                    {
                        // the output is actually a directory name
                        fileName = string.Empty;
                        outputDir = Output;
                    }
                    else
                    {
                        // get the extension
                        fileName = Path.GetFileName(Output);
                        outputDir = Path.GetDirectoryName(Output);
                    }

                    // Rule #6.1: if it is just the output directory
                    // we append the input file name as the output + csv as default
                    if (string.IsNullOrEmpty(fileName))
                    {
                        TranslationFileType = FileType.CSV;
                        Output = outputDir  
                               + Path.DirectorySeparatorChar 
                               + Path.GetFileName(Input) 
                               + "." 
                               + TranslationFileType.ToString();
                    }
                    else
                    {
                        // Rule #6.2: if we have file name, check the extension.
                        string extension = Path.GetExtension(Output);

                        // ignore case and invariant culture
                        if (string.Compare(extension, "." + FileType.CSV.ToString(), true, CultureInfo.InvariantCulture) == 0)
                        {
                            TranslationFileType = FileType.CSV;
                        }
                        else
                        {
                            // just consider the output as txt format if it doesn't have .csv extension
                            TranslationFileType = FileType.TXT;
                        }
                    }                    
                }
                else
                {
                    // it is to generate. And Output should point to the directory name.                    
                    if (!Directory.Exists(Output))
                        return StringLoader.Get("OutputDirectoryError", Output);
                }
            }

            // Rule #7: if the input assembly path is not null
            if (AssemblyPaths != null && AssemblyPaths.Count > 0)
            {
                Assemblies = new Assembly[AssemblyPaths.Count];
                for (int i = 0; i < Assemblies.Length; i++)
                {
                    string errorMsg = null;
                    try
                    {
                        // load the assembly
                        Assemblies[i] = Assembly.LoadFrom((string) AssemblyPaths[i]);
                    }
                    catch (ArgumentException argumentError)
                    {
                        errorMsg = argumentError.Message;   
                    }
                    catch (BadImageFormatException formatError)
                    {
                        errorMsg = formatError.Message;   
                    }
                    catch (FileNotFoundException fileError)
                    {
                        errorMsg = fileError.Message;
                    }
                    catch (PathTooLongException pathError)
                    {
                        errorMsg = pathError.Message;
                    }
                    catch (SecurityException securityError)
                    {

                        errorMsg = securityError.Message;
                    }

                    if (errorMsg != null)
                    {
                        return errorMsg; // return error message when loading this assembly
                    }
                }               
            }

            // if we come to this point, we are all fine, return null error message
            return null;
        }


        /// <summary>
        /// Write message line depending on IsVerbose flag
        /// </summary>
        internal void WriteLine(string message)
        {
            if (IsVerbose)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Write the message depending on IsVerbose flag
        /// </summary>        
        internal void Write(string message)
        {
            if (IsVerbose)
            {
                Console.Write(message);
            }
        }
    }
    
    internal enum FileType
    {
        NONE = 0,
        BAML,
        RESOURCES,
        DLL,
        CSV,
        TXT,
        EXE,
    }

    #endregion    
}


