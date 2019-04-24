// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//---------------------------------------------------------------------------
//
// Description: LocBaml command line parsing tool. 
//
//---------------------------------------------------------------------------


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Globalization;

namespace BamlLocalization 
{

    internal class Option
    {
        private String m_strName;
        private String m_strValue;

        public Option(String strName, String strValue)
        {
            m_strName = strName;
            m_strValue = strValue;
        }

        public String Name { get { return m_strName; } }
        public String Value { get { return m_strValue; } }
    }

    internal class Abbrevs
    {
        private String[] m_aOptions;
        private bool[] m_bRequiresValue;
        private bool[] m_bCanHaveValue;

        public Abbrevs(String[] aOptions)
        {
            m_aOptions = new String[aOptions.Length];
            m_bRequiresValue = new bool[aOptions.Length];
            m_bCanHaveValue = new bool[aOptions.Length];

            // Store option list in lower case for canonical comparison.
            for (int i = 0; i < aOptions.Length; i++)
            {
                String strOption = aOptions[i].ToLower(CultureInfo.InvariantCulture);

                // A leading '*' implies the option requires a value
                // (the '*' itself is not stored in the option name).
                if (strOption.StartsWith("*"))
                {
                    m_bRequiresValue[i] = true;
                    m_bCanHaveValue[i] = true;
                    strOption = strOption.Substring(1);
                }
                else if (strOption.StartsWith("+"))
                {
                    m_bRequiresValue[i] = false;
                    m_bCanHaveValue[i] = true;
                    strOption = strOption.Substring(1);
                }

                m_aOptions[i] = strOption;
            }
        }

        public String Lookup(String strOpt, out bool bRequiresValue, out bool bCanHaveValue)
        {
            String strOptLower = strOpt.ToLower(CultureInfo.InvariantCulture);
            int i;
            bool bMatched = false;
            int iMatch = -1;

            // Compare option to stored list.
            for (i = 0; i < m_aOptions.Length; i++)
            {
                // Exact matches always cause immediate termination of
                // the search
                if (strOptLower.Equals(m_aOptions[i]))
                {
                    bRequiresValue = m_bRequiresValue[i];
                    bCanHaveValue = m_bCanHaveValue[i];
                    return m_aOptions[i];
                }

                // Check for potential match (the input word is a prefix
                // of the current stored option).
                if (m_aOptions[i].StartsWith(strOptLower))
                {
                    // If we've already seen a prefix match then the
                    // input word is ambiguous.
                    if (bMatched)
                        throw new ArgumentException(StringLoader.Get("Err_AmbigousOption", strOpt));

                    // Remember this partial match.
                    bMatched = true;
                    iMatch = i;
                }
            }

            // If we get here with bMatched set, we saw one and only one
            // partial match, so we've got a winner.
            if (bMatched)
            {
                bRequiresValue = m_bRequiresValue[iMatch];
                bCanHaveValue = m_bCanHaveValue[iMatch];
                return m_aOptions[iMatch];
            }

            // Else the word doesn't match at all.
            throw new ArgumentException(StringLoader.Get("Err_UnknownOption", strOpt));
        }
    }

    internal class CommandLine
    {
        private String[] m_aArgList;
        private Option[] m_aOptList;
        private int m_iArgCursor;
        private int m_iOptCursor;
        private Abbrevs m_sValidOptions;

        public CommandLine(String[] aArgs, String[] aValidOpts)
        {
            int i, iArg, iOpt;

            // Keep a list of valid option names.
            m_sValidOptions = new Abbrevs(aValidOpts);

            // Temporary lists of raw arguments and options and their
            // associated values.
            String[] aArgList = new String[aArgs.Length];
            Option[] aOptList = new Option[aArgs.Length];

            // Reset counters of raw arguments and option/value pairs found
            // so far.
            iArg = 0;
            iOpt = 0;

            // Iterate through words of command line.
            for (i = 0; i < aArgs.Length; i++)
            {
                // Check for option or raw argument.
                if (aArgs[i].StartsWith("/") ||
                    aArgs[i].StartsWith("-"))
                {
                    String strOpt;
                    String strVal = null;
                    bool bRequiresValue;
                    bool bCanHaveValue;

                    // It's an option. Strip leading '/' or '-' and
                    // anything after a value separator (':' or
                    // '=').
                    int iColon = aArgs[i].IndexOfAny(new char[] {':', '='});
                    if (iColon == -1)
                            strOpt = aArgs[i].Substring(1);
                    else
                            strOpt = aArgs[i].Substring(1, iColon - 1);

                    // Look it up in the table of valid options (to
                    // check it exists, get the full option name and
                    // to see if an associated value is expected).
                    strOpt = m_sValidOptions.Lookup(strOpt, out bRequiresValue, out bCanHaveValue);

                    // Check that the user hasn't specified a value separator for an option 
                    // that doesn't take a value.
                    if (!bCanHaveValue && (iColon != -1))
                        throw new ApplicationException(StringLoader.Get("Err_NoValueRequired", strOpt));

                    // Check that the user has put a colon if the option requires a value.
                    if (bRequiresValue && (iColon == -1))
                        throw new ApplicationException(StringLoader.Get("Err_ValueRequired", strOpt));
                    
                    // Go look for a value if there is one.
                    if (bCanHaveValue && iColon != -1)
                    {
                        if (iColon == (aArgs[i].Length - 1))
                        {
                            // No value separator, or
                            // separator is at end of
                            // option; look for value in
                            // next command line arg.
                            if (i + 1 == aArgs.Length)
                            {
                                throw new ApplicationException(StringLoader.Get("Err_ValueRequired", strOpt));
                            }
                            else
                            {
                                if ((aArgs[i + 1].StartsWith( "/" ) || aArgs[i + 1].StartsWith( "-" )))
                                    throw new ApplicationException(StringLoader.Get("Err_ValueRequired", strOpt));

                                strVal = aArgs[i+1];
                                i++;
                            }
                        }
                        else
                        {
                            // Value is in same command line
                            // arg as the option, substring
                            // it out.
                            strVal = aArgs[i].Substring(iColon + 1);
                        }
                    }

                    // Build the option value pair.
                    aOptList[iOpt++] = new Option(strOpt, strVal);
                }
                else
                {
                    // Command line word is a raw argument.
                    aArgList[iArg++] = aArgs[i];
                }
            }

            // Allocate the non-temporary arg and option lists at exactly
            // the right size.
            m_aArgList = new String[iArg];
            m_aOptList = new Option[iOpt];

            // Copy in the values we've calculated.
            Array.Copy(aArgList, m_aArgList, iArg);
            Array.Copy(aOptList, m_aOptList, iOpt);
        }

        public int NumArgs { get { return m_aArgList.Length; } }

        public int NumOpts { get { return m_aOptList.Length; } }

        public String GetNextArg()
        {
            if (m_iArgCursor >= m_aArgList.Length)
                return null;
            return m_aArgList[m_iArgCursor++];
        }

        public Option GetNextOption()
        {
            if (m_iOptCursor >= m_aOptList.Length)
                return null;
            return m_aOptList[m_iOptCursor++];
        }
    }

}
