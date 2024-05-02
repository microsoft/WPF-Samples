using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WPFGallery.Models
{
    public class IconData
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public string Character => char.ConvertFromUtf32(Convert.ToInt32(Code, 16));
        public string CodeGlyph => "\\x" + Code;
        public string TextGlyph => "&#x" + Code + ";";
    }
}
