namespace WPFGallery.Models
{
    /// <summary>
    /// IconData class for icons in icon page
    /// </summary>
    public class IconData
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> Tags { get; set; } = [];

        public string Character => char.ConvertFromUtf32(Convert.ToInt32(Code, 16));
        public string CodeGlyph => "\\x" + Code;
        public string TextGlyph => "&#x" + Code + ";";
    }
}
