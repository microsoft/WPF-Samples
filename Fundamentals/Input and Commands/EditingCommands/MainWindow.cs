// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace EditingCommands
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExecuteCommand(object sender, RoutedEventArgs args)
        {
            var command = ((Button) sender).Name;

            var target = routeToRTB.IsChecked.Value
                ? sampleRTB
                : (routeToTB.IsChecked.Value ? sampleTB : samplePWB as IInputElement);

            switch (command)
            {
                // Paragraph Alignment
                case "AlignCenter":
                    System.Windows.Documents.EditingCommands.AlignCenter.Execute(null, target);
                    break;
                case "AlignJustify":
                    System.Windows.Documents.EditingCommands.AlignJustify.Execute(null, target);
                    break;
                case "AlignRight":
                    System.Windows.Documents.EditingCommands.AlignRight.Execute(null, target);
                    break;
                case "AlignLeft":
                    System.Windows.Documents.EditingCommands.AlignLeft.Execute(null, target);
                    break;

                //Caret Movement by Line
                case "MoveUpByLine":
                    System.Windows.Documents.EditingCommands.MoveUpByLine.Execute(null, target);
                    break;
                case "MoveDownByLine":
                    System.Windows.Documents.EditingCommands.MoveDownByLine.Execute(null, target);
                    break;
                case "MoveToLineEnd":
                    System.Windows.Documents.EditingCommands.MoveToLineEnd.Execute(null, target);
                    break;
                case "MoveToLineStart":
                    System.Windows.Documents.EditingCommands.MoveToLineStart.Execute(null, target);
                    break;

                //Caret Movement by Character
                case "MoveLeftByCharacter":
                    System.Windows.Documents.EditingCommands.MoveLeftByCharacter.Execute(null, target);
                    break;
                case "MoveRightByCharacter":
                    System.Windows.Documents.EditingCommands.MoveRightByCharacter.Execute(null, target);
                    break;

                //Caret Movement by Word
                case "MoveLeftByWord":
                    System.Windows.Documents.EditingCommands.MoveLeftByWord.Execute(null, target);
                    break;
                case "MoveRightByWord":
                    System.Windows.Documents.EditingCommands.MoveRightByWord.Execute(null, target);
                    break;

                //Caret Movement by Paragraph
                case "MoveUpByParagraph":
                    System.Windows.Documents.EditingCommands.MoveUpByParagraph.Execute(null, target);
                    break;
                case "MoveDownByParagraph":
                    System.Windows.Documents.EditingCommands.MoveDownByParagraph.Execute(null, target);
                    break;

                //Caret Movement by Page
                case "MoveUpByPage":
                    System.Windows.Documents.EditingCommands.MoveUpByPage.Execute(null, target);
                    break;
                case "MoveDownByPage":
                    System.Windows.Documents.EditingCommands.MoveDownByPage.Execute(null, target);
                    break;

                //Caret Movement by Document
                case "MoveToDocumentEnd":
                    System.Windows.Documents.EditingCommands.MoveToDocumentEnd.Execute(null, target);
                    break;
                case "MoveToDocumentStart":
                    System.Windows.Documents.EditingCommands.MoveToDocumentStart.Execute(null, target);
                    break;

                //Deletion
                case "Delete":
                    System.Windows.Documents.EditingCommands.Delete.Execute(null, target);
                    break;
                case "DeleteNextWord":
                    System.Windows.Documents.EditingCommands.DeleteNextWord.Execute(null, target);
                    break;
                case "DeletePreviousWord":
                    System.Windows.Documents.EditingCommands.DeletePreviousWord.Execute(null, target);
                    break;

                //Spelling Errors
                case "CorrectSpellingError":
                    System.Windows.Documents.EditingCommands.CorrectSpellingError.Execute(null, target);
                    break;
                case "IgnoreSpellingError":
                    System.Windows.Documents.EditingCommands.IgnoreSpellingError.Execute(null, target);
                    break;

                //Toggle Insert
                case "ToggleInsert":
                    System.Windows.Documents.EditingCommands.ToggleInsert.Execute(null, target);
                    break;

                //Symbol Entry
                case "Backspace":
                    System.Windows.Documents.EditingCommands.Backspace.Execute(null, target);
                    break;
                case "EnterLineBreak":
                    System.Windows.Documents.EditingCommands.EnterLineBreak.Execute(null, target);
                    break;
                case "EnterParagraphBreak":
                    System.Windows.Documents.EditingCommands.EnterParagraphBreak.Execute(null, target);
                    break;
                case "TabBackward":
                    System.Windows.Documents.EditingCommands.TabBackward.Execute(null, target);
                    break;
                case "TabForward":
                    System.Windows.Documents.EditingCommands.TabForward.Execute(null, target);
                    break;

                //Paragraph Formatting
                case "IncreaseIndentation":
                    System.Windows.Documents.EditingCommands.IncreaseIndentation.Execute(null, target);
                    break;
                case "DecreaseIndentation":
                    System.Windows.Documents.EditingCommands.DecreaseIndentation.Execute(null, target);
                    break;
                case "ToggleBullets":
                    System.Windows.Documents.EditingCommands.ToggleBullets.Execute(null, target);
                    break;
                case "ToggleNumbering":
                    System.Windows.Documents.EditingCommands.ToggleNumbering.Execute(null, target);
                    break;

                //Formatting
                case "IncreaseFontSize":
                    System.Windows.Documents.EditingCommands.IncreaseFontSize.Execute(null, target);
                    break;
                case "DecreaseFontSize":
                    System.Windows.Documents.EditingCommands.DecreaseFontSize.Execute(null, target);
                    break;
                case "ToggleBold":
                    System.Windows.Documents.EditingCommands.ToggleBold.Execute(null, target);
                    break;
                case "ToggleItalic":
                    System.Windows.Documents.EditingCommands.ToggleItalic.Execute(null, target);
                    break;
                case "ToggleSubscript":
                    System.Windows.Documents.EditingCommands.ToggleSubscript.Execute(null, target);
                    break;
                case "ToggleSuperscript":
                    System.Windows.Documents.EditingCommands.ToggleSuperscript.Execute(null, target);
                    break;
                case "ToggleUnderline":
                    System.Windows.Documents.EditingCommands.ToggleUnderline.Execute(null, target);
                    break;


                //Selection by Line
                case "SelectUpByLine":
                    System.Windows.Documents.EditingCommands.SelectUpByLine.Execute(null, target);
                    break;
                case "SelectDownByLine":
                    System.Windows.Documents.EditingCommands.SelectDownByLine.Execute(null, target);
                    break;
                case "SelectToLineEnd":
                    System.Windows.Documents.EditingCommands.SelectToLineEnd.Execute(null, target);
                    break;
                case "SelectToLineStart":
                    System.Windows.Documents.EditingCommands.SelectToLineStart.Execute(null, target);
                    break;

                //Selection by Character
                case "SelectLeftByCharacter":
                    System.Windows.Documents.EditingCommands.SelectLeftByCharacter.Execute(null, target);
                    break;
                case "SelectRightByCharacter":
                    System.Windows.Documents.EditingCommands.SelectRightByCharacter.Execute(null, target);
                    break;

                //Caret Selection by Word
                case "SelectLeftByWord":
                    System.Windows.Documents.EditingCommands.SelectLeftByWord.Execute(null, target);
                    break;
                case "SelectRightByWord":
                    System.Windows.Documents.EditingCommands.SelectRightByWord.Execute(null, target);
                    break;

                //Selection by Paragraph
                case "SelectUpByParagraph":
                    System.Windows.Documents.EditingCommands.SelectUpByParagraph.Execute(null, target);
                    break;
                case "SelectDownByParagraph":
                    System.Windows.Documents.EditingCommands.SelectDownByParagraph.Execute(null, target);
                    break;

                //Selection by Page
                case "SelectUpByPage":
                    System.Windows.Documents.EditingCommands.SelectUpByPage.Execute(null, target);
                    break;
                case "SelectDownByPage":
                    System.Windows.Documents.EditingCommands.SelectDownByPage.Execute(null, target);
                    break;

                //Selection by Document
                case "SelectToDocumentEnd":
                    System.Windows.Documents.EditingCommands.SelectToDocumentEnd.Execute(null, target);
                    break;
                case "SelectToDocumentStart":
                    System.Windows.Documents.EditingCommands.SelectToDocumentStart.Execute(null, target);
                    break;
            }

            target.Focus();
        }
    }
}