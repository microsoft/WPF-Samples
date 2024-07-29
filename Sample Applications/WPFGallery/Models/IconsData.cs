using System.Text.Json;
using System.Text.Json.Serialization;

namespace WPFGallery.Models
{
    /// <summary>
    /// IconData class for icons in icon page
    /// </summary>
    public class IconData
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public string Character => char.ConvertFromUtf32(Convert.ToInt32(Code, 16));
        public string CodeGlyph => "\\x" + Code;
        public string TextGlyph => "&#x" + Code + ";";
    }
}
