using System.Drawing;
using OpenTK;
using RuneGear.Utilities;

namespace RuneGear.MapObjects
{
    public class RubberBand : MapObject
    {
        public Matrix4 Transformation { get; set; }
        public SolidGrabHandles Handles { get; }
        public Color Color { get; set; }
        public bool ShowGrabhandles { get; set; }

        public bool HasVolume2D => Bounds.HasVolume2D;
        public bool HasVolume3D => Bounds.HasVolume3D;

        public override bool Selected
        {
            get { return false; }
            set { }
        }

        public override object Clone()
        {
            return null;
        }

        public RubberBand()
        {
            SetToZeroVolume();
            ShowGrabhandles = true;
            Handles = new SolidGrabHandles();
            Color = Color.Yellow;
        }

        public void SetToZeroVolume()
        {
            Transformation = Matrix4.Identity;
            Bounds.Reset();
            Bounds.Grow(Vector3.Zero);
        }

        public override void PerformOperation(IMapObjectOperation visitor)
        {
            visitor.Visit(this);
        }
    }
}