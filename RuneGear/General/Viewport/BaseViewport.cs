using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RuneGear.General.Camera;
using RuneGear.Geometry;
using RuneGear.Utilities.Extensions;

namespace RuneGear.General.Viewport
{
    public abstract class BaseViewport
    {
        public enum ViewportButtons
        {
            LEFT = 1,
            MIDDLE = 2,
            RIGHT = 4
        }

        public enum RenderModes
        {
            WIREFRAME = 0,
            SOLID = 1,
            TEXTURED,
            TEXTUREDLIGHTED
        }

        public enum ViewportTypes
        {
            /// <summary>
            /// XY plane
            /// </summary>
            FRONT = 0,

            /// <summary>
            /// ZY plane
            /// </summary>
            SIDE,

            /// <summary>
            /// XZ plane
            /// </summary>
            TOP,

            /// <summary>
            /// 3D viewport
            /// </summary>
            PERSPECTIVE
        }

        private const int OVERLAY_GAP = 1;

        protected int mX, mY;
        protected int mWidth, mHeight;
        protected int mButtonsHeld;
        protected RenderModes mRenderMode;
        protected ViewportTypes MViewportTypes;

        public bool CapturedMouse { get; protected set; }

        public RenderModes RenderMode
        {
            get { return mRenderMode; }
            set { mRenderMode = value; }
        }

        public ViewportTypes ViewportType
        {
            get { return MViewportTypes;}
            set { MViewportTypes = value; }
        }

        public int Width => mWidth;

        public int Height => mHeight;

        public abstract BaseViewportCamera Camera { get; }

        public abstract int GridSize { get; }

        public abstract float Zoom { get; }

        public IEditorController Controller => controller;
        protected IEditorController controller;


        public abstract void ResetViewport();
        public abstract void OnRender(Graphics.Graphics graphics);
        public abstract void OnResized(int x, int y, int w, int h);

        public abstract Ray ViewportToRay(int x, int y);

        public virtual void OnMouseDown(int x, int y, MouseButtons button)
        {
        }

        public virtual void OnMouseUp(int x, int y, MouseButtons button)
        {
        }

        public virtual void OnMouseMove(int x, int y)
        {
        }

        public virtual void OnMouseWheel(int x, int y, int delta)
        {
        }

        public virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        public virtual void OnKeyUp(KeyEventArgs e)
        {
        }

        public virtual void OnEditorMoves(Point parentAbsolutePosition)
        {
        }

        public virtual void OnEditorGotFocus()
        {
        }

        public virtual void OnEditorLostFocus()
        {
        }

        //############ TEMPLATE PATTERN #########################
        public void Render(Graphics.Graphics graphics)
        {
            int flippedY = (mHeight*2) - (mY + 1) - (mHeight - 2);
            GL.Viewport(mX + 1, flippedY, mWidth - 2, mHeight - 2);

            OnRender(graphics);
        }

        public void MouseDown(int x, int y, MouseButtons button)
        {
            RegisterButton(button);
            x -= mX;
            y -= mY;

            OnMouseDown(x, y, button);
        }

        public void MouseUp(int x, int y, MouseButtons button)
        {
            UnregisterButton(button);
            x -= mX;
            y -= mY;

            OnMouseUp(x, y, button);
        }

        public void MouseMove(int x, int y)
        {
            x -= mX;
            y -= mY;

            OnMouseMove(x, y);
        }

        public void MouseWheel(int x, int y, int delta)
        {
            OnMouseWheel(x, y, delta);
        }

        //############ TEMPLATE END #########################

        public virtual Vector3 WorldToViewport(Vector3 point)
        {
            Matrix4 viewMatrix = Camera.GetViewMatrix();
            Matrix4 projMatrix = Camera.GetProjMatrix();

            Vector4 point4 = new Vector4(point, 1.0f);
            point4 = point4 * viewMatrix * projMatrix;

            point = point4.Xyz/point4.W;

            int winX = (int) Math.Round((point.X + 1.0) / 2.0 * mWidth);
            int winY = (int) Math.Round((1.0 - point.Y) / 2.0 * Height);

            return new Vector3(winX, winY, 0.0f);
        }

        public virtual Vector3 ViewportToWorld(Vector3 p)
        {
            Matrix4 projMat = Camera.GetProjMatrix();
            Matrix4 cameraWorldMatrix = Camera.GetWorldMatrix();
            Vector3 viewportPoint;

            // convert to view space
            viewportPoint.X = (((2.0f * p.X) / Width - 1)) / projMat.M11;
            viewportPoint.Y = -(((2.0f * p.Y) / Height - 1)) / projMat.M22;
            viewportPoint.Z = p.Z;

            // convert to viewport space
            return viewportPoint.TransformL(cameraWorldMatrix);
        }

        /// <summary>
        /// Sets the controller for this viewport
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(IEditorController controller)
        {
            this.controller = controller;
            CapturedMouse = false;
        }

        /// <summary>
        /// Renders a overlay for the viewport
        /// </summary>
        /// <param name="graphics"></param>
        protected virtual void RenderOverlay(Graphics.Graphics graphics)
        {
            Color overlayColor = SystemColors.Control;
            Vector3 pointOne = new Vector3(OVERLAY_GAP, OVERLAY_GAP, 0);
            Vector3 pointTwo = new Vector3(mWidth - OVERLAY_GAP, OVERLAY_GAP, 0);
            Vector3 pointThree = new Vector3(mWidth - OVERLAY_GAP, mHeight - OVERLAY_GAP, 0);
            Vector3 pointFour = new Vector3(OVERLAY_GAP, mHeight - OVERLAY_GAP, 0);

            graphics.BeginDraw(Matrix4.CreateOrthographicOffCenter(0, mWidth, mHeight, 0, 0, 1));

            graphics.DrawLine(pointOne, pointTwo, Graphics.Graphics.LineType.LineNormal, overlayColor);
            graphics.DrawLine(pointTwo, pointThree, Graphics.Graphics.LineType.LineNormal, overlayColor);
            graphics.DrawLine(pointThree, pointFour, Graphics.Graphics.LineType.LineNormal, overlayColor);
            graphics.DrawLine(pointFour, pointOne, Graphics.Graphics.LineType.LineNormal, overlayColor);

            graphics.EndDraw();
        }

        /// <summary>
        /// Draw Axes
        /// </summary>
        /// <param name="graphics"></param>
        protected void DrawAxes(Graphics.Graphics graphics)
        {
            float axesLength = 256.0f;

            GL.LineWidth(5.0f);
            graphics.DrawLine(Vector3.Zero, Vector3.UnitX*axesLength, Graphics.Graphics.LineType.LineNormal, Color.Red);
            graphics.DrawLine(Vector3.Zero, Vector3.UnitY*axesLength, Graphics.Graphics.LineType.LineNormal, Color.Green);
            graphics.DrawLine(Vector3.Zero, Vector3.UnitZ*axesLength, Graphics.Graphics.LineType.LineNormal, Color.Blue);
            GL.LineWidth(1.0f);
        }

        /// <summary>
        /// Check if the coordinates are within the viewport
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual bool IsPointInViewport(int x, int y)
        {
            return x > mX && x < mX + mWidth &&
                   y > mY && y < mY + mHeight;
        }

        /// <summary>
        /// Convert winforms mousebuttons to viewport mousebuttons
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        protected int GetMouseButtonFlag(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return (int) ViewportButtons.LEFT;
                case MouseButtons.Middle:
                    return (int) ViewportButtons.MIDDLE;
                case MouseButtons.Right:
                    return (int) ViewportButtons.RIGHT;
            }

            return 0;
        }

        /// <summary>
        /// Checks if a button is held
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonHeld(ViewportButtons button)
        {
            return (mButtonsHeld & (int) button) > 0;
        }

        /// <summary>
        /// Enable button in flags integer
        /// </summary>
        /// <param name="button"></param>
        protected void RegisterButton(MouseButtons button)
        {
            mButtonsHeld |= GetMouseButtonFlag(button);
        }

        /// <summary>
        /// Disable button in flags integer
        /// </summary>
        /// <param name="button"></param>
        protected void UnregisterButton(MouseButtons button)
        {
            mButtonsHeld &= ~GetMouseButtonFlag(button);
        }
    }
}