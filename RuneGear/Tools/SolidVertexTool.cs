using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RuneGear.General;
using RuneGear.General.Camera;
using RuneGear.General.Viewport;
using RuneGear.Geometry;
using RuneGear.MapObjects;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.MapObjects.SolidMapObject.Operations;
using RuneGear.Utilities;

namespace RuneGear.Tools
{
    public class SolidVertexTool : BaseTool
    {
        private const int HandleSize = 6;

        public struct Handle
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
        public enum VertexToolActionType
        {
            None = 0,
            SelectMultiple,
            SelectDrag
        }

        private Keys modifierKey;
        private VertexToolActionType currentAction;
        private BaseViewport.ViewportTypes currentActionViewport;
        private Point mouseDownPos;

        private AABB coverBlanket;
        private Vector3 dragStartPosition;
        private bool mouseHasMoved;
        private Dictionary<Solid, List<int>> newHandlesHit;

        public override void Initialize(IEditorController controller)
        {
            this.controller = controller;
            controller.CurrentSolidManipulationMode = SolidManipulationMode.Vertex;
            currentAction = VertexToolActionType.None;
            mouseHasMoved = false;
            newHandlesHit = new Dictionary<Solid, List<int>>();
            coverBlanket = new AABB();
            coverBlanket.Grow(Vector3.Zero);
            controller.SetCursor(Cursors.Default);
        }

        public override void Deinitialize()
        {
            ClearAllSelectedVertices();
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
            Matrix4 vpMatrix4 = camera.GetViewMatrix()*camera.GetProjMatrix();
            SolidRenderOperation render = new SolidRenderOperation
            {
                Viewport = viewport,
                Graphics = graphics
            };

            // Draw all solids
            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                GL.Enable(EnableCap.DepthTest);

            graphics.BeginDraw(vpMatrix4);

            foreach (MapObject mapObject in controller.SceneDocument)
            {
                if (mapObject.Selected)
                    continue;

                mapObject.PerformOperation(render);
            }

            controller.Selection?.PerformOperation(render);

            graphics.EndDraw();

            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                GL.Disable(EnableCap.DepthTest);

            // Draw white wireframesolid on top of selection
            // only in Solid rendermode
            graphics.BeginDraw(vpMatrix4);
            if (viewport.RenderMode == BaseViewport.RenderModes.SOLID || viewport.RenderMode == BaseViewport.RenderModes.TEXTURED)
            {
                Color color = viewport.RenderMode == BaseViewport.RenderModes.TEXTURED ? Color.Yellow : Color.White;
                DoSolidAction(controller.Selection, (solid) =>
                {
                    graphics.DrawWireframeSolid(solid, color);
                });
            }
            graphics.EndDraw();

            // draw handles and blanket on top of everything else
            RenderAllVertexHandles(graphics, viewport);
            RenderBlanket(graphics, viewport);
        }

        /// <summary>
        /// Only draw for the current viewport that is doing the action
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="viewport"></param>
        private void RenderBlanket(Graphics.Graphics graphics, BaseViewport viewport)
        {
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE &&
                coverBlanket.HasVolume2D && currentActionViewport == viewport.ViewportType)
            {
                Vector3 bottomLeft = new Vector3(coverBlanket.Min.X, coverBlanket.Min.Y, 0);
                Vector3 bottomRight = new Vector3(coverBlanket.Max.X, coverBlanket.Min.Y, 0);
                Vector3 topRight = new Vector3(coverBlanket.Max.X, coverBlanket.Max.Y, 0);
                Vector3 topLeft = new Vector3(coverBlanket.Min.X, coverBlanket.Max.Y, 0);

                graphics.BeginDraw(Matrix4.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1));

                //Color blanketColor = Color.FromArgb(64, Color.LightSkyBlue);
                //renderer.DrawSolidRectangle(topLeft, topRight, bottomRight, bottomLeft, blanketColor); // TODO FIX winding....??!?!?!?
                graphics.DrawRectangle(bottomLeft, bottomRight, topRight, topLeft, Graphics.Graphics.LineType.LineDashed, Color.DeepSkyBlue);
                graphics.EndDraw();
            }
        }

        private void RenderAllVertexHandles(Graphics.Graphics graphics, BaseViewport viewport)
        {
            int nearDepth = 1;
            int farDepth = 0;

            //TODO check why this is neccessary
            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
            {
                nearDepth = 0;
                farDepth = 1;
            }

            // handle vertices are already in NDC space when send to the graphicscard
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.DepthRange(nearDepth, farDepth);
            GL.Enable(EnableCap.DepthTest);

            graphics.BeginDraw(Matrix4.Identity);

            Vector3 handleDimensions = new Vector3((float)HandleSize / viewport.Width, (float)HandleSize / viewport.Height, 0.0f);
            Matrix4 vpMatrix = viewport.Camera.GetViewMatrix() * viewport.Camera.GetProjMatrix();
            Handle handle = new Handle();
            List<Handle> selectedHandles = new List<Handle>();

            // create handles and draw non selected handles
            DoSolidAction(controller.Selection, (solid) =>
            {
                int index = 0;
                foreach (Vector3 position in solid.VertexPositions)
                {
                    Vector4 vec4Pos = new Vector4(position, 1.0f) * vpMatrix;
                    Vector3 pos = vec4Pos.Xyz / vec4Pos.W;

                    handle.BottomLeft = pos + (-Vector3.UnitX - Vector3.UnitY) * handleDimensions;
                    handle.BottomRight = pos + (Vector3.UnitX - Vector3.UnitY) * handleDimensions;
                    handle.TopRight = pos + (Vector3.UnitX + Vector3.UnitY) * handleDimensions;
                    handle.TopLeft = pos + (-Vector3.UnitX + Vector3.UnitY) * handleDimensions;

                    if (solid.SelectedVertices.Contains(index))
                    {
                        selectedHandles.Add(handle);
                        index++;
                        continue;
                    }
                    index++;

                    graphics.DrawSolidRectangle(handle.BottomLeft, handle.BottomRight,
                        handle.TopRight, handle.TopLeft, Color.White);

                    graphics.DrawRectangle(handle.BottomLeft, handle.BottomRight,
                    handle.TopRight, handle.TopLeft, Graphics.Graphics.LineType.LineNormal, Color.Black);                    
                }
            });

            graphics.EndDraw();

            GL.Clear(ClearBufferMask.DepthBufferBit);
            graphics.BeginDraw(Matrix4.Identity);

            //draw selected handles
            foreach (Handle selectedHandle in selectedHandles)
            {
                graphics.DrawSolidRectangle(selectedHandle.BottomLeft, selectedHandle.BottomRight,
                selectedHandle.TopRight, selectedHandle.TopLeft, Color.Red);

                graphics.DrawRectangle(selectedHandle.BottomLeft, selectedHandle.BottomRight,
                selectedHandle.TopRight, selectedHandle.TopLeft, Graphics.Graphics.LineType.LineNormal, Color.Black);
            }
            graphics.EndDraw();

            GL.Disable(EnableCap.DepthTest);
            GL.DepthRange(farDepth, nearDepth);
        }

        public override bool OnMouseDown(Point mouseCurPos, MouseButtons button, BaseViewport viewport)
        {
            if (button == MouseButtons.Left)
            {
                currentActionViewport = viewport.ViewportType;
                MapObjectGroup selectedMapObjectGroup = controller.Selection;

                bool isControlPressed = modifierKey == Keys.Control;
                newHandlesHit = GetHandlesHitWithMouse(viewport, mouseCurPos);

                // Check if we hit a vertex handle
                if (newHandlesHit.Count > 0)
                {
                    Action dragStartPositionAction = () =>
                    {
                        // any vertex will do
                        var first = newHandlesHit.First();
                        Solid solid = first.Key;
                        int firstVertex = first.Value[0];

                        SetSelectDragAction(solid.VertexPositions[firstVertex], viewport);
                    };

                    Action isAnySelected = () =>
                    {
                        // if still any selected left, setSelectDrag
                        bool selected = newHandlesHit.All((pair) => pair.Value.Any((index) => pair.Key.SelectedVertices.Contains(index)));
                        if (selected)
                            dragStartPositionAction();
                    };

                    bool anySelected = newHandlesHit.All((pair) => pair.Value.Any((index) => pair.Key.SelectedVertices.Contains(index)));
                    if (isControlPressed)
                    {
                        foreach (KeyValuePair<Solid, List<int>> pair in newHandlesHit)
                        {
                            pair.Value.ForEach((index) =>
                            {
                                if (pair.Key.SelectedVertices.Contains(index))
                                    pair.Key.SelectedVertices.Remove(index);
                                else
                                    pair.Key.SelectedVertices.Add(index);
                            });
                        }

                        isAnySelected();

                    }
                    else if (anySelected)
                    {
                        dragStartPositionAction();
                    }
                    else
                    {
                        ClearAllSelectedVertices();
                        foreach (KeyValuePair<Solid, List<int>> pair in newHandlesHit)
                        {
                            pair.Value.ForEach((index) =>
                            {
                                if(!pair.Key.SelectedVertices.Contains(index))
                                    pair.Key.SelectedVertices.Add(index);
                            });
                        }

                        isAnySelected();
                    }
                }
                else // if no handles hit, check if we hit a solid
                {
                    MapObject rootMapObject = controller.GetMapObjectIfHit(mouseCurPos.X, mouseCurPos.Y, viewport);
                    if (rootMapObject == null)
                    {
                        if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                            ClearAllSelectedVertices();
                        else // no map object hit for orthographic set action to SelectMultiple 
                            currentAction = VertexToolActionType.SelectMultiple;
                    }
                    else
                    {
                        if (isControlPressed)
                        {
                            if (!rootMapObject.Selected)
                            {
                                rootMapObject.Selected = true;
                                selectedMapObjectGroup.Add(rootMapObject);
                            }
                            else
                            {
                                DoSolidAction(rootMapObject, (solid) =>
                                {
                                    solid.SelectedVertices.Clear();
                                });
                                rootMapObject.Selected = false;
                                selectedMapObjectGroup.Remove(rootMapObject);
                            }

                        }
                        else if (!rootMapObject.Selected)
                        {
                            ClearAllSelectedVertices();
                            selectedMapObjectGroup.Clear();
                            rootMapObject.Selected = true;
                            selectedMapObjectGroup.Add(rootMapObject);
                        }
                        else
                        {
                            // solid we hit was already selected so we set action to SelectMultiples
                            currentAction = VertexToolActionType.SelectMultiple;
                        }
                    }
                }

                mouseDownPos = mouseCurPos;
            }

            return true;
        }

        public override bool OnMouseMove(Point mouseCurPos, Point mousePrevPos, BaseViewport viewport)
        {
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE)
            {
                controller.SetCursor(Cursors.Cross);

                if (viewport.IsButtonHeld(BaseViewport.ViewportButtons.LEFT))
                {
                    Vector3 mouseDownPosition = new Vector3(mouseDownPos.X, mouseDownPos.Y, 0);
                    Vector3 currentMousePosition = new Vector3(mouseCurPos.X, mouseCurPos.Y, 0);

                    Vector3 currentMouseWorld = viewport.ViewportToWorld(currentMousePosition);
                    Vector3 snappedCurrentMouseWorld = GeneralUtility.SnapToGrid(currentMouseWorld, viewport.GridSize);
                    mouseHasMoved = (currentMousePosition - mouseDownPosition).Length > GeneralUtility.Epsilon;

                    switch (currentAction)
                    {
                        case VertexToolActionType.SelectMultiple:
                            coverBlanket.Reset();
                            coverBlanket.Grow(mouseDownPosition);
                            coverBlanket.Grow(currentMousePosition);
                            break;
                        case VertexToolActionType.SelectDrag:
                            Vector3 delta = snappedCurrentMouseWorld - dragStartPosition;
                            if (delta != Vector3.Zero)
                            {
                                TranslateVertices(delta);
                            }
                            dragStartPosition = snappedCurrentMouseWorld;
                            break;
                    }
                }
            }
            else
            {
                controller.SetCursor(Cursors.Default);
            }

            return false;
        }

        public override bool OnMouseUp(Point mouseCurPos, MouseButtons button, BaseViewport viewport)
        {
            if (viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE
                && button == MouseButtons.Left)
            {
                // ON MOUSE UP
                // -- action == selectDrag && NOT control pressed && if we did not move the mouse
                // -- and selected new vertices we select them (adding them to existing selection)
                switch (currentAction)
                {
                    case VertexToolActionType.SelectMultiple:
                        if(mouseHasMoved)
                            SelectHandlesWithBlanket(viewport, coverBlanket);
                        else
                            ClearAllSelectedVertices();
                        break;
                    case VertexToolActionType.SelectDrag:
                        bool isControlPressed = modifierKey == Keys.Control;
                        bool anySelected = newHandlesHit.All((pair) => pair.Value.Any((index) => pair.Key.SelectedVertices.Contains(index)));

                        if (!isControlPressed && !mouseHasMoved && anySelected)
                        {
                            foreach (KeyValuePair<Solid, List<int>> pair in newHandlesHit)
                            {
                                pair.Value.ForEach((index) =>
                                {
                                    if (!pair.Key.SelectedVertices.Contains(index))
                                        pair.Key.SelectedVertices.Add(index);
                                });
                            }
                        }
                        
                        break;
                }

                coverBlanket.Reset();
                coverBlanket.Grow(Vector3.Zero);

                mouseHasMoved = false;
                currentAction = VertexToolActionType.None;
            }

            return false;
        }

        private void TranslateVertices(Vector3 delta)
        {
            CustomOperation operation = new CustomOperation();
            operation.OnSolid += (solid) =>
            {
                solid.SelectedVertices.ForEach((index) =>
                {
                    solid.VertexPositions[index] += delta;

                });

                if (solid.SelectedVertices.Count > 0)
                {
                    solid.RegenerateBounds();
                    solid.CalculateNormals();
                }
            };

            operation.OnMapObjectGroup += (mapObjectGroup) =>
            {
                mapObjectGroup.MapObjectList.ForEach((mapObject) => mapObject.PerformOperation(operation));
                mapObjectGroup.RegenerateBounds();
            };

            controller.Selection.PerformOperation(operation);
        }

        private void SelectHandlesWithBlanket(BaseViewport viewport, AABB blanket)
        {
            Dictionary<Solid, List<int>> newSelections = new Dictionary<Solid, List<int>>();
            bool controlKeyPressed = modifierKey == Keys.Control;
            Handle tempHandle = new Handle();
            int handlesHit = 0;

            DoSolidAction(controller.Selection, (solid) =>
            {
                int index = 0;
                solid.VertexPositions.ForEach((position) =>
                {
                    CreateNdcHandle(position, viewport, ref tempHandle);
                    if (DoesBlanketIntersectsHandle(blanket, tempHandle, viewport))
                    {
                        handlesHit++;
                        bool isSelected = solid.SelectedVertices.Contains(index);
                        if (controlKeyPressed)
                        {
                            if (isSelected)
                            {
                                solid.SelectedVertices.Remove(index);
                            }
                            else
                            {
                                solid.SelectedVertices.Add(index);
                            }   
                        }
                        else
                        {
                            if (!newSelections.ContainsKey(solid))
                                newSelections[solid] = new List<int>();

                            newSelections[solid].Add(index);
                        }
                    }
                    index++;
                });
            });

            // if we hit zero indices we deselect all vertices
            if (handlesHit <= 0)
            {
                ClearAllSelectedVertices();
            }
            else if (newSelections.Count > 0)
            {
                ClearAllSelectedVertices();

                foreach (Solid solid in newSelections.Keys)
                {
                    solid.SelectedVertices.AddRange(newSelections[solid]);
                }
            }
        }

        private Dictionary<Solid, List<int>> GetHandlesHitWithMouse(BaseViewport viewport, Point mouseCurPos)
        {
            Dictionary<Solid, List<int>> newSelections = new Dictionary<Solid, List<int>>();
            Handle tempHandle = new Handle();
            DoSolidAction(controller.Selection, (solid) =>
            {
                int vertices = solid.VertexPositions.Count;
                for (int i = 0; i < vertices; i++)
                {
                    CreateNdcHandle(solid.VertexPositions[i], viewport, ref tempHandle);
                    if (IsHandleHit(tempHandle, mouseCurPos, viewport))
                    {
                        if (!newSelections.ContainsKey(solid))
                            newSelections[solid] = new List<int>();

                        newSelections[solid].Add(i);
                    }
                }
            });

            return newSelections;
        }

        private void SetSelectDragAction(Vector3 startPosition, BaseViewport viewport)
        {
            if(viewport.ViewportType != BaseViewport.ViewportTypes.PERSPECTIVE)
            {
                currentAction = VertexToolActionType.SelectDrag;
                Matrix4 toViewportMatrix = viewport.Camera.GetWorldMatrix().ClearTranslation();
                dragStartPosition = startPosition * (toViewportMatrix.Row0.Xyz + toViewportMatrix.Row1.Xyz);
            }
        }

        private bool IsHandleHit(Handle handle, Point point, BaseViewport viewport)
        {
            int width = viewport.Width;
            int height = viewport.Height;

            // convert ndc space handle to screenspace
            int minX = (int)((1 + handle.TopLeft.X) * width / 2.0f + 0.5f);
            int minY = (int)((1 - handle.TopLeft.Y) * height / 2.0f + 0.5f);
            int maxX = (int)((1 + handle.BottomRight.X) * width / 2.0f + 0.5f);
            int maxY = (int)((1 - handle.BottomRight.Y) * height / 2.0f + 0.5f);

            return point.X >= minX && point.X <= maxX
                && point.Y >= minY && point.Y <= maxY;
        }

        private void CreateNdcHandle(Vector3 position, BaseViewport viewport, ref Handle handle)
        {
            Vector3 handleDimensions = new Vector3((float)HandleSize / viewport.Width, (float)HandleSize / viewport.Height, 0.0f);
            Matrix4 vpMatrix = viewport.Camera.GetViewMatrix() * viewport.Camera.GetProjMatrix();

            Vector4 vec4Pos = new Vector4(position, 1.0f) * vpMatrix;
            Vector3 pos = vec4Pos.Xyz / vec4Pos.W;

            handle.BottomLeft = pos + (-Vector3.UnitX - Vector3.UnitY) * handleDimensions;
            handle.BottomRight = pos + (Vector3.UnitX - Vector3.UnitY) * handleDimensions;
            handle.TopRight = pos + (Vector3.UnitX + Vector3.UnitY) * handleDimensions;
            handle.TopLeft = pos + (-Vector3.UnitX + Vector3.UnitY) * handleDimensions;
        }

        private bool DoesBlanketIntersectsHandle(AABB blanket, Handle handle, BaseViewport viewport)
        {
            int width = viewport.Width;
            int height = viewport.Height;

            //Matrix4 toViewportMatrix = viewport.Camera.GetViewMatrix().ClearTranslation();
            Vector3 min = blanket.Min;//.TransformL(toViewportMatrix);
            Vector3 max = blanket.Max;//.TransformL(toViewportMatrix);

            // convert ndc space handle to screenspace
            float minX = (1 + handle.TopLeft.X) * width / 2.0f;
            float minY = (1 - handle.TopLeft.Y) * height / 2.0f;
            float maxX = (1 + handle.BottomRight.X) * width / 2.0f;
            float maxY = (1 - handle.BottomRight.Y) * height / 2.0f;

            return !(min.X > maxX ||
                     max.X < minX ||
                     min.Y > maxY ||
                     max.Y < minY);
        }

        private void ClearAllSelectedVertices()
        {
            DoSolidAction(controller.Selection, (solid) =>
            {
                solid.SelectedVertices.Clear();
            });
        }

        private void DoSolidAction(MapObject subjectMapObject, Action<Solid> action)
        {
            CustomOperation operation = new CustomOperation();
            operation.OnSolid += (solid) => action?.Invoke(solid);
            operation.OnMapObjectGroup += (mapObjectGroup) => mapObjectGroup.MapObjectList.ForEach((solid) => solid.PerformOperation(operation));

            subjectMapObject.PerformOperation(operation);
        }
    }
}
