using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using RuneGear.General.Camera;
using RuneGear.Geometry;
using RuneGear.Tools;
using RuneGear.Utilities.Extensions;

namespace RuneGear.General.Viewport
{
    public class PerspViewport : BaseViewport
    {
        private const int CROSS_SIZE = 6;

        private PerspCamera mCamera;
        private Vector2 mousePrevPos;
        private Vector2 mCurrentMousePos;
        private System.Timers.Timer mFreeCamTimer;

        private const float mFramesPerSecond = 120.0f;
        private const float mSpeed = 1960.0f;
        private float mFreeModeMoveSpeed = mSpeed/mFramesPerSecond;
        private float mSpeedModifier = 0.5f;
        private float mMouseSensitivity = 0.2f;

        public Point parentOriginPoint;

        public override BaseViewportCamera Camera => mCamera;
        public override int GridSize => 0;
        public override float Zoom => 1.0f;

        /// <summary>
        /// Constructor
        /// </summary>
        public PerspViewport(Point parentOrigin)
        {
            parentOriginPoint = parentOrigin;
            mCamera = new PerspCamera();
            mousePrevPos = new Vector2();
            mFreeCamTimer = new System.Timers.Timer(1000.0/mFramesPerSecond);
            mFreeCamTimer.Elapsed += FreeModeUpdate;
            MViewportTypes = ViewportTypes.PERSPECTIVE;
        }

        public override void ResetViewport()
        {
            RenderMode = RenderModes.TEXTURED;
            mCamera.Reset();
            mCamera.Move(0, 0, -512);
            mCamera.Rotate(1.0f, 0, 0, 35.0f);
        }

        private void RenderCrosshair(Graphics.Graphics graphics)
        {
            // render cross if captured the mouse
            if (CapturedMouse)
            {
                graphics.BeginDraw(MathExtensions.CreateOrthographicLH(mWidth, mHeight, 0, 1.0f));

                graphics.DrawSolidRectangle(Vector3.UnitX * -CROSS_SIZE + -Vector3.UnitY, Vector3.UnitX * CROSS_SIZE + -Vector3.UnitY,
                    Vector3.UnitX * CROSS_SIZE + Vector3.UnitY, Vector3.UnitX * -CROSS_SIZE + Vector3.UnitY, Color.White);
                graphics.DrawSolidRectangle(Vector3.UnitY * -CROSS_SIZE + -Vector3.UnitX, Vector3.UnitY * -CROSS_SIZE + Vector3.UnitX,
                    Vector3.UnitY * CROSS_SIZE + Vector3.UnitX, Vector3.UnitY * CROSS_SIZE + -Vector3.UnitX, Color.White);

                graphics.EndDraw();
            }
        }

        private void StartFreeMode()
        {
            Cursor.Hide();
            mFreeCamTimer.Start();
        }

        private void StopFreeMode()
        {
            Cursor.Show();
            mFreeCamTimer.Stop();
        }

        public void FreeModeUpdate(object sender, ElapsedEventArgs e)
        {
            // do rotation
            MouseState mouse = Mouse.GetState();

            mCurrentMousePos.X = mouse.X;
            mCurrentMousePos.Y = mouse.Y;

            Vector2 delta = (mCurrentMousePos - mousePrevPos)*mMouseSensitivity;
            if (delta != Vector2.Zero)
            {
                mCamera.RotateFPS(delta.X, delta.Y);
            }

            // do movement
            KeyboardState keyboardState = Keyboard.GetState();
            mSpeedModifier = keyboardState.IsKeyDown(Key.LShift) ? 2f : 1f;

            mCamera.MoveFPS(GetMovementVector(keyboardState), mFreeModeMoveSpeed*mSpeedModifier);

            // general 
            Mouse.SetPosition(parentOriginPoint.X + mWidth/2, parentOriginPoint.Y + mHeight/2);
            controller.RenderViewports();

            mousePrevPos = mCurrentMousePos;
        }

        private Vector3 GetMovementVector(KeyboardState state)
        {
            Vector3 result = new Vector3();

            // vertical
            if (state.IsKeyDown(Key.W))
                result.Y = 1;
            else if (state.IsKeyDown(Key.S))
                result.Y = -1;

            // horizontal
            if (state.IsKeyDown(Key.D))
                result.X = 1;
            else if (state.IsKeyDown(Key.A))
                result.X = -1;

            return result;
        }

        /// <summary>
        /// Convert the viewport coordinates to a ray
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override Ray ViewportToRay(int x, int y)
        {
            return CapturedMouse ? ViewportToRayCenter(x, y) : ViewportToRayMouse(x, y);
        }

        private Ray ViewportToRayMouse(int x, int y)
        {
            Matrix4 cameraProjectionMatrix = mCamera.GetProjMatrix();
            Matrix4 cameraWorldMatrix = mCamera.GetWorldMatrix();
            Vector3 direction;

            // convert to view space
            direction.X = ((2.0f * x / mWidth - 1)) / cameraProjectionMatrix.M11;
            direction.Y = -((2.0f * y / mHeight - 1)) / cameraProjectionMatrix.M22;
            direction.Z = 1.0f / cameraProjectionMatrix.M33;

            direction = Vector4.Transform(new Vector4(direction.X, direction.Y, direction.Z, 0.0f), cameraWorldMatrix).Xyz;

            Vector3 rayPos = cameraWorldMatrix.Row3.Xyz;
            Vector3 rayDir = direction.Normalized();

            return new Ray(rayPos, rayDir);
        }

        /// <summary>
        /// Convert the center of the camera to a ray
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Ray ViewportToRayCenter(int x, int y)
        {
            Matrix4 cameraProjectionMatrix = mCamera.GetProjMatrix();
            Matrix4 cameraWorldMatrix = mCamera.GetWorldMatrix();
            Vector3 viewportPoint;

            // convert to view space
            viewportPoint.X = ((2.0f*x/(float) mWidth - 1))/cameraProjectionMatrix.M11;
            viewportPoint.Y = -((2.0f*y/(float) mHeight - 1))/cameraProjectionMatrix.M22;
            viewportPoint.Z = 1.0f/cameraProjectionMatrix.M33;

            Vector3 rayPos = viewportPoint.TransformL(cameraWorldMatrix);
            Vector3 rayDir = cameraWorldMatrix.Row2.Xyz;

            return new Ray(rayPos, rayDir);
        }

        /// <summary>
        /// Renders the 3D brushes
        /// </summary>
        /// <param name="graphics"></param>
        public override void OnRender(Graphics.Graphics graphics)
        {
            BaseTool tool = controller.CurrentTool;
            tool?.OnRender(graphics, this);

            GL.Disable(EnableCap.Multisample);

            RenderCrosshair(graphics);
            RenderOverlay(graphics);

            GL.Enable(EnableCap.Multisample);
        }

        public override void OnResized(int x, int y, int w, int h)
        {
            mX = x;
            mY = y;
            mWidth = w;
            mHeight = h;

            mCamera.SetWindowDimensions(w, h);
        }

        /// <summary>
        /// checks if the grid increase buttons are down
        /// </summary>
        /// <param name="e"></param>
        public override void OnKeyDown(KeyEventArgs e)
        {
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

        public override void OnMouseDown(int x, int y, MouseButtons button)
        {
            if (button != MouseButtons.Right)
            {
                BaseTool tool = controller.CurrentTool;
                tool?.OnMouseDown(new Point(x, y), button, this);
            }
            else
            {
                CapturedMouse = !CapturedMouse;

                if (CapturedMouse)
                {
                    Mouse.SetPosition(parentOriginPoint.X + mWidth/2, parentOriginPoint.Y + mHeight/2);
                    MouseState mouse = Mouse.GetState();
                    mousePrevPos.X = mouse.X;
                    mousePrevPos.Y = mouse.Y;

                    StartFreeMode();
                }
                else
                {
                    StopFreeMode();
                }  
            }

            controller.RenderViewports();
        }

        public override void OnMouseMove(int x, int y)
        {
            if (!IsButtonHeld(ViewportButtons.RIGHT))
            {
                BaseTool tool = controller.CurrentTool;
                tool?.OnMouseMove(new Point(x, y), new Point((int) mousePrevPos.X, (int) mousePrevPos.Y), this);
            }
        }

        public override void OnMouseUp(int x, int y, MouseButtons button)
        {
            if (button != MouseButtons.Right)
            {
                BaseTool tool = controller.CurrentTool;
                tool?.OnMouseUp(new Point(x, y), button, this);
            }
        }

        public override void OnEditorGotFocus()
        {
            if (CapturedMouse)
            {
                Mouse.SetPosition(parentOriginPoint.X + mWidth / 2, parentOriginPoint.Y + mHeight / 2);
                MouseState mouse = Mouse.GetState();
                mousePrevPos.X = mouse.X;
                mousePrevPos.Y = mouse.Y;

                StartFreeMode();
            }
        }

        public override void OnEditorLostFocus()
        {
            if (CapturedMouse)
            {
                StopFreeMode();
            }
        }

        public override void OnEditorMoves(Point parentAbsolutePosition)
        {
            parentOriginPoint = parentAbsolutePosition;
        }
    }
}