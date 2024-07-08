﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.Navigation
{
    /// <summary>
    /// NavigationItem class
    /// </summary>
    public class NavigationItem
    {
        public string Name { get; set; } = "";
        public Type? PageType { get; set; } = null;

        public ICollection<NavigationItem> Children { get; set; } = new ObservableCollection<NavigationItem>();

        public string Icon { get; set; } = "";

        public NavigationItem() { }

        public NavigationItem(string name, Type pageType)
        {
            Name = name;
            PageType = pageType;
        }

        public NavigationItem(string name, Type pageType, ObservableCollection<NavigationItem> navItems)
        {
            Name = name;
            PageType = pageType;
            Children = navItems;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
