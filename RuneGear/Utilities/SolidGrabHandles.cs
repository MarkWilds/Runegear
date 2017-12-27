using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using RuneGear.Geometry;
using RuneGear.MapObjects;

namespace RuneGear.Utilities
{
    public class SolidGrabHandles
    {
        private const int HANDLE_SIZE = 6;
        private const int HANDLE_OFFSET = 2;
        private const int BRUSH_MANIPULATION_HANDLE_COUNT = 8;

        public struct SolidGrabHandle
        {
            public Vector3 BottomLeft;
            public Vector3 BottomRight;
            public Vector3 TopRight;
            public Vector3 TopLeft;

            public void Set(Vector3 bl, Vector3 br, Vector3 tr, Vector3 tl)
            {
                BottomLeft = bl;
                BottomRight = br;
                TopRight = tr;
                TopLeft = tl;
            }
        }

        public enum HandleMode
        {
            Resize,
            Rotate,
            Skew
        }

        public enum HitStatus
        {
            None = 0,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Top,
            Bottom,
            Left,
            Right
        }

        public HandleMode Mode
        {
            get; set;
        }

        public HitStatus LastHitStatus
        {
            private set;
            get;
        }

        private List<SolidGrabHandle> mHandles;

        /// <summary>
        /// Constructor
        /// </summary>
        public SolidGrabHandles()
        {
            Mode = HandleMode.Resize;
            mHandles = new List<SolidGrabHandle>();
        }

        public void NextMode()
        {
            Mode = (HandleMode) ( ((int)Mode + 1) % 3);
        }

        public void ResetMode()
        {
            Mode = HandleMode.Resize;
        }

        public void CreateHandles(MapObject brush, Matrix4 viewMatrix, float zoomFactor = 1.0f)
        {
            AABB brushBounds = brush.Bounds;
            Vector3 hBase = viewMatrix.Row0.Xyz;
            Vector3 vBase = viewMatrix.Row1.Xyz;
            float positionOffset = (HANDLE_SIZE / 2.0f + HANDLE_OFFSET) * zoomFactor;
            float halfHandleSize = HANDLE_SIZE / 2 * zoomFactor;
            mHandles.Clear();

            Vector3 center = brushBounds.Center;
            Vector3 diagonal = (brushBounds.Max - brushBounds.Min) * 0.5f;

            // we add positionOffset in each direction to offset the position for the handles
            Vector3 hOffset = Vector3.Multiply(hBase, diagonal) + hBase * positionOffset;
            Vector3 vOffset = Vector3.Multiply(vBase, diagonal) + vBase * positionOffset;

            List<Vector3> points = new List<Vector3>()
            {
                center - hOffset + vOffset, // top left handle
                center + hOffset + vOffset, // top right handle
                center - hOffset - vOffset, // bottom left handle
                center + hOffset - vOffset, // bottom right handle
                center + vOffset, // Top
                center - vOffset, // Bottom
                center - hOffset, // Left
                center + hOffset // Right
            };

            foreach (Vector3 position in points)
            {
                Vector3 bottomLeft = position + (-hBase - vBase) * halfHandleSize;
                Vector3 bottomRight = position + (hBase - vBase) * halfHandleSize;
                Vector3 topRight = position + (hBase + vBase) * halfHandleSize;
                Vector3 topLeft = position + (-hBase + vBase) * halfHandleSize;

                SolidGrabHandle grabHandle = new SolidGrabHandle();
                grabHandle.Set(bottomLeft, bottomRight, topRight, topLeft);
                mHandles.Add(grabHandle);
            }
        }

        /// <summary>
        /// Returns integer indicating which handle was touched.
        /// For vertex mode it returns the indices for the touched grabhandle for the vertex
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        public int CheckHit(Ray ray)
        {
            if (mHandles.Count <= 0)
                return (int)(LastHitStatus = HitStatus.None);

            // default HandleMode.Resize
            int startIndex = 0;
            int endIndex = BRUSH_MANIPULATION_HANDLE_COUNT;

            if (Mode == HandleMode.Rotate)
                endIndex = 4;
            else if (Mode == HandleMode.Skew)
                startIndex = 4;

            for (int i = startIndex; i < endIndex; i++)
            {
                SolidGrabHandle handle = mHandles[i];
                if (ray.IsIntersecting(handle.BottomLeft, handle.BottomRight, handle.TopRight, handle.TopLeft))
                    return (int)(LastHitStatus = (HitStatus)i + 1);
            }

            return (int)(LastHitStatus = HitStatus.None);
        }

        public void Render(Graphics.Graphics graphics)
        {
            switch (Mode)
            {
                case HandleMode.Resize:
                    for (int i = 0; i < BRUSH_MANIPULATION_HANDLE_COUNT; i++)
                    {
                        SolidGrabHandle handle = mHandles[i];
                        graphics.DrawSolidRectangle(handle.BottomLeft, handle.BottomRight, 
                            handle.TopRight, handle.TopLeft, Color.White);
                    }
                    break;
                case HandleMode.Rotate:
                    for (int i = 0; i < 4; i++)
                    {
                        SolidGrabHandle handle = mHandles[i];
                        graphics.DrawSolidCircle(handle.BottomLeft, handle.BottomRight,
                            handle.TopRight, handle.TopLeft, Color.White);
                    }
                    break;
                case HandleMode.Skew:
                    for (int i = 4; i < 8; i++)
                    {
                        SolidGrabHandle handle = mHandles[i];
                        graphics.DrawSolidRectangle(handle.BottomLeft, handle.BottomRight,
                            handle.TopRight, handle.TopLeft, Color.White);
                    }
                    break;
            }
        }
    }
}
