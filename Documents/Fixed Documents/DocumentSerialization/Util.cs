// DocumentSerialize SDK Sample - Util.cs
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Xml;
using System.Security;

namespace DocumentSerialization
{
    public static class Util
    {
        public static Stream ConvertTextToStream(string text)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            byte[] encodedBytes = encoding.GetBytes(text);
            byte[] preamble = encoding.GetPreamble();
            byte[] identifiableContent;

            if (preamble.Length == 0)
            {
                identifiableContent = encodedBytes;
            }
            else
            {
                identifiableContent = new byte[preamble.Length + encodedBytes.Length];
                preamble.CopyTo(identifiableContent, 0);
                encodedBytes.CopyTo(identifiableContent, preamble.Length);
            }

            return new MemoryStream(identifiableContent);
        }

        public static object DeSerializeObjectTree(string xaml)
        {
            Stream stream = ConvertTextToStream(xaml);
            ParserContext parserContext = new ParserContext();
            parserContext.BaseUri = new Uri("pack://siteoforigin:,,,/");
            return XamlReader.Load(stream, parserContext);
        }

        public static string SerializeObjectTree(object objectTree, XamlWriterMode expressionMode)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            XmlTextWriter xmlWriter = null;

            try
            {
                // Create XmlTextWriter
                xmlWriter = new XmlTextWriter(writer);

                // Set serialization mode
                XamlDesignerSerializationManager manager = new XamlDesignerSerializationManager(xmlWriter);
                manager.XamlWriterMode = expressionMode;
                // Serialize
                System.Windows.Markup.XamlWriter.Save(objectTree, manager);
            }
            finally
            {
                if (xmlWriter != null)
                    xmlWriter.Close();
            }

            return sb.ToString();
        }

        public static string GetSavedDataPath(string savedXamlFileName)
        {
            string path = null;

            path = Directory.GetCurrentDirectory();

            path = System.IO.Path.Combine(path, savedXamlFileName);
            return path;
        }

        public static FileStream GetStreamForSavedXamlFile(string savedDataPath, FileMode mode)
        {
#if XamlPadExpressApp
            // Get a store from Isolated Storage and see if xamlpad_saved.xaml exists
            // for this user/application.
            IsolatedStorageFile isoStore;
            try
            {
                isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            }

            catch (SecurityException)
            {
                // Just return null as if the saved file wasn't there.
                return null;
            }

            // Get a stream for this file
            try
            {
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(
                    savedXamlFileName,
                    mode,
                    isoStore);

                return stream;
            }

            catch (FileNotFoundException)
            {
                // We are trying to open an existing file but it's not there.
                // Just return null and we'll default to the initial content.
                return null;
            }

            catch (IsolatedStorageException e)
            {
                // Isolated Storage permissions may not be granted to this user.
                return null;
            }

#else
            // Use the rules for reading/writing the saved file -- if the app was deployed
            // with ClickOnce, it goes to/from the user's desktop, otherwise the current 
            // directory is used.
            try
            {
                FileStream stream = new FileStream(savedDataPath, mode);
                return stream;
            }

            catch (UnauthorizedAccessException)
            {
                return null;
            }

            catch (SecurityException)
            {
                return null;
            }

            catch (FileNotFoundException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
#endif
        }

        public static void FlushDispatcher()
        {
            FlushDispatcher(Dispatcher.CurrentDispatcher);
        }

        /// <summary></summary>
        /// <param name="ctx"></param>
        public static void FlushDispatcher(Dispatcher ctx)
        {
            FlushDispatcher(ctx, DispatcherPriority.SystemIdle);
        }

        /// <summary></summary>
        /// <param name="ctx"></param>
        /// <param name="priority"></param>
        public static void FlushDispatcher(Dispatcher ctx, DispatcherPriority priority)
        {
            ctx.Invoke(priority, new DispatcherOperationCallback(delegate { return null; }), null);
        }

        public static string GetIndentedXAML(string designXAML)
        {
            if (designXAML.Trim() == "") return "";
            TextReader treader = new StringReader(designXAML);
            XmlReader xmlReader = XmlReader.Create(treader);
            xmlReader.Read();
            string indentedXAML = "";

            while (!xmlReader.EOF)
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        string tabs = "";
                        for (int i = 0; i < xmlReader.Depth; i++) tabs += "    ";

                        if ((xmlReader.XmlSpace == XmlSpace.Preserve))
                        {
                            indentedXAML += "\n" + tabs + xmlReader.ReadOuterXml();
                            continue;
                        }

                        indentedXAML += "\n" + tabs + "<" + xmlReader.Name;
                        bool emptyElement = xmlReader.IsEmptyElement;
                        if (xmlReader.HasAttributes)
                        {
                            for (int i = 0; i < xmlReader.AttributeCount; i++)
                            {
                                xmlReader.MoveToAttribute(i);
                                indentedXAML += " " + xmlReader.Name + "=\"" + xmlReader.Value + "\"";
                            }
                        }

                        if (emptyElement) indentedXAML += "/";
                        indentedXAML += ">";
                        xmlReader.Read();
                        break;
                    case XmlNodeType.Text:
                        string origValue = xmlReader.Value;
                        origValue = origValue.Replace("&", "&amp;");
                        origValue = origValue.Replace("<", "&lt;");
                        origValue = origValue.Replace(">", "&gt;");
                        origValue = origValue.Replace("\"", "&quot;");
                        indentedXAML += origValue;
                        xmlReader.Read();
                        break;
                    case XmlNodeType.EndElement:
                        tabs = "";
                        for (int i = 0; i < xmlReader.Depth; i++) tabs += "    ";
                        indentedXAML += "\n" + tabs + "</" + xmlReader.Name + ">";
                        xmlReader.Read();
                        break;
                    case XmlNodeType.Whitespace:
                        //indentedXAML += xmlReader.Value;
                        xmlReader.Read();
                        break;
                    default:
                        xmlReader.Read();
                        break;
                }
            }

            // remove the first \n
            string realText = indentedXAML.Substring(1, indentedXAML.Length - 1);
            indentedXAML = realText;
            return indentedXAML;
        }

        private static void InsertBlockWithinParagraph(Paragraph origP, TextPointer _caretPosition, Block objectToInsert)
        {
            if (IsParagraphEmpty(origP))
            {
                FlowDocument fd = origP.Parent as FlowDocument;
                if (fd != null)
                {
                    fd.Blocks.InsertAfter(origP, objectToInsert);
                    fd.Blocks.Remove(origP);
                    return;
                }

                ListItem li = origP.Parent as ListItem;
                if (li != null)
                {
                    li.Blocks.InsertAfter(origP, objectToInsert);
                    li.Blocks.Remove(origP);
                    return;
                }
            }

            _caretPosition.InsertParagraphBreak();
            origP.SiblingBlocks.InsertAfter(origP, objectToInsert);
        }

        public static string GetFileName(string path)
        {
            string[] values = path.Split('/');

            foreach (string s in values)
            {
                if (s.ToLower().Contains(".jpg") || s.ToLower().Contains(".bmp") || s.ToLower().Contains(".png") || s.ToLower().Contains(".gif"))
                {
                    return s;
                }
            }
            return "";
        }

        public static string FindNumeric(string content)
        {
            string[] values = content.Split(' ');
            if (values != null)
            {
                return values[0];
            }

            return "none";
        }

        public static string GetBrushesFromString(string value)
        {
            if (string.Compare(value, Brushes.AliceBlue.ToString()) == 0) return "AliceBlue";
            if (string.Compare(value, Brushes.AntiqueWhite.ToString()) == 0) return "AntiqueWhite";
            if (string.Compare(value, Brushes.Aqua.ToString()) == 0) return "Aqua";
            if (string.Compare(value, Brushes.Aquamarine.ToString()) == 0) return "Aquamarine";
            if (string.Compare(value, Brushes.Azure.ToString()) == 0) return "Azure";
            if (string.Compare(value, Brushes.Beige.ToString()) == 0) return "Beige";
            if (string.Compare(value, Brushes.Bisque.ToString()) == 0) return "Bisque";
            if (string.Compare(value, Brushes.Black.ToString()) == 0) return "Black";
            if (string.Compare(value, Brushes.BlanchedAlmond.ToString()) == 0) return "BlanchedAlmond";
            if (string.Compare(value, Brushes.Blue.ToString()) == 0) return "Blue";
            if (string.Compare(value, Brushes.BlueViolet.ToString()) == 0) return "BlueViolet";
            if (string.Compare(value, Brushes.Brown.ToString()) == 0) return "Brown";
            if (string.Compare(value, Brushes.BurlyWood.ToString()) == 0) return "BurlyWood";
            if (string.Compare(value, Brushes.CadetBlue.ToString()) == 0) return "CadetBlue";
            if (string.Compare(value, Brushes.Chartreuse.ToString()) == 0) return "Chartreuse";
            if (string.Compare(value, Brushes.Chocolate.ToString()) == 0) return "Chocolate";
            if (string.Compare(value, Brushes.Coral.ToString()) == 0) return "Coral";
            if (string.Compare(value, Brushes.CornflowerBlue.ToString()) == 0) return "CornflowerBlue";
            if (string.Compare(value, Brushes.Cornsilk.ToString()) == 0) return "Cornsilk";
            if (string.Compare(value, Brushes.Crimson.ToString()) == 0) return "Crimson";
            if (string.Compare(value, Brushes.Cyan.ToString()) == 0) return "Cyan";
            if (string.Compare(value, Brushes.DarkBlue.ToString()) == 0) return "DarkBlue";
            if (string.Compare(value, Brushes.DarkCyan.ToString()) == 0) return "DarkCyan";
            if (string.Compare(value, Brushes.DarkGoldenrod.ToString()) == 0) return "DarkGoldenrod";
            if (string.Compare(value, Brushes.DarkGray.ToString()) == 0) return "DarkGray";
            if (string.Compare(value, Brushes.DarkGreen.ToString()) == 0) return "DarkGreen";
            if (string.Compare(value, Brushes.DarkKhaki.ToString()) == 0) return "DarkKhaki";
            if (string.Compare(value, Brushes.DarkMagenta.ToString()) == 0) return "DarkMagenta";
            if (string.Compare(value, Brushes.DarkOliveGreen.ToString()) == 0) return "DarkOliveGreen";
            if (string.Compare(value, Brushes.DarkOrange.ToString()) == 0) return "DarkOrange";
            if (string.Compare(value, Brushes.DarkOrchid.ToString()) == 0) return "DarkOrchid";
            if (string.Compare(value, Brushes.DarkRed.ToString()) == 0) return "DarkRed";
            if (string.Compare(value, Brushes.DarkSalmon.ToString()) == 0) return "DarkSalmon";
            if (string.Compare(value, Brushes.DarkSeaGreen.ToString()) == 0) return "DarkSeaGreen";
            if (string.Compare(value, Brushes.DarkSlateBlue.ToString()) == 0) return "DarkSlateBlue";
            if (string.Compare(value, Brushes.DarkSlateGray.ToString()) == 0) return "DarkSlateGray";
            if (string.Compare(value, Brushes.DarkTurquoise.ToString()) == 0) return "DarkTurquoise";
            if (string.Compare(value, Brushes.DarkViolet.ToString()) == 0) return "DarkViolet";
            if (string.Compare(value, Brushes.DeepPink.ToString()) == 0) return "DeepPink";
            if (string.Compare(value, Brushes.DeepSkyBlue.ToString()) == 0) return "DeepSkyBlue";
            if (string.Compare(value, Brushes.DimGray.ToString()) == 0) return "DimGray";
            if (string.Compare(value, Brushes.DodgerBlue.ToString()) == 0) return "DodgerBlue";
            if (string.Compare(value, Brushes.Firebrick.ToString()) == 0) return "Firebrick";
            if (string.Compare(value, Brushes.FloralWhite.ToString()) == 0) return "FloralWhite";
            if (string.Compare(value, Brushes.ForestGreen.ToString()) == 0) return "ForestGreen";
            if (string.Compare(value, Brushes.Fuchsia.ToString()) == 0) return "Fuchsia";
            if (string.Compare(value, Brushes.Gainsboro.ToString()) == 0) return "Gainsboro";
            if (string.Compare(value, Brushes.GhostWhite.ToString()) == 0) return "GhostWhite";
            if (string.Compare(value, Brushes.Gold.ToString()) == 0) return "Gold";
            if (string.Compare(value, Brushes.Goldenrod.ToString()) == 0) return "Goldenrod";
            if (string.Compare(value, Brushes.Gray.ToString()) == 0) return "Gray";
            if (string.Compare(value, Brushes.Green.ToString()) == 0) return "Green";
            if (string.Compare(value, Brushes.GreenYellow.ToString()) == 0) return "GreenYellow";
            if (string.Compare(value, Brushes.Honeydew.ToString()) == 0) return "Honeydew";
            if (string.Compare(value, Brushes.HotPink.ToString()) == 0) return "HotPink";
            if (string.Compare(value, Brushes.IndianRed.ToString()) == 0) return "IndianRed";
            if (string.Compare(value, Brushes.Indigo.ToString()) == 0) return "Indigo";
            if (string.Compare(value, Brushes.Ivory.ToString()) == 0) return "Ivory";
            if (string.Compare(value, Brushes.Khaki.ToString()) == 0) return "Khaki";
            if (string.Compare(value, Brushes.Lavender.ToString()) == 0) return "Lavender";
            if (string.Compare(value, Brushes.LavenderBlush.ToString()) == 0) return "LavenderBlush";
            if (string.Compare(value, Brushes.LawnGreen.ToString()) == 0) return "LawnGreen";
            if (string.Compare(value, Brushes.LemonChiffon.ToString()) == 0) return "LemonChiffon";
            if (string.Compare(value, Brushes.LightBlue.ToString()) == 0) return "LightBlue";
            if (string.Compare(value, Brushes.LightCoral.ToString()) == 0) return "LightCoral";
            if (string.Compare(value, Brushes.LightCyan.ToString()) == 0) return "LightCyan";
            if (string.Compare(value, Brushes.LightGoldenrodYellow.ToString()) == 0) return "LightGoldenrodYellow";
            if (string.Compare(value, Brushes.LightGray.ToString()) == 0) return "LightGray";
            if (string.Compare(value, Brushes.LightGreen.ToString()) == 0) return "LightGreen";
            if (string.Compare(value, Brushes.LightPink.ToString()) == 0) return "LightPink";
            if (string.Compare(value, Brushes.LightSalmon.ToString()) == 0) return "LightSalmon";
            if (string.Compare(value, Brushes.LightSeaGreen.ToString()) == 0) return "LightSeaGreen";
            if (string.Compare(value, Brushes.LightSkyBlue.ToString()) == 0) return "LightSkyBlue";
            if (string.Compare(value, Brushes.LightSlateGray.ToString()) == 0) return "LightSlateGray";
            if (string.Compare(value, Brushes.LightSteelBlue.ToString()) == 0) return "LightSteelBlue";
            if (string.Compare(value, Brushes.LightYellow.ToString()) == 0) return "LightYellow";
            if (string.Compare(value, Brushes.Lime.ToString()) == 0) return "Lime";
            if (string.Compare(value, Brushes.LimeGreen.ToString()) == 0) return "LimeGreen";
            if (string.Compare(value, Brushes.Linen.ToString()) == 0) return "Linen";
            if (string.Compare(value, Brushes.Magenta.ToString()) == 0) return "Magenta";
            if (string.Compare(value, Brushes.Maroon.ToString()) == 0) return "Maroon";
            if (string.Compare(value, Brushes.MediumAquamarine.ToString()) == 0) return "MediumAquamarine";
            if (string.Compare(value, Brushes.MediumBlue.ToString()) == 0) return "MediumBlue";
            if (string.Compare(value, Brushes.MediumOrchid.ToString()) == 0) return "MediumOrchid";
            if (string.Compare(value, Brushes.MediumPurple.ToString()) == 0) return "MediumPurple";
            if (string.Compare(value, Brushes.MediumSeaGreen.ToString()) == 0) return "MediumSeaGreen";
            if (string.Compare(value, Brushes.MediumSlateBlue.ToString()) == 0) return "MediumSlateBlue";
            if (string.Compare(value, Brushes.MediumSpringGreen.ToString()) == 0) return "MediumSpringGreen";
            if (string.Compare(value, Brushes.MediumTurquoise.ToString()) == 0) return "MediumTurquoise";
            if (string.Compare(value, Brushes.MediumVioletRed.ToString()) == 0) return "MediumVioletRed";
            if (string.Compare(value, Brushes.MidnightBlue.ToString()) == 0) return "MidnightBlue";
            if (string.Compare(value, Brushes.MintCream.ToString()) == 0) return "MintCream";
            if (string.Compare(value, Brushes.MistyRose.ToString()) == 0) return "MistyRose";
            if (string.Compare(value, Brushes.Moccasin.ToString()) == 0) return "Moccasin";
            if (string.Compare(value, Brushes.NavajoWhite.ToString()) == 0) return "NavajoWhite";
            if (string.Compare(value, Brushes.Navy.ToString()) == 0) return "Navy";
            if (string.Compare(value, Brushes.OldLace.ToString()) == 0) return "OldLace";
            if (string.Compare(value, Brushes.Olive.ToString()) == 0) return "Olive";
            if (string.Compare(value, Brushes.OliveDrab.ToString()) == 0) return "OliveDrab";
            if (string.Compare(value, Brushes.Orange.ToString()) == 0) return "Orange";
            if (string.Compare(value, Brushes.OrangeRed.ToString()) == 0) return "OrangeRed";
            if (string.Compare(value, Brushes.Orchid.ToString()) == 0) return "Orchid";
            if (string.Compare(value, Brushes.PaleGoldenrod.ToString()) == 0) return "PaleGoldenrod";
            if (string.Compare(value, Brushes.PaleGreen.ToString()) == 0) return "AntiqueWhite";
            if (string.Compare(value, Brushes.PaleTurquoise.ToString()) == 0) return "PaleGreen";
            if (string.Compare(value, Brushes.PaleVioletRed.ToString()) == 0) return "PaleVioletRed";
            if (string.Compare(value, Brushes.PapayaWhip.ToString()) == 0) return "PapayaWhip";
            if (string.Compare(value, Brushes.PeachPuff.ToString()) == 0) return "PeachPuff";
            if (string.Compare(value, Brushes.Peru.ToString()) == 0) return "Peru";
            if (string.Compare(value, Brushes.Pink.ToString()) == 0) return "Pink";
            if (string.Compare(value, Brushes.Plum.ToString()) == 0) return "Plum";
            if (string.Compare(value, Brushes.PowderBlue.ToString()) == 0) return "PowderBlue";
            if (string.Compare(value, Brushes.Purple.ToString()) == 0) return "Purple";
            if (string.Compare(value, Brushes.Red.ToString()) == 0) return "Red";
            if (string.Compare(value, Brushes.RosyBrown.ToString()) == 0) return "RosyBrown";
            if (string.Compare(value, Brushes.RoyalBlue.ToString()) == 0) return "RoyalBlue";
            if (string.Compare(value, Brushes.SaddleBrown.ToString()) == 0) return "SaddleBrown";
            if (string.Compare(value, Brushes.Salmon.ToString()) == 0) return "Salmon";
            if (string.Compare(value, Brushes.SandyBrown.ToString()) == 0) return "SandyBrown";
            if (string.Compare(value, Brushes.SeaGreen.ToString()) == 0) return "SeaGreen";
            if (string.Compare(value, Brushes.SeaShell.ToString()) == 0) return "SeaShell";
            if (string.Compare(value, Brushes.Sienna.ToString()) == 0) return "Sienna";
            if (string.Compare(value, Brushes.Silver.ToString()) == 0) return "Silver";
            if (string.Compare(value, Brushes.SkyBlue.ToString()) == 0) return "SkyBlue";
            if (string.Compare(value, Brushes.SlateBlue.ToString()) == 0) return "SlateBlue";
            if (string.Compare(value, Brushes.SlateGray.ToString()) == 0) return "SlateGray";
            if (string.Compare(value, Brushes.Snow.ToString()) == 0) return "Snow";
            if (string.Compare(value, Brushes.SpringGreen.ToString()) == 0) return "SpringGreen";
            if (string.Compare(value, Brushes.SteelBlue.ToString()) == 0) return "SteelBlue";
            if (string.Compare(value, Brushes.Tan.ToString()) == 0) return "Tan";
            if (string.Compare(value, Brushes.Teal.ToString()) == 0) return "Teal";
            if (string.Compare(value, Brushes.Thistle.ToString()) == 0) return "Thistle";
            if (string.Compare(value, Brushes.Tomato.ToString()) == 0) return "Tomato";
            if (string.Compare(value, Brushes.Transparent.ToString()) == 0) return "Transparent";
            if (string.Compare(value, Brushes.Turquoise.ToString()) == 0) return "Turquoise";
            if (string.Compare(value, Brushes.Violet.ToString()) == 0) return "Violet";
            if (string.Compare(value, Brushes.Wheat.ToString()) == 0) return "Wheat";
            if (string.Compare(value, Brushes.White.ToString()) == 0) return "White";
            if (string.Compare(value, Brushes.WhiteSmoke.ToString()) == 0) return "WhiteSmoke";
            if (string.Compare(value, Brushes.Yellow.ToString()) == 0) return "Yellow";
            if (string.Compare(value, Brushes.YellowGreen.ToString()) == 0) return "YellowGreen";

            return "color not specified";
        }

        public static SolidColorBrush ColorStringToBrushes(string colorString)
        {
            colorString = colorString.Trim();
            if (null != colorString)
            {
                // We use invariant culture because we don't globalize our color names
                string colorUpper = colorString.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

                // Use String.Equals because it does explicit equality
                // StartsWith/EndsWith are culture sensitive and are 4-7 times slower than Equals

                switch (colorUpper.Length)
                {
                    case 3:
                        if (colorUpper.Equals("RED")) return Brushes.Red;
                        if (colorUpper.Equals("TAN")) return Brushes.Tan;
                        break;
                    case 4:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("AQUA")) return Brushes.Aqua;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BLUE")) return Brushes.Blue;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CYAN")) return Brushes.Cyan;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GOLD")) return Brushes.Gold;
                                if (colorUpper.Equals("GRAY")) return Brushes.Gray;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIME")) return Brushes.Lime;
                                break;
                            case 'N':
                                if (colorUpper.Equals("NAVY")) return Brushes.Navy;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PERU")) return Brushes.Peru;
                                if (colorUpper.Equals("PINK")) return Brushes.Pink;
                                if (colorUpper.Equals("PLUM")) return Brushes.Plum;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SNOW")) return Brushes.Snow;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TEAL")) return Brushes.Teal;
                                break;
                        }
                        break;
                    case 5:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("AZURE")) return Brushes.Azure;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BEIGE")) return Brushes.Beige;
                                if (colorUpper.Equals("BLACK")) return Brushes.Black;
                                if (colorUpper.Equals("BROWN")) return Brushes.Brown;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CORAL")) return Brushes.Coral;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GREEN")) return Brushes.Green;
                                break;
                            case 'I':
                                if (colorUpper.Equals("IVORY")) return Brushes.Ivory;
                                break;
                            case 'K':
                                if (colorUpper.Equals("KHAKI")) return Brushes.Khaki;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LINEN")) return Brushes.Linen;
                                break;
                            case 'O':
                                if (colorUpper.Equals("OLIVE")) return Brushes.Olive;
                                break;
                            case 'W':
                                if (colorUpper.Equals("WHEAT")) return Brushes.Wheat;
                                if (colorUpper.Equals("WHITE")) return Brushes.White;
                                break;
                        }
                        break;
                    case 6:
                        switch (colorUpper[0])
                        {
                            case 'B':
                                if (colorUpper.Equals("BISQUE")) return Brushes.Bisque;
                                break;
                            case 'I':
                                if (colorUpper.Equals("INDIGO")) return Brushes.Indigo;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MAROON")) return Brushes.Maroon;
                                break;
                            case 'O':
                                if (colorUpper.Equals("ORANGE")) return Brushes.Orange;
                                if (colorUpper.Equals("ORCHID")) return Brushes.Orchid;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PURPLE")) return Brushes.Purple;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SALMON")) return Brushes.Salmon;
                                if (colorUpper.Equals("SIENNA")) return Brushes.Sienna;
                                if (colorUpper.Equals("SILVER")) return Brushes.Silver;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TOMATO")) return Brushes.Tomato;
                                break;
                            case 'V':
                                if (colorUpper.Equals("VIOLET")) return Brushes.Violet;
                                break;
                            case 'Y':
                                if (colorUpper.Equals("YELLOW")) return Brushes.Yellow;
                                break;
                        }
                        break;
                    case 7:
                        switch (colorUpper[0])
                        {
                            case 'C':
                                if (colorUpper.Equals("CRIMSON")) return Brushes.Crimson;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKRED")) return Brushes.DarkRed;
                                if (colorUpper.Equals("DIMGRAY")) return Brushes.DimGray;
                                break;
                            case 'F':
                                if (colorUpper.Equals("FUCHSIA")) return Brushes.Fuchsia;
                                break;
                            case 'H':
                                if (colorUpper.Equals("HOTPINK")) return Brushes.HotPink;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MAGENTA")) return Brushes.Magenta;
                                break;
                            case 'O':
                                if (colorUpper.Equals("OLDLACE")) return Brushes.OldLace;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SKYBLUE")) return Brushes.SkyBlue;
                                break;
                            case 'T':
                                if (colorUpper.Equals("THISTLE")) return Brushes.Thistle;
                                break;
                        }
                        break;
                    case 8:
                        switch (colorUpper[0])
                        {
                            case 'C':
                                if (colorUpper.Equals("CORNSILK")) return Brushes.Cornsilk;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKBLUE")) return Brushes.DarkBlue;
                                if (colorUpper.Equals("DARKCYAN")) return Brushes.DarkCyan;
                                if (colorUpper.Equals("DARKGRAY")) return Brushes.DarkGray;
                                if (colorUpper.Equals("DEEPPINK")) return Brushes.DeepPink;
                                break;
                            case 'H':
                                if (colorUpper.Equals("HONEYDEW")) return Brushes.Honeydew;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LAVENDER")) return Brushes.Lavender;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MOCCASIN")) return Brushes.Moccasin;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SEAGREEN")) return Brushes.SeaGreen;
                                if (colorUpper.Equals("SEASHELL")) return Brushes.SeaShell;
                                break;
                        }
                        break;
                    case 9:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("ALICEBLUE")) return Brushes.AliceBlue;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BURLYWOOD")) return Brushes.BurlyWood;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CADETBLUE")) return Brushes.CadetBlue;
                                if (colorUpper.Equals("CHOCOLATE")) return Brushes.Chocolate;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKGREEN")) return Brushes.DarkGreen;
                                if (colorUpper.Equals("DARKKHAKI")) return Brushes.DarkKhaki;
                                break;
                            case 'F':
                                if (colorUpper.Equals("FIREBRICK")) return Brushes.Firebrick;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GAINSBORO")) return Brushes.Gainsboro;
                                if (colorUpper.Equals("GOLDENROD")) return Brushes.Goldenrod;
                                break;
                            case 'I':
                                if (colorUpper.Equals("INDIANRED")) return Brushes.IndianRed;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LAWNGREEN")) return Brushes.LawnGreen;
                                if (colorUpper.Equals("LIGHTBLUE")) return Brushes.LightBlue;
                                if (colorUpper.Equals("LIGHTCYAN")) return Brushes.LightCyan;
                                if (colorUpper.Equals("LIGHTGRAY")) return Brushes.LightGray;
                                if (colorUpper.Equals("LIGHTPINK")) return Brushes.LightPink;
                                if (colorUpper.Equals("LIMEGREEN")) return Brushes.LimeGreen;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MINTCREAM")) return Brushes.MintCream;
                                if (colorUpper.Equals("MISTYROSE")) return Brushes.MistyRose;
                                break;
                            case 'O':
                                if (colorUpper.Equals("OLIVEDRAB")) return Brushes.OliveDrab;
                                if (colorUpper.Equals("ORANGERED")) return Brushes.OrangeRed;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PALEGREEN")) return Brushes.PaleGreen;
                                if (colorUpper.Equals("PEACHPUFF")) return Brushes.PeachPuff;
                                break;
                            case 'R':
                                if (colorUpper.Equals("ROSYBROWN")) return Brushes.RosyBrown;
                                if (colorUpper.Equals("ROYALBLUE")) return Brushes.RoyalBlue;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SLATEBLUE")) return Brushes.SlateBlue;
                                if (colorUpper.Equals("SLATEGRAY")) return Brushes.SlateGray;
                                if (colorUpper.Equals("STEELBLUE")) return Brushes.SteelBlue;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TURQUOISE")) return Brushes.Turquoise;
                                break;
                        }
                        break;
                    case 10:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("AQUAMARINE")) return Brushes.Aquamarine;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BLUEVIOLET")) return Brushes.BlueViolet;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CHARTREUSE")) return Brushes.Chartreuse;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKORANGE")) return Brushes.DarkOrange;
                                if (colorUpper.Equals("DARKORCHID")) return Brushes.DarkOrchid;
                                if (colorUpper.Equals("DARKSALMON")) return Brushes.DarkSalmon;
                                if (colorUpper.Equals("DARKVIOLET")) return Brushes.DarkViolet;
                                if (colorUpper.Equals("DODGERBLUE")) return Brushes.DodgerBlue;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GHOSTWHITE")) return Brushes.GhostWhite;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTCORAL")) return Brushes.LightCoral;
                                if (colorUpper.Equals("LIGHTGREEN")) return Brushes.LightGreen;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MEDIUMBLUE")) return Brushes.MediumBlue;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PAPAYAWHIP")) return Brushes.PapayaWhip;
                                if (colorUpper.Equals("POWDERBLUE")) return Brushes.PowderBlue;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SANDYBROWN")) return Brushes.SandyBrown;
                                break;
                            case 'W':
                                if (colorUpper.Equals("WHITESMOKE")) return Brushes.WhiteSmoke;
                                break;
                        }
                        break;
                    case 11:
                        switch (colorUpper[0])
                        {
                            case 'D':
                                if (colorUpper.Equals("DARKMAGENTA")) return Brushes.DarkMagenta;
                                if (colorUpper.Equals("DEEPSKYBLUE")) return Brushes.DeepSkyBlue;
                                break;
                            case 'F':
                                if (colorUpper.Equals("FLORALWHITE")) return Brushes.FloralWhite;
                                if (colorUpper.Equals("FORESTGREEN")) return Brushes.ForestGreen;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GREENYELLOW")) return Brushes.GreenYellow;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSALMON")) return Brushes.LightSalmon;
                                if (colorUpper.Equals("LIGHTYELLOW")) return Brushes.LightYellow;
                                break;
                            case 'N':
                                if (colorUpper.Equals("NAVAJOWHITE")) return Brushes.NavajoWhite;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SADDLEBROWN")) return Brushes.SaddleBrown;
                                if (colorUpper.Equals("SPRINGGREEN")) return Brushes.SpringGreen;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TRANSPARENT")) return Brushes.Transparent;
                                break;
                            case 'Y':
                                if (colorUpper.Equals("YELLOWGREEN")) return Brushes.YellowGreen;
                                break;
                        }
                        break;
                    case 12:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("ANTIQUEWHITE")) return Brushes.AntiqueWhite;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKSEAGREEN")) return Brushes.DarkSeaGreen;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSKYBLUE")) return Brushes.LightSkyBlue;
                                if (colorUpper.Equals("LEMONCHIFFON")) return Brushes.LemonChiffon;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MEDIUMORCHID")) return Brushes.MediumOrchid;
                                if (colorUpper.Equals("MEDIUMPURPLE")) return Brushes.MediumPurple;
                                if (colorUpper.Equals("MIDNIGHTBLUE")) return Brushes.MidnightBlue;
                                break;
                        }
                        break;
                    case 13:
                        switch (colorUpper[0])
                        {
                            case 'D':
                                if (colorUpper.Equals("DARKSLATEBLUE")) return Brushes.DarkSlateBlue;
                                if (colorUpper.Equals("DARKSLATEGRAY")) return Brushes.DarkSlateGray;
                                if (colorUpper.Equals("DARKGOLDENROD")) return Brushes.DarkGoldenrod;
                                if (colorUpper.Equals("DARKTURQUOISE")) return Brushes.DarkTurquoise;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSEAGREEN")) return Brushes.LightSeaGreen;
                                if (colorUpper.Equals("LAVENDERBLUSH")) return Brushes.LavenderBlush;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PALEGOLDENROD")) return Brushes.PaleGoldenrod;
                                if (colorUpper.Equals("PALETURQUOISE")) return Brushes.PaleTurquoise;
                                if (colorUpper.Equals("PALEVIOLETRED")) return Brushes.PaleVioletRed;
                                break;
                        }
                        break;
                    case 14:
                        switch (colorUpper[0])
                        {
                            case 'B':
                                if (colorUpper.Equals("BLANCHEDALMOND")) return Brushes.BlanchedAlmond;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CORNFLOWERBLUE")) return Brushes.CornflowerBlue;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKOLIVEGREEN")) return Brushes.DarkOliveGreen;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSLATEGRAY")) return Brushes.LightSlateGray;
                                if (colorUpper.Equals("LIGHTSTEELBLUE")) return Brushes.LightSteelBlue;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MEDIUMSEAGREEN")) return Brushes.MediumSeaGreen;
                                break;
                        }
                        break;
                    case 15:
                        if (colorUpper.Equals("MEDIUMSLATEBLUE")) return Brushes.MediumSlateBlue;
                        if (colorUpper.Equals("MEDIUMTURQUOISE")) return Brushes.MediumTurquoise;
                        if (colorUpper.Equals("MEDIUMVIOLETRED")) return Brushes.MediumVioletRed;
                        break;
                    case 16:
                        if (colorUpper.Equals("MEDIUMAQUAMARINE")) return Brushes.MediumAquamarine;
                        break;
                    case 17:
                        if (colorUpper.Equals("MEDIUMSPRINGGREEN")) return Brushes.MediumSpringGreen;
                        break;
                    case 20:
                        if (colorUpper.Equals("LIGHTGOLDENRODYELLOW")) return Brushes.LightGoldenrodYellow;
                        break;
                }
            }
            return null;
        }

        public static List<UIElement> UIElementsInElement(FrameworkContentElement parent)
        {
            List<UIElement> UIEresults = new List<UIElement>();
            GetUIElementsRecursively(parent as DependencyObject, UIEresults);

            return UIEresults;
        }

        static void GetUIElementsRecursively(DependencyObject dObj, List<UIElement> UIresults)
        {
            if (dObj == null)
            {
                return;
            }

            UIElement uiElement = dObj as UIElement;
            if (uiElement != null)
            {
                UIresults.Add(uiElement);
            }

            foreach (object o in LogicalTreeHelper.GetChildren(dObj))
            {
                GetUIElementsRecursively(o as DependencyObject, UIresults);
            }
        }

        static void GetContentElementsRecursively(DependencyObject dObj, List<ContentElement> CEresults)
        {
            if (dObj == null)
            {
                return;
            }

            ContentElement ceElement = dObj as ContentElement;
            if (ceElement != null)
            {
                CEresults.Add(ceElement);
            }

            foreach (object o in LogicalTreeHelper.GetChildren(dObj))
            {
                GetContentElementsRecursively(o as DependencyObject, CEresults);
            }
        }

        static public bool IsParagraphEmpty(Paragraph suspect)
        {
            if (suspect.Inlines.Count == 0)
                return true;

            if (suspect.Inlines.Count == 1)
            {
                Run r = suspect.Inlines.FirstInline as Run;
                if (r != null)
                {
                    if (r.Text == "")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static void FindParentParagraph(Span child, ref Paragraph parent)
        {
            if (child.Parent is Paragraph)
            {
                parent = child.Parent as Paragraph;
                return;
            }
            else
            {
                FindParentParagraph(child.Parent as Span, ref parent);
            }
        }
    }
}
