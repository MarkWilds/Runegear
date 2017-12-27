using OpenTK;
using RuneGear.Geometry;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.Utilities;
using RuneGear.Utilities.Extensions;

namespace RuneGear.MapObjects.Operations
{
    public class ResizeTransformation : IMapObjectOperation
    {
        private Vector3 deltaStart;
        private Vector3 deltaEnd;
        private Matrix4 transform;
        private SolidGrabHandles.HitStatus hitStatus;
        private int gridSize;

        private Vector3 horizontalMaskVector;
        private Vector3 verticalMaskVector;
        private Vector3 depthMaskVector;

        public AABB rootBounds { get; set; }

        public ResizeTransformation(Matrix4 transform, Vector3 start, Vector3 end, SolidGrabHandles.HitStatus hitStatus,
            int gridSize)
        {
            rootBounds = null;
            deltaStart = start;
            deltaEnd = end;
            this.hitStatus = hitStatus;
            this.gridSize = gridSize;
            this.transform = transform;

            // create mask vectors, transform mask vectors to viewport space
            horizontalMaskVector = MathExtensions.KeepVectorAbsolute(Vector3.UnitX.TransformL(transform));
            verticalMaskVector = MathExtensions.KeepVectorAbsolute(Vector3.UnitY.TransformL(transform));
            depthMaskVector = Vector3.UnitZ.TransformL(transform);
        }

        public void Visit(Solid solid)
        {
            // for solidbrush deltaEnd and deltaStart are the bound vectors (max - min)
            Vector3 scaleVector = Vector3.Divide(deltaEnd, deltaStart);
            for (int i = 0; i < solid.VertexPositions.Count; i++)
            {
                solid.VertexPositions[i] = ResizeVertex(rootBounds ?? solid.Bounds, solid.VertexPositions[i], scaleVector);
            }

            solid.RegenerateBounds();
            solid.CalculateNormals();
            solid.Faces.ForEach( f => solid.CalculateTextureCoordinatesForFace(f, true));
        }

        public void Visit(RubberBand mapObject)
        {
            AABB bounds = mapObject.Bounds;

            // resize bounding box
            switch (hitStatus)
            {
                case SolidGrabHandles.HitStatus.Top:
                    bounds.Max = ResizeRubberbandBounds(bounds.Max, verticalMaskVector);
                    break;
                case SolidGrabHandles.HitStatus.TopRight:
                    bounds.Max = ResizeRubberbandBounds(bounds.Max, horizontalMaskVector);
                    bounds.Max = ResizeRubberbandBounds(bounds.Max, verticalMaskVector);
                    break;
                case SolidGrabHandles.HitStatus.Right:
                    bounds.Max = ResizeRubberbandBounds(bounds.Max, horizontalMaskVector);
                    break;

                case SolidGrabHandles.HitStatus.Left:
                    bounds.Min = ResizeRubberbandBounds(bounds.Min, horizontalMaskVector);
                    KeepVolumeInMinArea(bounds);
                    break;
                case SolidGrabHandles.HitStatus.BottomLeft:
                    bounds.Min = ResizeRubberbandBounds(bounds.Min, horizontalMaskVector);
                    bounds.Min = ResizeRubberbandBounds(bounds.Min, verticalMaskVector);
                    break;
                case SolidGrabHandles.HitStatus.Bottom:
                    bounds.Min = ResizeRubberbandBounds(bounds.Min, verticalMaskVector);
                    break;

                case SolidGrabHandles.HitStatus.TopLeft:
                    bounds.Min = ResizeRubberbandBounds(bounds.Min, horizontalMaskVector);
                    bounds.Max = ResizeRubberbandBounds(bounds.Max, verticalMaskVector);
                    break;
                case SolidGrabHandles.HitStatus.BottomRight:
                    bounds.Min = ResizeRubberbandBounds(bounds.Min, verticalMaskVector);
                    bounds.Max = ResizeRubberbandBounds(bounds.Max, horizontalMaskVector);

                    break;
            }

            //do not allow negative resizing and keep volume
            switch (hitStatus)
            {
                case SolidGrabHandles.HitStatus.Top:
                case SolidGrabHandles.HitStatus.TopRight:
                case SolidGrabHandles.HitStatus.Right:
                    KeepVolumeInMaxArea(bounds);
                    break;

                case SolidGrabHandles.HitStatus.Left:
                case SolidGrabHandles.HitStatus.BottomLeft:
                case SolidGrabHandles.HitStatus.Bottom:
                    KeepVolumeInMinArea(bounds);
                    break;

                case SolidGrabHandles.HitStatus.TopLeft:
                    KeepVolumeTopLeft(bounds);
                    break;
                case SolidGrabHandles.HitStatus.BottomRight:
                    KeepVolumeBottomRight(bounds);
                    break;
            }
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            // we only set the bounds for the top groupbrush
            // this way a child groupbrush wont change the rootBounds again
            if (rootBounds == null)
                rootBounds = mapObjectGroup.Bounds;

            foreach (MapObject brush in mapObjectGroup.MapObjectList)
            {
                brush.PerformOperation(this);
            }

            mapObjectGroup.RegenerateBounds();
        }

        private Vector3 ResizeVertex(AABB brushBounds, Vector3 inputVector, Vector3 scaleVector)
        {
            Vector3 originVector = Vector3.Zero;
            Vector3 transformVector = horizontalMaskVector - verticalMaskVector + depthMaskVector;

            switch (hitStatus)
            {
                case SolidGrabHandles.HitStatus.TopRight:
                case SolidGrabHandles.HitStatus.Right:
                case SolidGrabHandles.HitStatus.Top:
                    originVector = brushBounds.Min;
                    break;
                case SolidGrabHandles.HitStatus.BottomLeft:
                case SolidGrabHandles.HitStatus.Left:
                case SolidGrabHandles.HitStatus.Bottom:
                    originVector = brushBounds.Max;
                    break;
                case SolidGrabHandles.HitStatus.TopLeft:
                    originVector = Vector3.Multiply(brushBounds.Max - brushBounds.Center, transformVector) +
                                   brushBounds.Center;
                    break;
                case SolidGrabHandles.HitStatus.BottomRight:
                    originVector = Vector3.Multiply(brushBounds.Min - brushBounds.Center, transformVector) +
                                   brushBounds.Center;
                    break;
            }

            return Vector3.Multiply(inputVector - originVector, scaleVector) + originVector;
        }

        public bool SnapAabb(AABB bounds)
        {
            bool snappedAabb = false;
            Matrix4 inverse = transform.Inverted();
            Vector3 newMin = bounds.Min.TransformL(transform);
            Vector3 newMax = bounds.Max.TransformL(transform);

            //float restMinX = Math.Abs(newMin.X % GridSize);
            //float restMinY = Math.Abs(newMin.Y % GridSize);
            //float restMaxX = Math.Abs(newMax.X % GridSize);
            //float restMaxY = Math.Abs(newMax.Y % GridSize);

            //Vector3 movement = Vector3.Transform(translationVector, transform);

            //// horizontal
            //if (movement.X < -GeneralUtil.Epsilon && Math.Abs(restMinX) > GeneralUtil.Epsilon)
            //{
            //    SnapTranslation(ref newMin.X, ref newMax.X);
            //    snappedAabb = true;
            //}
            //else if (movement.X > GeneralUtil.Epsilon && Math.Abs(restMaxX) > GeneralUtil.Epsilon)
            //{
            //    SnapTranslation(ref newMax.X, ref newMin.X);
            //    snappedAabb = true;
            //}

            //// vertical
            //if (movement.Y < -GeneralUtil.Epsilon && Math.Abs(restMinY) > GeneralUtil.Epsilon)
            //{
            //    SnapTranslation(ref newMin.Y, ref newMax.Y);
            //    snappedAabb = true;
            //}
            //else if (movement.Y > GeneralUtil.Epsilon && Math.Abs(restMaxY) > GeneralUtil.Epsilon)
            //{
            //    SnapTranslation(ref newMax.Y, ref newMin.Y);
            //    snappedAabb = true;
            //}

            bounds.Min = newMin.TransformL(inverse);
            bounds.Max = newMax.TransformL(inverse);

            return snappedAabb;
        }

        private Vector3 ResizeRubberbandBounds(Vector3 outputVector, Vector3 mask)
        {
            Vector3 maskedVector = Vector3.Multiply(outputVector, mask);
            Vector3 snappedMaskedVector = maskedVector;
            GeneralUtility.SnapToGrid(ref snappedMaskedVector, gridSize);

            Vector3 axisDelta = Vector3.Multiply(deltaEnd - deltaStart, mask);

            // if something changed then the vector has snapped!
            if ((snappedMaskedVector - maskedVector).Length > GeneralUtility.Epsilon)
                outputVector = outputVector - maskedVector + snappedMaskedVector;
            else
                outputVector += axisDelta;

            return outputVector;
        }

        private void KeepVolumeInMaxArea(AABB bounds)
        {
            Vector3 max = bounds.Max;
            Vector3 min = bounds.Min;

            if (bounds.Max.X <= bounds.Min.X)
                max.X = bounds.Min.X + gridSize;

            if (bounds.Max.Y <= bounds.Min.Y)
                max.Y = bounds.Min.Y + gridSize;

            if (bounds.Max.Z <= bounds.Min.Z)
                max.Z = bounds.Min.Z + gridSize;

            bounds.Max = max;
            bounds.Min = min;
        }

        private void KeepVolumeInMinArea(AABB bounds)
        {
            Vector3 max = bounds.Max;
            Vector3 min = bounds.Min;

            if (bounds.Max.X <= bounds.Min.X)
                min.X = bounds.Max.X - gridSize;

            if (bounds.Max.Y <= bounds.Min.Y)
                min.Y = bounds.Max.Y - gridSize;

            if (bounds.Max.Z <= bounds.Min.Z)
                min.Z = bounds.Max.Z - gridSize;

            bounds.Max = max;
            bounds.Min = min;
        }

        private void KeepVolumeTopLeft(AABB bounds)
        {
            Matrix4 invertedViewportMatrix = transform.Inverted();

            Vector3 max = bounds.Max.TransformL(transform);
            Vector3 min = bounds.Min.TransformL(transform);

            if (max.X <= min.X)
                min.X = max.X - gridSize;

            if (max.Y <= min.Y)
                max.Y = min.Y + gridSize;

            bounds.Max = max.TransformL(invertedViewportMatrix);
            bounds.Min = min.TransformL(invertedViewportMatrix);
        }

        private void KeepVolumeBottomRight(AABB bounds)
        {
            Matrix4 invertedViewportMatrix = transform.Inverted();

            Vector3 max = bounds.Max.TransformL(transform);
            Vector3 min = bounds.Min.TransformL(transform);

            if (max.X <= min.X)
                max.X = min.X + gridSize;

            if (max.Y <= min.Y)
                min.Y = max.Y - gridSize;

            bounds.Max = max.TransformL(invertedViewportMatrix);
            bounds.Min = min.TransformL(invertedViewportMatrix);
        }
    }
}