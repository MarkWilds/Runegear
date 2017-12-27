using System;
using OpenTK;

namespace RuneGear.MapObjects.SolidMapObject
{
    /// <summary>
    /// Solid index
    /// </summary>
    public class SolidIndex : ICloneable
    {
        public int Index { get; set; }
        public Vector2 DiffuseUv { get; set; }

        public SolidIndex()
        {
            Index = 0;
            DiffuseUv = new Vector2();
        }

        public object Clone()
        {
            return new SolidIndex
            {
                Index = Index,
                DiffuseUv = DiffuseUv
            };
        }
    }
}
