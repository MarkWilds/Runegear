using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using RuneGear.General.Viewport;
using RuneGear.Geometry;
using RuneGear.MapObjects.SolidMapObject;

namespace RuneGear.MapObjects.Operations
{
    public class HitOperation : IMapObjectOperation
    {
        private readonly BaseViewport viewport;
        private readonly Ray ray;
        private bool hasIntersected;
        private float t;

        public MapObject HitMapObject { get; private set; }

        public HitOperation(BaseViewport view, Point mousePosition)
        {
            viewport = view;
            ray = view.ViewportToRay(mousePosition.X, mousePosition.Y);
            hasIntersected = false;
            t = float.MaxValue;
            HitMapObject = null;
        }

        public void Visit(Solid solid)
        {
            if (ray.Direction.Length < 1e-6)
                return;

            float tFraction = 0.0f;

            CheckIntersection(solid, ref tFraction);
            if (hasIntersected && tFraction < t)
            {
                t = tFraction;
                HitMapObject = solid;
            }
        }

        private void CheckIntersection(Solid solid, ref float tFraction)
        {
            SolidFace hitFace = null;
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE)
                hasIntersected = CheckBrushCenterHit(solid, ref tFraction) ||
                                 CheckBrushEdgesHit(solid, ref tFraction);
            else
                hasIntersected = ray.IsIntersecting(solid, ref hitFace, ref tFraction);
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            foreach (MapObject mapObject in mapObjectGroup)
            {
                mapObject.PerformOperation(this);

                // as soon as we have found a brush hit we can say that this group brush is hit!
                if (hasIntersected)
                {
                    HitMapObject = mapObjectGroup;
                    break;
                }
            }
        }

        public void Visit(RubberBand mapObject)
        {
        }

        /// <summary>
        /// Checks if we hit any of the brush's edges
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="tRef"></param>
        /// <returns></returns>
        private bool CheckBrushEdgesHit(Solid brush, ref float tRef)
        {
            List<Vector3> vertexPositions = brush.VertexPositions;
            foreach (SolidFace face in brush.Faces)
            {
                int vertexCount = face.Indices.Count;
                for (int i = 0; i < vertexCount; i++)
                {
                    Vector3 start = vertexPositions[face.Indices[i].Index];
                    Vector3 end = vertexPositions[face.Indices[(i + 1)%vertexCount].Index];
                    if (ray.IsCloseEnough(new Line(start, end), viewport.Zoom, ref tRef))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// checks if we hit the center of the brush
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="tRef"></param>
        /// <returns></returns>
        private bool CheckBrushCenterHit(MapObject brush, ref float tRef)
        {
            float radius = 5.0f*viewport.Zoom;
            return ray.IsIntersecting(brush.Bounds.Center, radius, ref tRef);
        }
    }
}