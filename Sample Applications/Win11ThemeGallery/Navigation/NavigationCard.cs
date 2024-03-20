using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Win11ThemeGallery.Navigation
{
    public class NavigationCard
    {
        public string Name { get; set; } = "";
        public Type? PageType { get; set; } = null;

        public string Description { get; set; } = "";

        //public IconElement? Icon { get; set; } = null;

        public NavigationCard() { }

        public NavigationCard(string name, Type pageType, string description = "")
        {
            Name = name;
            PageType = pageType;
            Description = description;
        }

        //public NavigationCard(string name, Type pageType, SymbolRegular icon, string description = "")
        //{
        //    Name = name;
        //    PageType = pageType;
        //    Description = description;
        //   // Icon = newSymbolIcon { Symbol = icon };
        //}

        public override string ToString()
        {
            return Name;
        }
    }

}
