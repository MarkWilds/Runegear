using System;
using OpenTK;
using RuneGear.Geometry;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.Utilities;
using RuneGear.Utilities.Extensions;

namespace RuneGear.MapObjects.Operations
{
    public class TranslateOperation : IMapObjectOperation
    {
        private readonly Vector3 translationVector;
        public int GridSize { get; set; }
        public Matrix4 Transform { get; set; }

        public TranslateOperation(Vector3 translation)
        {
            translationVector = translation;
        }

        public void Visit(Solid solid)
        {
            for (int i = 0; i < solid.VertexPositions.Count; i++)
            {
                solid.VertexPositions[i] += translationVector;
            }

            TranslateBounds(solid.Bounds);

            // calculate texture coordinates
            solid.Faces.ForEach( f => solid.CalculateTextureCoordinatesForFace(f, true));
        }

        public void Visit(RubberBand mapObject)
        {
            if (!SnapAabb(mapObject.Bounds))
                TranslateBounds(mapObject.Bounds);
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            foreach (MapObject brush in mapObjectGroup.MapObjectList)
            {
                brush.PerformOperation(this);
            }

            TranslateBounds(mapObjectGroup.Bounds);
        }

        private void TranslateBounds(AABB bounds)
        {
            bounds.Min += translationVector;
            bounds.Max += translationVector;
        }
        
        public bool SnapAabb(AABB bounds)
        {
            bool snappedAabb = false;
            Matrix4 inverse = Transform.Inverted();
            Vector3 newMin = bounds.Min.TransformL(Transform);
            Vector3 newMax = bounds.Max.TransformL(Transform);
            Vector3 movement = translationVector.TransformL(Transform);

            float restMinX = (int)(Math.Abs(newMin.X) + 0.5f) % GridSize;
            float restMinY = (int)(Math.Abs(newMin.Y) + 0.5f) % GridSize;
            float restMaxX = (int)(Math.Abs(newMax.X) + 0.5f) % GridSize;
            float restMaxY = (int)(Math.Abs(newMax.Y) + 0.5f) % GridSize;                      

            // moving left
            if (movement.X < -GeneralUtility.Epsilon 
                && restMinX > GeneralUtility.Epsilon)
            {                
                SnapTranslation(ref newMin.X, ref newMax.X);
                snappedAabb = true;              
            } // moving right
            else if (movement.X > GeneralUtility.Epsilon 
                && restMaxX > GeneralUtility.Epsilon)
            {                
                SnapTranslation(ref newMax.X, ref newMin.X);
                snappedAabb = true;                
            }

            // moving down
            if (movement.Y < -GeneralUtility.Epsilon 
                && restMinY > GeneralUtility.Epsilon)
            {                
                SnapTranslation(ref newMin.Y, ref newMax.Y);
                snappedAabb = true;                
            } // moving up
            else if (movement.Y > GeneralUtility.Epsilon 
                && restMaxY > GeneralUtility.Epsilon)
            {                
                SnapTranslation(ref newMax.Y, ref newMin.Y);
                snappedAabb = true;                
            }

            if (snappedAabb)
            {
                bounds.Min = newMin.TransformL(inverse);
                bounds.Max = newMax.TransformL(inverse);
            }

            return snappedAabb;
        }

        private void SnapTranslation(ref float first, ref float second)
        {
            float newFirst = GeneralUtility.SnapNumber(first, GridSize);
            second = second + newFirst - first;
            first = newFirst;
        }
    }
}