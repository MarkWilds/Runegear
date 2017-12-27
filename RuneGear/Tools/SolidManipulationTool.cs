using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RuneGear.Forms;
using RuneGear.General;
using RuneGear.General.Camera;
using RuneGear.General.Viewport;
using RuneGear.Geometry;
using RuneGear.MapObjects;
using RuneGear.MapObjects.Operations;
using RuneGear.MapObjects.SolidMapObject.Operations;
using RuneGear.Utilities;

namespace RuneGear.Tools
{
    public class SolidManipulationTool : BaseTool
    {
        public enum SolidToolActionType
        {
            None = 0,
            Create,
            Select,
            Drag,
            Transform
        }

        private SolidToolActionType currentAction;
        private Keys modifierKey;
        private Point mouseDownPos;

        public SolidManipulationTool()
        {
            mouseDownPos = new Point();
        }

        public override void Initialize(IEditorController controller)
        {
            this.controller = controller;
            controller.CurrentSolidManipulationMode = SolidManipulationMode.Solid;
            currentAction = SolidToolActionType.None;
            controller.SetCursor(Cursors.Default);
            
            // when entering this mode, we select alle faces of selected solids again
            SetAllFacesSelected(true);
        }

        public override void Deinitialize()
        {
            controller.RubberBand.Handles.ResetMode();
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            modifierKey = e.Modifiers;
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            modifierKey = Keys.None;
        }

        public override void OnRender(Graphics.Graphics graphics, BaseViewport viewport)
        {
            BaseViewportCamera camera = viewport.Camera;
            RubberBand rubberband = controller.RubberBand;
            SolidRenderOperation render = new SolidRenderOperation
            {
                Viewport = viewport,
                Graphics = graphics
            };

            // Draw all solids
            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                    GL.Enable(EnableCap.DepthTest);

            graphics.BeginDraw(camera.GetViewMatrix() * camera.GetProjMatrix());

            foreach (MapObject mapObject in controller.SceneDocument)
            {
                if (mapObject.Selected)
                    continue;

                mapObject.PerformOperation(render);
            }

            controller.Selection?.PerformOperation(render);

            // only in orthographic viewport
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE)
            {
                // draw rubberband 
                if (rubberband.Bounds.HasVolume2D)
                    graphics.DrawWireframeAabb(rubberband.Bounds, Graphics.Graphics.LineType.LineDashed, rubberband.Color, rubberband.Transformation, viewport.Zoom);
                else
                {   
                    // draw "rubberband" for the selection
                    graphics.DrawWireframeAabb(controller.Selection?.Bounds, Graphics.Graphics.LineType.LineDashed,
                        viewport.RenderMode == BaseViewport.RenderModes.SOLID ? Color.Yellow : Color.Red,
                        Matrix4.Identity, viewport.Zoom);
                }

                // draw grabhandles for selection
                if (controller.RubberBand.ShowGrabhandles)
                {
                    SolidGrabHandles handles = controller.RubberBand.Handles;
                    Matrix4 viewportMatrix = viewport.Camera.GetWorldMatrix().ClearTranslation();
                    handles.CreateHandles(controller.Selection, viewportMatrix, viewport.Zoom);
                    handles.Render(graphics);
                }
            }

            graphics.EndDraw();

            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                GL.Disable(EnableCap.DepthTest);
        }

        public override bool OnMouseDown(Point mouseCurPos, MouseButtons button, BaseViewport viewport)
        {
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE
                && button == MouseButtons.Left)
            {
                RubberBand rubberBand = controller.RubberBand;

                bool controlPressed = Control.ModifierKeys == Keys.Control;
                bool isSelectedSolidHit = IsSelectedSolidAabbHit(mouseCurPos.X, mouseCurPos.Y, viewport);
                bool isGrabHandleHit = GetGrableHandleHit(mouseCurPos.X, mouseCurPos.Y, viewport) != (int)SolidGrabHandles.HitStatus.None;

                // check if we want to do something with the selected solids
                if ((isSelectedSolidHit || isGrabHandleHit) && !controlPressed)
                {
                    currentAction = isSelectedSolidHit ? SolidToolActionType.Drag : SolidToolActionType.Transform;
                    rubberBand.Color = Color.Red;
                    rubberBand.Bounds = (AABB)controller.Selection.Bounds.Clone();
                }
                else
                {
                    // default is solid creation
                    rubberBand.Color = Color.Yellow;
                    currentAction = SolidToolActionType.Create;

                    MapObject hitMapObject = controller.GetMapObjectIfHit(mouseCurPos.X, mouseCurPos.Y, viewport);
                    SelectMapObject(hitMapObject);
                    controller.UpdateUserInterface();
                    if (hitMapObject != null)
                        currentAction = SolidToolActionType.Select;
                }
            }
            else if(button == MouseButtons.Left)
            {
                MapObject hitMapObject = controller.GetMapObjectIfHit(mouseCurPos.X, mouseCurPos.Y, viewport);
                SelectMapObject(hitMapObject);
                controller.UpdateUserInterface();
            }

            mouseDownPos = mouseCurPos;

            return true;
        }

        public override bool OnMouseMove(Point mouseCurPos, Point mousePrevPos, BaseViewport viewport)
        {
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE)
            {
                UpdateCursor(mouseCurPos, viewport);

                if (viewport.IsButtonHeld(BaseViewport.ViewportButtons.LEFT))
                {
                    // set standard values
                    Vector3 mouseDownPosition = new Vector3(mouseDownPos.X, mouseDownPos.Y, 0);
                    Vector3 prevMousePosition = new Vector3(mousePrevPos.X, mousePrevPos.Y, 0);
                    Vector3 currentMousePosition = new Vector3(mouseCurPos.X, mouseCurPos.Y, 0);
                    Matrix4 fromGridSpaceMatrix = viewport.Camera.GetViewMatrix().ClearTranslation();

                    // we only give the rubberband volume if the mouse has moved in creation mode
                    bool mouseHasMoved = (currentMousePosition - mouseDownPosition).Length > GeneralUtility.Epsilon;
                    if (currentAction == SolidToolActionType.Create && mouseHasMoved)
                    {
                        mouseDownPosition.Z = -viewport.GridSize * 4;
                        currentMousePosition.Z = viewport.GridSize * 4;
                    }

                    // convert to world
                    mouseDownPosition = viewport.ViewportToWorld(mouseDownPosition);
                    prevMousePosition = viewport.ViewportToWorld(prevMousePosition);
                    currentMousePosition = viewport.ViewportToWorld(currentMousePosition);

                    // snap
                    Vector3 snappedDownMousePosition = GeneralUtility.SnapToGrid(mouseDownPosition, viewport.GridSize);
                    Vector3 snappedPrevMousePosition = GeneralUtility.SnapToGrid(prevMousePosition, viewport.GridSize);
                    Vector3 snappedCurrentMousePosition = GeneralUtility.SnapToGrid(currentMousePosition, viewport.GridSize);

                    RubberBand rubberband = controller.RubberBand;

                    switch (currentAction)
                    {
                        case SolidToolActionType.Create:
                            rubberband.Bounds.Reset();
                            rubberband.Bounds.Grow(snappedDownMousePosition);
                            rubberband.Bounds.Grow(snappedCurrentMousePosition);
                            break;
                        case SolidToolActionType.Drag:
                            {
                                Vector3 delta = snappedCurrentMousePosition - snappedPrevMousePosition;
                                if (delta != Vector3.Zero)
                                {
                                    TranslateOperation translate = new TranslateOperation(delta)
                                    {
                                        GridSize = viewport.GridSize,
                                        Transform = fromGridSpaceMatrix
                                    };
                                    rubberband.PerformOperation(translate);
                                    rubberband.ShowGrabhandles = false;
                                }
                            }
                            break;
                        case SolidToolActionType.Transform:
                            {
                                SolidGrabHandles handles = rubberband.Handles;
                                IMapObjectOperation operation = null;

                                Vector3 delta = currentMousePosition - prevMousePosition;
                                if (delta != Vector3.Zero)
                                    rubberband.ShowGrabhandles = false;

                                switch (handles.Mode)
                                {
                                    case SolidGrabHandles.HandleMode.Resize:
                                        operation = new ResizeTransformation(fromGridSpaceMatrix, snappedPrevMousePosition,
                                            snappedCurrentMousePosition,
                                            handles.LastHitStatus, viewport.GridSize);
                                        break;
                                    case SolidGrabHandles.HandleMode.Rotate:
                                        operation = new RotateTransformation(fromGridSpaceMatrix, mouseDownPosition,
                                            currentMousePosition, Matrix4.Identity);
                                        break;
                                    case SolidGrabHandles.HandleMode.Skew:
                                        operation = new SkewTransformation(fromGridSpaceMatrix, snappedDownMousePosition,
                                            snappedCurrentMousePosition, Matrix4.Identity,
                                            handles.LastHitStatus);
                                        break;
                                }

                                if (operation != null)
                                    controller.RubberBand.PerformOperation(operation);
                            }

                            break;
                    }
                }
            }
            else
            {
                controller.SetCursor(Cursors.Default);
            }

            return true;
        }

        public override bool OnMouseUp(Point mouseCurPos, MouseButtons button, BaseViewport viewport)
        {
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE
                && button == MouseButtons.Left)
            {
                RubberBand rubberBand = controller.RubberBand;
                MapObject selectionGroup = controller.Selection;
                SolidGrabHandles handles = rubberBand.Handles;
                Matrix4 fromGridSpaceMatrix = viewport.Camera.GetViewMatrix().ClearTranslation();

                // check for the program to decide if it needs to go the next grab handle mode
                bool isHoveringSelectedSolid = IsSelectedSolidAabbHit(mouseCurPos.X, mouseCurPos.Y, viewport);
                bool hasNoNewSelection = currentAction != SolidToolActionType.Select;

                Vector3 snappedDownMousePosition = GeneralUtility.SnapToGrid(
                    new Vector3(mouseDownPos.X, mouseDownPos.Y, 0), viewport.GridSize);
                Vector3 snappedCurMousePosition = GeneralUtility.SnapToGrid(new Vector3(mouseCurPos.X, mouseCurPos.Y, 0),
                    viewport.GridSize);
                bool mouseHasNotMoved = snappedDownMousePosition == snappedCurMousePosition;

                if (isHoveringSelectedSolid && mouseHasNotMoved && hasNoNewSelection)
                {
                    handles.NextMode();
                }
                else
                {
                    switch (currentAction)
                    {
                        case SolidToolActionType.Create:
                            controller.CreateSolid(fromGridSpaceMatrix);
                            break;
                        case SolidToolActionType.Drag:
                            // set new position
                            Vector3 displacement = rubberBand.Bounds.Center - selectionGroup.Bounds.Center;
                            controller.Selection.PerformOperation(new TranslateOperation(displacement));
                            break;
                        case SolidToolActionType.Transform:
                            Vector3 oldBoundVector = selectionGroup.Bounds.Max - selectionGroup.Bounds.Min;
                            Vector3 newBoundVector = rubberBand.Bounds.Max - rubberBand.Bounds.Min;
                            IMapObjectOperation operation = null;

                            switch (handles.Mode)
                            {
                                case SolidGrabHandles.HandleMode.Resize:
                                    operation = new ResizeTransformation(fromGridSpaceMatrix, oldBoundVector,
                                        newBoundVector,
                                        handles.LastHitStatus, viewport.GridSize);
                                    break;
                                case SolidGrabHandles.HandleMode.Rotate:
                                    operation = new RotateTransformation(fromGridSpaceMatrix, Vector3.Zero, Vector3.Zero,
                                        rubberBand.Transformation);
                                    break;
                                case SolidGrabHandles.HandleMode.Skew:
                                    operation = new SkewTransformation(fromGridSpaceMatrix, Vector3.Zero, Vector3.Zero,
                                        rubberBand.Transformation,
                                        handles.LastHitStatus);
                                    break;
                            }

                            if (operation != null)
                                controller.Selection.PerformOperation(operation);
                            break;
                    }

                    controller.UpdateUserInterface();
                }

                rubberBand.SetToZeroVolume();
                rubberBand.ShowGrabhandles = true;
                currentAction = SolidToolActionType.None;
            }

            return true;
        }

        private void UpdateCursor(Point mousePos, BaseViewport viewport)
        {
            SolidGrabHandles handles = controller.RubberBand.Handles;

            if (currentAction == SolidToolActionType.Transform)
                return;

            SolidGrabHandles.HitStatus hitStatus = (SolidGrabHandles.HitStatus)GetGrableHandleHit(mousePos.X, mousePos.Y, viewport);

            if (IsSelectedSolidAabbHit(mousePos.X, mousePos.Y, viewport) || currentAction == SolidToolActionType.Drag)
                controller.SetCursor(Cursors.SizeAll);
            else if (hitStatus == SolidGrabHandles.HitStatus.None)
                controller.SetCursor(Cursors.Cross);
            else if (handles.Mode == SolidGrabHandles.HandleMode.Resize)
            {
                if (hitStatus == SolidGrabHandles.HitStatus.TopLeft || hitStatus == SolidGrabHandles.HitStatus.BottomRight)
                    controller.SetCursor(Cursors.SizeNWSE);
                else if (hitStatus == SolidGrabHandles.HitStatus.TopRight || hitStatus == SolidGrabHandles.HitStatus.BottomLeft)
                    controller.SetCursor(Cursors.SizeNESW);
                else if (hitStatus == SolidGrabHandles.HitStatus.Left || hitStatus == SolidGrabHandles.HitStatus.Right)
                    controller.SetCursor(Cursors.SizeWE);
                else if (hitStatus == SolidGrabHandles.HitStatus.Top || hitStatus == SolidGrabHandles.HitStatus.Bottom)
                    controller.SetCursor(Cursors.SizeNS);
            }
            else if (handles.Mode == SolidGrabHandles.HandleMode.Skew)
            {
                if (hitStatus == SolidGrabHandles.HitStatus.Left || hitStatus == SolidGrabHandles.HitStatus.Right)
                    controller.SetCursor(Cursors.SizeNS);
                else if (hitStatus == SolidGrabHandles.HitStatus.Top || hitStatus == SolidGrabHandles.HitStatus.Bottom)
                    controller.SetCursor(Cursors.SizeWE);
            }
            else if (handles.Mode == SolidGrabHandles.HandleMode.Rotate)
                controller.SetCursor(EditorForm.Rotate);
        }

        /// <summary>
        /// Check if selected solid aabb is hit
        /// </summary>
        private bool IsSelectedSolidAabbHit(int x, int y, BaseViewport viewport)
        {
            if (controller.Selection.Empty)
                return false;

            Ray ray = viewport.ViewportToRay(x, y);
            return ray.IsIntersecting(controller.Selection.Bounds);
        }

        /// <summary>
        /// Check if we hit the handles
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="viewport"></param>
        /// <returns></returns>
        private int GetGrableHandleHit(int x, int y, BaseViewport viewport)
        {
            MapObjectGroup selectedMapObjectGroup = controller.Selection;
            if (selectedMapObjectGroup.Empty)
                return (int)SolidGrabHandles.HitStatus.None; 

            Matrix4 toGridMatrix = viewport.Camera.GetWorldMatrix().ClearTranslation();

            SolidGrabHandles handles = controller.RubberBand.Handles;
            handles.CreateHandles(selectedMapObjectGroup, toGridMatrix, viewport.Zoom);

            return handles.CheckHit(viewport.ViewportToRay(x, y));
        }


        /// <summary>
        /// (De)Select the given map object
        /// </summary>
        /// <param name="mapObject"></param>
        public void SelectMapObject(MapObject mapObject)
        {
            MapObjectGroup selection = controller.Selection;
            if (mapObject != null)
            {
                if (modifierKey == Keys.Control)
                {
                    if (selection.Contains(mapObject))
                    {
                        mapObject.Selected = false;
                        selection.Remove(mapObject);
                    }
                    else
                    {
                        mapObject.Selected = true;
                        selection.Add(mapObject);
                    }
                }
                else if (!selection.Contains(mapObject))
                {
                    selection.Clear();
                    mapObject.Selected = true;
                    selection.Add(mapObject);
                }
            }
            else if (!selection.Empty)
            {
                // deselect
                SolidGrabHandles handles = controller.RubberBand.Handles;
                selection.Clear();
                handles.ResetMode();
            }
        }
    }
}
