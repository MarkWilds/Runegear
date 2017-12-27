using System.Drawing;

namespace RuneGear.General
{
    public struct TextureItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string FilePath { get; set; }
        public Image Image { get; set; }
    }
}
