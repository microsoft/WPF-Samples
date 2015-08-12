// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EditingExaminerDemo
{
    /// <summary>
    ///     Provides help function for the document TreeView.
    /// </summary>
    internal class TreeViewhelper
    {
        /// <summary>
        ///     Create or fill up a TreeView of the document content
        /// </summary>
        /// <param name="treeView">Pass an TreeView control</param>
        /// <param name="document">Pass in an Document object</param>
        /// <returns></returns>
        public static TreeView SetupTreeView(TreeView treeView, FlowDocument document)
        {
            TreeViewItem root;
            if (treeView == null)
            {
                treeView = new TreeView {Visibility = Visibility.Visible};
            }
            if (document != null)
            {
                treeView.Items.Clear();
                root = new TreeViewItem
                {
                    Header = "Document",
                    IsExpanded = true
                };
                treeView.Items.Add(root);
                AddCollection(root, document.Blocks);
            }

            return treeView;
        }

        private static void AddCollection(TreeViewItem item, IList list)
        {
            foreach (var listItem in list)
            {
                var titem = new TreeViewItem();
                item.Items.Add(titem);
                titem.IsExpanded = true;
                titem.Header = listItem.GetType().Name;
                AddItem(titem, listItem as TextElement);
            }
        }

        private static void AddItem(TreeViewItem item, TextElement textElement)
        {
            TreeViewItem childItem;

            if (textElement is InlineUIContainer)
            {
                childItem = new TreeViewItem
                {
                    Header = ((InlineUIContainer) textElement).Child.GetType().Name,
                    IsExpanded = true
                };
                item.Items.Add(childItem);
            }
            else if (textElement is BlockUIContainer)
            {
                childItem = new TreeViewItem
                {
                    Header = ((BlockUIContainer) textElement).Child.GetType().Name,
                    IsExpanded = true
                };
                item.Items.Add(childItem);
            }
            else if (textElement is Span)
            {
                AddCollection(item, ((Span) textElement).Inlines);
            }
            else if (textElement is Paragraph)
            {
                AddCollection(item, ((Paragraph) textElement).Inlines);
            }
            else if (textElement is List)
            {
                AddCollection(item, ((List) textElement).ListItems);
            }
            else if (textElement is ListItem)
            {
                AddCollection(item, ((ListItem) textElement).Blocks);
            }
            else if (textElement is Table)
            {
                TableTreeView(item, textElement as Table);
            }
            else if (textElement is AnchoredBlock)
            {
                var floater = textElement as Floater;
                AddCollection(item, ((AnchoredBlock) textElement).Blocks);
            }

            //The element should be an inline (Run); try to display the text.
            else if (textElement is Inline)
            {
                var range = new TextRange(((Inline) textElement).ContentEnd, ((Inline) textElement).ContentStart);
                item.Header = item.Header + " - [" + range.Text + "]";
            }
        }

        private static void TableTreeView(TreeViewItem item, Table table)
        {
            TreeViewItem item1, item2;
            foreach (var rg in table.RowGroups)
            {
                foreach (var tr in rg.Rows)
                {
                    item1 = new TreeViewItem
                    {
                        IsExpanded = true,
                        Header = "TableRow"
                    };
                    item.Items.Add(item1);
                    foreach (var tc in tr.Cells)
                    {
                        item2 = new TreeViewItem {Header = "TableCell"};
                        item1.Items.Add(item2);
                        item2.IsExpanded = true;
                        AddCollection(item2, tc.Blocks);
                    }
                }
            }
        }
    }
}