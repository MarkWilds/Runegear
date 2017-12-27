using System;
using OpenTK;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.Utilities;
using RuneGear.Utilities.Extensions;

namespace RuneGear.MapObjects.Operations
{
    public class RotateTransformation : IMapObjectOperation
    {
        private Vector3 deltaStart;
        private Vector3 deltaEnd;
        private Matrix4 viewportMatrix;
        private Matrix4 transformation;
        private const int mSnapValue = 5;

        public RotateTransformation(Matrix4 viewportMatrix, Vector3 start, Vector3 end, Matrix4 transformation)
        {
            this.viewportMatrix = viewportMatrix;
            this.deltaStart = start;
            this.deltaEnd = end;
            this.transformation = transformation;
        }

        public void Visit(Solid solid)
        {
            for (int i = 0; i < solid.VertexPositions.Count; i++)
            {
                solid.VertexPositions[i] = solid.VertexPositions[i].TransformL(transformation);
            }

            solid.RegenerateBounds();
            solid.CalculateNormals();
            solid.Faces.ForEach( f => solid.CalculateTextureCoordinatesForFace(f, true));
        }

        public void Visit(RubberBand mapObject)
        {
            if ((deltaStart - deltaEnd).Length < GeneralUtility.Epsilon)
                return;

            Vector3 planeNormal = Vector3.UnitZ.TransformL(viewportMatrix);
            Vector3 axisMask = KeepVectorAbsolute((Vector3.UnitX + Vector3.UnitY).TransformL(viewportMatrix));
            Vector3 axisRotationMask = KeepVectorAbsolute(planeNormal);

            // calculate angle
            Vector3 first = Vector3.Multiply(deltaStart - mapObject.Bounds.Center, axisMask);
            Vector3 second = Vector3.Multiply(deltaEnd - mapObject.Bounds.Center, axisMask);

            float angle = (float) first.SignedAngleTo(second, planeNormal);
            Vector3 euler = axisRotationMask * angle;
            euler = KeepEulerSnapped(euler);

            Matrix4 rotationMatrix = Matrix4.CreateRotationX(euler.X)*Matrix4.CreateRotationY(euler.Y)*
                                     Matrix4.CreateRotationZ(euler.Z);
            mapObject.Transformation = Matrix4.CreateTranslation(-mapObject.Bounds.Center)*rotationMatrix*
                                   Matrix4.CreateTranslation(mapObject.Bounds.Center);
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            foreach (MapObject brush in mapObjectGroup.MapObjectList)
            {
                brush.PerformOperation(this);
            }

            mapObjectGroup.RegenerateBounds();
        }

        private Vector3 KeepEulerSnapped(Vector3 euler)
        {
            float snapValue = (float) (mSnapValue*Math.PI/180.0f);
            for (int i = 0; i < 3; i++)
            {
                euler[i] = (int) (euler[i]/snapValue + 0.5f)*snapValue;
            }

            return euler;
        }

        private Vector3 KeepVectorAbsolute(Vector3 vector)
        {
            vector.X = Math.Abs(vector.X);
            vector.Y = Math.Abs(vector.Y);
            vector.Z = Math.Abs(vector.Z);

            return vector;
        }
    }
}