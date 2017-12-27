using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RuneGear.General.Camera;
using RuneGear.Geometry;
using RuneGear.Tools;
using RuneGear.Utilities.Extensions;

namespace RuneGear.General.Viewport
{
    public class OrthoViewport : BaseViewport
    {
        private readonly OrthoCamera mCamera;

        private int mGridSizeInPixels;
        private const int MaxGridSize = 512;
        private Point mousePrevPos;

        public override BaseViewportCamera Camera => mCamera;
        public override int GridSize => mGridSizeInPixels;
        public override float Zoom => mCamera.Zoom;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Orthographic viewport type</param>
        public OrthoViewport(ViewportTypes type)
        {           
            mousePrevPos = new Point();
            mCamera = new OrthoCamera();
            MViewportTypes = type;
        }

        /// <summary>
        /// Resets the viewport camera to it's original state
        /// </summary>
        public override void ResetViewport()
        {
            RenderMode = RenderModes.WIREFRAME;
            mCamera.Reset();
            switch (MViewportTypes)
            {
                case ViewportTypes.TOP:
                    mCamera.Rotate(1.0f, 0, 0, 90.0f);
                    break;
                case ViewportTypes.SIDE:
                    mCamera.Rotate(0, 1.0f, 0, -90.0f);
                    break;
            }

            mGridSizeInPixels = 8;
            mCamera.Zoom = 1.0f;
        }

        /// <summary>
        /// Get the grid plane for this viewport
        /// Since were using row matrices we can take the third row as the plane normal
        /// As this is the normal in the direction we will always be looking at for every ortho viewport
        /// </summary>
        /// <returns></returns>
        private Plane GetGridPlane()
        {
            Vector3 planeNormal = mCamera.GetWorldMatrix().Row2.Xyz;
            return new Plane(planeNormal, 0.0f);
        }

        /// <summary>
        /// Convert viewport coordinates to grid coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="gridPlane"></param>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        private bool ViewportToGridPlane(int x, int y, Plane gridPlane, ref Vector3 worldPos)
        {
            Ray ray = ViewportToRay(x, y);

            float rayDirDistToPlane = Vector3.Dot(ray.Direction, gridPlane.Normal);
            if (Math.Abs(rayDirDistToPlane) < 1e-5f)
                return false;

            float rayPosDistToPlane = Vector3.Dot(ray.Origin, gridPlane.Normal) + gridPlane.Distance;
            float distance = rayPosDistToPlane / -rayDirDistToPlane;

            worldPos = ray.Origin + (ray.Direction * distance);

            return true;
        }

        public override Vector3 WorldToViewport(Vector3 point)
        {
            //Matrix4 projMatrix = Camera.GetProjMatrix();
            Vector3 newPoint = point.TransformL(Camera.GetViewMatrix());

            //newPoint.X = projMatrix.M11 * (2.0f / newPoint.X * Width + 1);
            //newPoint.Y = projMatrix.M22 * -(2.0f / newPoint.Y * Height + 1);
            newPoint.X = newPoint.X + Width / 2.0f;
            newPoint.Y = Height / 2.0f - newPoint.Y;

            return newPoint;
        }

        /// <summary>
        /// Convert the viewport coordinates to a ray
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override Ray ViewportToRay(int x, int y)
        {
            Matrix4 projMat = mCamera.GetProjMatrix();
            Matrix4 cameraWorldMatrix = mCamera.GetWorldMatrix();
            Vector3 viewportPoint;

            // convert to view space
            viewportPoint.X = ((2.0f * x / mWidth - 1)) / projMat.M11;
            viewportPoint.Y = -((2.0f * y / mHeight - 1)) / projMat.M22;
            viewportPoint.Z = 1.0f / projMat.M33;

            // convert to grid space
            Vector3 rayPos = viewportPoint.TransformL(cameraWorldMatrix);
            Vector3 rayDir = new Vector3(cameraWorldMatrix.Row2.X, cameraWorldMatrix.Row2.Y, cameraWorldMatrix.Row2.Z);

            // normalize ray direction
            rayDir.Normalize();
            return new Ray(rayPos, rayDir);
        }

        /// <summary>
        /// Renders the grid on the viewport
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawGrid(Graphics.Graphics graphics)
        {
            Matrix4 translationViewMatrix = mCamera.GetViewMatrix().ClearRotation();
            Matrix4 viewProjMatrix = translationViewMatrix * mCamera.GetProjMatrix();

            // get info from the settings
            Color minorGridColor = controller.EditorSettings.GetColor("minorGridColor");
            Color majorGridColor = controller.EditorSettings.GetColor("majorGridColor");
            Color originGridColor = controller.EditorSettings.GetColor("originGridColor");
            int hideLinesLower = controller.EditorSettings.HideLinesLower;
            int majorLineEvery = controller.EditorSettings.MajorLineEvery;

            int gridDim = 1 << 14; // 16384 dimensions

            float[] bounds = {float.MaxValue, float.MaxValue, -float.MaxValue, -float.MaxValue};
            Vector3[] worldExtends = new Vector3[4];

            // hide lines if grid is smaller than specified number
            int gridSize = mGridSizeInPixels;
            while ((gridSize/mCamera.Zoom) < hideLinesLower)
                gridSize *= majorLineEvery;

            // get the 4 world space corner points
            if (!ViewportToGridPlane(0, 0, GetGridPlane(), ref worldExtends[0]))
                return;
            if (!ViewportToGridPlane(mWidth, 0, GetGridPlane(), ref worldExtends[1]))
                return;
            if (!ViewportToGridPlane(mWidth, mHeight, GetGridPlane(), ref worldExtends[2]))
                return;
            if (!ViewportToGridPlane(0, mHeight, GetGridPlane(), ref worldExtends[3]))
                return;

            // get bounding box
            for (int i = 0; i < 4; i++)
            {
                // convert to default space (x,y)
                Matrix4 fromGridMatrix = mCamera.GetViewMatrix().ClearTranslation();
                worldExtends[i] = worldExtends[i].TransformL(fromGridMatrix);

                if (worldExtends[i].X < bounds[0]) bounds[0] = worldExtends[i].X;
                if (worldExtends[i].Y < bounds[1]) bounds[1] = worldExtends[i].Y;
                if (worldExtends[i].X > bounds[2]) bounds[2] = worldExtends[i].X;
                if (worldExtends[i].Y > bounds[3]) bounds[3] = worldExtends[i].Y;
            }

            float gridWidth = bounds[2] - bounds[0]; // max.x - min.x
            float gridHeight = bounds[3] - bounds[1]; // max.y - min.y

            int gridCountX = (int) gridWidth/gridSize + 4;
            int gridCountY = (int) gridHeight/gridSize + 4;
            int gridStartX = (int) bounds[0]/gridSize - 1;
            int gridStartY = (int) bounds[1]/gridSize - 1;

            // Set line start and line end in world space coordinates
            float lineStartX = gridStartX*gridSize;
            float lineStartY = gridStartY*gridSize;
            float lineEndX = (gridStartX + (gridCountX - 1))*gridSize;
            float lineEndY = (gridStartY + (gridCountY - 1))*gridSize;

            // keep line start and line end inside the grid dimensions
            lineStartX = (lineStartX < -gridDim) ? -gridDim : lineStartX;
            lineStartY = (lineStartY < -gridDim) ? -gridDim : lineStartY;
            lineEndX = (lineEndX > gridDim) ? gridDim : lineEndX;
            lineEndY = (lineEndY > gridDim) ? gridDim : lineEndY;

            // make sure we have anything to render
            if (gridCountX + gridCountY <= 0)
                return;

            // start drawing the grid
            graphics.BeginDraw(viewProjMatrix);

            // the grid lines are ordered as minor, major, origin
            for (int lineType = 0; lineType < 3; lineType++)
            {
                Color lineColor = minorGridColor;
                if (lineType == 1)
                    lineColor = majorGridColor;
                else if (lineType == 2)
                    lineColor = originGridColor;

                // draw horizontal lines first
                for (int i = gridStartY; i < gridStartY + gridCountY; ++i)
                {
                    // skip lines that are out of bound
                    if (i*gridSize < -gridDim || i*gridSize > gridDim)
                        continue;

                    // skip any line that don't match the line type we're adding
                    if (lineType == 0 && (i == 0 || (i%majorLineEvery) == 0))
                        continue;
                    else if (lineType == 1 && (i == 0 || (i%majorLineEvery) != 0))
                        continue;
                    else if (lineType == 2 && i != 0)
                        continue;

                    // draw the line
                    graphics.DrawLine(new Vector3(lineStartX, (float) i*gridSize, 0.0f),
                        new Vector3(lineEndX, (float) i*gridSize, 0.0f), Graphics.Graphics.LineType.LineNormal,
                        lineColor);
                }

                // draw horizontal lines first
                for (int i = gridStartX; i < gridStartX + gridCountX; ++i)
                {
                    // skip lines that are out of bound
                    if (i*gridSize < -gridDim || i*gridSize > gridDim)
                        continue;

                    // skip any line that don't match the line type we're adding
                    if (lineType == 0 && (i == 0 || (i%majorLineEvery) == 0))
                        continue;
                    else if (lineType == 1 && (i == 0 || (i%majorLineEvery) != 0))
                        continue;
                    else if (lineType == 2 && i != 0)
                        continue;

                    // draw the line
                    graphics.DrawLine(new Vector3((float) i*gridSize, lineStartY, 0.0f),
                        new Vector3((float) i*gridSize, lineEndY, 0.0f), Graphics.Graphics.LineType.LineNormal,
                        lineColor);
                }
            }

            graphics.EndDraw();
        }

        /// <summary>
        /// Renders this viewport
        /// </summary>
        /// <param name="graphics"></param>
        public override void OnRender(Graphics.Graphics graphics)
        {
            GL.Disable(EnableCap.Multisample);
            DrawGrid(graphics);

            BaseTool tool = controller.CurrentTool;
            tool?.OnRender(graphics, this);

            RenderOverlay(graphics);
            GL.Enable(EnableCap.Multisample);
        }

        /// <summary>
        /// Resizes this viewport
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public override void OnResized(int x, int y, int w, int h)
        {
            mX = x;
            mY = y;
            mWidth = w;
            mHeight = h;

            int halfW = mWidth/2;
            int halfH = mHeight/2;

            mCamera.SetClipDistance(16384, -16384);
            mCamera.SetProjectionWindow(-halfW, halfW, -halfH, halfH);
        }

        /// <summary>
        /// checks if the grid increase buttons are down
        /// </summary>
        /// <param name="e"></param>
        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oem4)
            {
                mGridSizeInPixels = mGridSizeInPixels >> 1;
                if (mGridSizeInPixels == 0)
                    mGridSizeInPixels = 1;

                controller.RenderViewports();
            }
            else if (e.KeyCode == Keys.Oem6)
            {
                mGridSizeInPixels = mGridSizeInPixels << 1;
                if (mGridSizeInPixels == MaxGridSize << 1)
                    mGridSizeInPixels = MaxGridSize;

                controller.RenderViewports();
            }

            BaseTool tool = controller.CurrentTool;
            tool?.OnKeyDown(e);
        }

        /// <summary>
        /// Propegate up keys to tools
        /// </summary>
        /// <param name="e"></param>
        public override void OnKeyUp(KeyEventArgs e)
        {
            BaseTool tool = controller.CurrentTool;
            tool?.OnKeyUp(e);
        }

        /// <summary>
        /// Mouse button down
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="button"></param>
        public override void OnMouseDown(int x, int y, MouseButtons button)
        {
            if (button != MouseButtons.Right)
            {
                BaseTool tool = controller.CurrentTool;
                tool?.OnMouseDown(new Point(x, y), button, this);
            }

            mousePrevPos = new Point(x, y);
            controller.RenderViewports();
        }

        /// <summary>
        /// Mouse movement
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void OnMouseMove(int x, int y)
        {
            Point mouseCurPos = new Point(x, y);
            if (IsButtonHeld(ViewportButtons.RIGHT))
            {
                // set standard values
                Vector3 oldScreenPos = new Vector3(mousePrevPos.X, mousePrevPos.Y, 0);
                Vector3 currentScreenPosition = new Vector3(x, y, 0);

                if (
                    !ViewportToGridPlane((int) oldScreenPos.X, (int) oldScreenPos.Y, GetGridPlane(),
                        ref oldScreenPos) ||
                    !ViewportToGridPlane((int) currentScreenPosition.X, (int) currentScreenPosition.Y, GetGridPlane(),
                        ref currentScreenPosition))
                    return;

                mCamera.Move(oldScreenPos - currentScreenPosition);
            }
            else
            {
                BaseTool tool = controller.CurrentTool;
                tool?.OnMouseMove(mouseCurPos, mousePrevPos, this);
            }

            mousePrevPos = mouseCurPos;
            controller.RenderViewports();
        }

        /// <summary>
        /// mouse button up
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="button"></param>
        public override void OnMouseUp(int x, int y, MouseButtons button)
        {
            if (button != MouseButtons.Right)
            {
                BaseTool tool = controller.CurrentTool;
                tool?.OnMouseUp(new Point(x, y), button, this);
            }

            mousePrevPos = new Point(x, y);
            controller.RenderViewports();
        }

        /// <summary>
        /// Mousewheel movement
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="delta"></param>
        public override void OnMouseWheel(int x, int y, int delta)
        {
            if (IsButtonHeld(ViewportButtons.RIGHT)
                || IsButtonHeld(ViewportButtons.LEFT)
                || IsButtonHeld(ViewportButtons.MIDDLE))
                return;

            double zoomFactor = mCamera.Zoom;
            zoomFactor = zoomFactor/(1.0 + delta/120.0*0.14);

            if (zoomFactor > 0.94f && zoomFactor < 1.06f)
                zoomFactor = 1.0f;

            if (zoomFactor < 0.08f)
                zoomFactor = 0.08f;
            else if (zoomFactor > 72.0f)
                zoomFactor = 72.0f;

            mCamera.Zoom = (float) zoomFactor;

            // redraw the viewport
            controller.RenderViewports();
        }
    }
}