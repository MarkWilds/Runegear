using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using RuneGear.Graphics.Textures;

namespace RuneGear.MapObjects.SolidMapObject
{
    /// <summary>
    /// Solid face
    /// </summary>
    public class SolidFace : ICloneable
    {
        public Vector3 Normal { get; set; }
        public Texture2D Texture { get; set; }
        public bool Selected { get; set; }
        public List<SolidIndex> Indices { get; private set; }

        public TextureMapping TextureMapping;

        public SolidFace()
        {
            Indices = new List<SolidIndex>();
            TextureMapping = new TextureMapping();
            TextureMapping.UScale = TextureMapping.VScale = (float)TextureMapping.DefaultScaleValue;
            TextureMapping.UShift = TextureMapping.VShift = (float)TextureMapping.DefaultShiftValue;
            TextureMapping.Rotation = (float) TextureMapping.DefaultRotationValue;
            TextureMapping.TextureLocked = TextureMapping.DefaultTextureLockValue;
        }

        public object Clone()
        {
            return new SolidFace
            {
                Normal = Normal,
                Texture = Texture,
                TextureMapping = TextureMapping,
                Selected =  Selected,
                Indices = Indices.Select(i => (SolidIndex)i.Clone()).ToList()
            };
        }
    }
}
