using OpenTK;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.Utilities;
using RuneGear.Utilities.Extensions;

namespace RuneGear.MapObjects.Operations
{
    public class SkewTransformation : IMapObjectOperation
    {
        private Vector3 deltaStart;
        private Vector3 deltaEnd;
        private SolidGrabHandles.HitStatus hitStatus;
        private Matrix4 transformation;

        private Vector3 horizontalMaskVector;
        private Vector3 verticalMaskVector;

        public SkewTransformation(Matrix4 viewportMatrix, Vector3 start, Vector3 end, Matrix4 transformation,
            SolidGrabHandles.HitStatus hitStatus)
        {
            this.deltaStart = start;
            this.deltaEnd = end;
            this.hitStatus = hitStatus;
            this.transformation = transformation;

            // create mask vectors, transform mask vectors to viewport space
            horizontalMaskVector = MathExtensions.KeepVectorAbsolute(Vector3.UnitX.TransformL(viewportMatrix));
            verticalMaskVector = MathExtensions.KeepVectorAbsolute(Vector3.UnitY.TransformL(viewportMatrix));
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
            Vector3 skewAxis = Vector3.Zero;
            Vector3 perpendicularAxis = Vector3.Zero;
            Vector3 offsetVector = Vector3.Zero;

            // get the correct vectors
            int sign = SetVectors(mapObject, ref skewAxis, ref perpendicularAxis, ref offsetVector);

            // create the skew matrix
            float skewFactor = sign*GetSkewFactor(mapObject, skewAxis, perpendicularAxis);
            mapObject.Transformation = CreateSkewMatrix(skewAxis, skewFactor, perpendicularAxis, offsetVector);
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            foreach (MapObject brush in mapObjectGroup.MapObjectList)
            {
                brush.PerformOperation(this);
            }

            mapObjectGroup.RegenerateBounds();
        }

        private int SetVectors(RubberBand brush, ref Vector3 skewAxis, ref Vector3 perpAxis, ref Vector3 offset)
        {
            skewAxis = verticalMaskVector;
            perpAxis = horizontalMaskVector;
            offset = brush.Bounds.Max;
            int sign = 1;

            if (hitStatus == SolidGrabHandles.HitStatus.Top ||
                hitStatus == SolidGrabHandles.HitStatus.Bottom)
            {
                skewAxis = horizontalMaskVector;
                perpAxis = verticalMaskVector;
            }

            // northern and eastern skew handles use the min vector as offset
            if (hitStatus == SolidGrabHandles.HitStatus.Top ||
                hitStatus == SolidGrabHandles.HitStatus.Right)
                offset = brush.Bounds.Min;

            // invert western and southern skewing factor
            if (hitStatus == SolidGrabHandles.HitStatus.Left ||
                hitStatus == SolidGrabHandles.HitStatus.Bottom)
                sign = -1;

            return sign;
        }

        private float GetSkewFactor(RubberBand brush, Vector3 skewAxis, Vector3 perpAxis)
        {
            float delta = Vector3.Dot(deltaEnd - deltaStart, skewAxis);

            Vector3 brushDelta = brush.Bounds.Max - brush.Bounds.Min;
            float axisLength = Vector3.Dot(brushDelta, skewAxis);
            float perpAxisLength = Vector3.Dot(brushDelta, perpAxis);

            // we do multiply with the scale factor because we want the skew to be based of the skew axis
            // instead of the perpendicular axis
            float scaleFactor = axisLength/perpAxisLength;
            return delta/axisLength*scaleFactor;
        }

        private Matrix4 CreateSkewMatrix(Vector3 skewAxis, float skewFactor, Vector3 perpendicularAxis, Vector3 offset)
        {
            Vector4 horizontalMaskVector4D = new Vector4(skewAxis, 0.0f);
            Vector4 skewMaskVector = new Vector4(perpendicularAxis*skewFactor, 0.0f);
            Matrix4 skewMatrix = Matrix4.Identity;

            // create rows
            skewMatrix.Row0 = horizontalMaskVector4D;
            skewMatrix.Row1 = horizontalMaskVector4D;
            skewMatrix.Row2 = horizontalMaskVector4D;

            // fill columns
            skewMatrix.Column0 = Vector4.Multiply(skewMatrix.Column0, skewMaskVector);
            skewMatrix.Column1 = Vector4.Multiply(skewMatrix.Column1, skewMaskVector);
            skewMatrix.Column2 = Vector4.Multiply(skewMatrix.Column2, skewMaskVector);
            skewMatrix.Diagonal = Vector4.One;

            Vector3 translationVector = Vector3.Multiply(offset, perpendicularAxis);
            return Matrix4.CreateTranslation(-translationVector)*skewMatrix*Matrix4.CreateTranslation(translationVector);
        }
    }
}