using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using RuneGear.General;
using RuneGear.General.Viewport;

namespace RuneGear.Controls
{
    public class EditorGlViewport : GLControl
    {
        private const int VIEWPORT_COUNT = 4;
        private Graphics.Graphics graphics;
        private BaseViewport[] viewports;
        private BaseViewport capturedViewport;
        private BaseViewport lastFocusedViewport;

        public class ViewportEventArgs : EventArgs
        {
            public ViewportEventArgs(BaseViewport viewport)
            {
                FocusedViewport = viewport;
            }

            public BaseViewport FocusedViewport { get; }
        }
        public event EventHandler<ViewportEventArgs> OnViewportFocus;

        /// <summary>
        /// Constructor initalizing important variables
        /// </summary>
        public EditorGlViewport()
            : base(new GraphicsMode(32, 24, 0, 8), 3, 3, GraphicsContextFlags.Default)
        {
            BackColor = Color.Black;
            Dock = DockStyle.Fill;
            Margin = new Padding(0, 0, 0, 0);
            TabIndex = 0;

            graphics = new Graphics.Graphics();
            viewports = new BaseViewport[VIEWPORT_COUNT];
            capturedViewport = null;

            InitializeComponent();
            InitializeViewports();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.MouseWheel += OnMouseWheel;

            // 
            // EditorGLViewport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "EditorGlViewport";
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Initialize the viewport controllers
        /// </summary>
        private void InitializeViewports()
        {
            viewports[0] = new PerspViewport(this.PointToScreen(this.Location));
            viewports[1] = new OrthoViewport(BaseViewport.ViewportTypes.TOP);
            viewports[2] = new OrthoViewport(BaseViewport.ViewportTypes.SIDE);
            viewports[3] = new OrthoViewport(BaseViewport.ViewportTypes.FRONT);

            for (int i = 0; i < 4; i++)
            {
                viewports[i].ResetViewport();
            }
        }

        /// <summary>
        /// When the control is created initialize the renderer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // need to use licenseManager as DesignMode is not set at this stage
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                graphics.Init();
        }

        /// <summary>
        /// When the control is destroyed deinitialize the renderer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            if (!DesignMode)
                graphics.DeInit();
        }

        /// <summary>
        /// Draw the viewports
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.Clear(Color.DodgerBlue);
            }
            else
            {
                graphics.Clear(Color.Black);

                for (int i = 0; i < VIEWPORT_COUNT; i++)
                {
                    viewports[i].Render(graphics);
                }

                SwapBuffers();
            }
        }

        /// <summary>
        /// Resizes the viewports
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            int halfWidth = Size.Width / 2;
            int halfHeight = Size.Height / 2;

            viewports[0].OnResized(0, 0, halfWidth, halfHeight);
            viewports[1].OnResized(halfWidth, 0, halfWidth, halfHeight);
            viewports[2].OnResized(0, halfHeight, halfWidth, halfHeight);
            viewports[3].OnResized(halfWidth, halfHeight, halfWidth, halfHeight);
        }

        /// <summary>
        /// Set the controller for the viewports
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(IEditorController controller)
        {
            for (int i = 0; i < VIEWPORT_COUNT; i++)
            {
                viewports[i].SetController(controller);
            }
        }

        /// <summary>
        /// Sets the rendermode for the currently captured viewport
        /// </summary>
        /// <param name="renderMode"></param>
        public bool SetRendermode(BaseViewport.RenderModes renderMode)
        {
            if(lastFocusedViewport != null)
            {
                lastFocusedViewport.RenderMode = renderMode;
                Invalidate();

                return true;
            }

            return false;
        }

        /// <summary>
        /// handles viewport capturing
        /// </summary>
        /// <param name="viewport"></param>
        private void CaptureViewport(BaseViewport viewport)
        {
            capturedViewport = lastFocusedViewport = viewport;
        }

        /// <summary>
        /// Resets the viewports to original state
        /// </summary>
        public void ResetViewports()
        {
            lastFocusedViewport = capturedViewport = null;
            for (int i = 0; i < VIEWPORT_COUNT; i++)
            {
                viewports[i].ResetViewport();
            }
        }

        #region Viewport events

        private void RaiseOnViewportFocusEvent(BaseViewport viewport)
        {
            ViewportEventArgs e = new ViewportEventArgs(viewport);

            OnViewportFocus?.Invoke(this, e);
        }

        /// <summary>
        /// Call when a key is down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (lastFocusedViewport != null)
            {
                lastFocusedViewport.OnKeyDown(e);
                RaiseOnViewportFocusEvent(lastFocusedViewport);
            }
        }

        /// <summary>
        /// Call when a key is up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (lastFocusedViewport != null)
            {
                lastFocusedViewport.OnKeyUp(e);
                RaiseOnViewportFocusEvent(lastFocusedViewport);
            }
        }

        /// <summary>
        /// Call when the main form control has moved
        /// </summary>
        public void OnEditorMoves(object sender, EventArgs e)
        {
            for (int i = 0; i < VIEWPORT_COUNT; i++)
            {
                viewports[i].OnEditorMoves(Parent.PointToScreen(Location));
            }
        }

        /// <summary>
        /// Calls when the main form has gained focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEditorGotFocus(object sender, EventArgs e)
        {
            capturedViewport?.OnEditorGotFocus();
        }

        /// <summary>
        /// Called when the main form has lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEditorLostFocus(object sender, EventArgs e)
        {
            capturedViewport?.OnEditorLostFocus();
        }

        #endregion

        #region Mouse events

        /// <summary>
        /// Mouse button down
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (capturedViewport != null)
            {
                capturedViewport.MouseDown(e.X, e.Y, e.Button);
            }
            else
            {
                for (int i = 0; i < VIEWPORT_COUNT; i++)
                {
                    if (viewports[i].IsPointInViewport(e.X, e.Y))
                    {
                        CaptureViewport(viewports[i]);
                        viewports[i].MouseDown(e.X, e.Y, e.Button);
                        RaiseOnViewportFocusEvent(viewports[i]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// mouse button up
        /// </summary>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (capturedViewport != null)
            {
                capturedViewport.MouseUp(e.X, e.Y, e.Button);
                RaiseOnViewportFocusEvent(capturedViewport);

                if (!capturedViewport.CapturedMouse)
                    capturedViewport = null;
            }
        }

        /// <summary>
        /// Mouse movement
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (capturedViewport != null)
            {
                capturedViewport.MouseMove(e.X, e.Y);
            }
            else
            {
                for (int i = 0; i < VIEWPORT_COUNT; i++)
                {
                    if (viewports[i].IsPointInViewport(e.X, e.Y))
                    {
                        lastFocusedViewport = viewports[i];
                        viewports[i].MouseMove(e.X, e.Y);
                        RaiseOnViewportFocusEvent(viewports[i]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Mousewheel movement
        /// </summary>
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (capturedViewport != null)
            {
                capturedViewport.MouseWheel(e.X, e.Y, e.Delta);
            }
            else
            {
                for (int i = 0; i < VIEWPORT_COUNT; i++)
                {
                    if (viewports[i].IsPointInViewport(e.X, e.Y))
                    {
                        lastFocusedViewport = viewports[i];
                        viewports[i].MouseWheel(e.X, e.Y, e.Delta);
                        RaiseOnViewportFocusEvent(viewports[i]);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}