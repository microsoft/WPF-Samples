using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFGallery.Navigation
{
    /// <summary>
    /// Represents a navigation card that can be used to navigate to a page.
    /// </summary>
    public class NavigationCard
    {
        public string Name { get; set; } = "";
        public Type? PageType { get; set; } = null;

        public string Description { get; set; } = "";

        //public IconElement? Icon { get; set; } = null;

        public Object? Icon { get; set; } = null;

        public NavigationCard() { }

        public NavigationCard(string name, Type pageType, string description = "")
        {
            Name = name;
            PageType = pageType;
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
