// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace OverridingLogicalTree
{
    public class SingletonPanel : StackPanel
    {
        //private UIElementCollection _children; 
        private FrameworkElement _child;

        public FrameworkElement SingleChild
        {
            get { return _child; }
            set
            {
                if (value == null)
                {
                    RemoveLogicalChild(_child);
                }
                else
                {
                    if (_child == null)
                    {
                        _child = value;
                    }
                    else
                    {
                        // raise an exception?
                        MessageBox.Show("Needs to be a single element");
                    }
                }
            }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                // cheat, make a list with one member and return the enumerator
                var list = new ArrayList {_child};
                return list.GetEnumerator();
            }
        }

        public void SetSingleChild(object child)
        {
            AddLogicalChild(child);
        }

        public new void AddLogicalChild(object child)
        {
            _child = (FrameworkElement) child;
            if (Children.Count == 1)
            {
                RemoveLogicalChild(Children[0]);
                Children.Add((UIElement) child);
            }
            else
            {
                Children.Add((UIElement) child);
            }
        }

        public new void RemoveLogicalChild(object child)

        {
            _child = null;
            Children.Clear();
        }
    }
}