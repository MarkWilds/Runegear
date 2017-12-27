using System.Drawing;
using RuneGear.General.Viewport;
using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Operations
{
    public class SolidFaceHitOperation : IMapObjectOperation
    {
        private readonly Ray ray;
        private float t;

        public SolidFace hitFace { get; private set; }

        public SolidFaceHitOperation(BaseViewport view, Point mousePosition)
        {
            ray = view.ViewportToRay(mousePosition.X, mousePosition.Y);
            t = float.MaxValue;
        }

        public void Visit(Solid solid)
        {
            float tFraction = float.MaxValue;
            SolidFace face = null;
            if (ray.IsIntersecting(solid, ref face, ref tFraction))
            {
                if (tFraction < t)
                {
                    t = tFraction;
                    hitFace = face;
                }
            }
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            foreach (MapObject brush in mapObjectGroup)
            {
                brush.PerformOperation(this);
            }
        }

        public void Visit(RubberBand mapObject)
        {

        }
    }
}
