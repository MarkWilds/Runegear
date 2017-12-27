using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL4;
using RuneGear.General;
using RuneGear.General.Camera;
using RuneGear.General.Viewport;
using RuneGear.MapObjects;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.MapObjects.SolidMapObject.Operations;
using RuneGear.Utilities;

namespace RuneGear.Tools
{
    public class SolidFaceTool : BaseTool
    {
        private Keys modifierKey;

        public override void Initialize(IEditorController controller)
        {
            this.controller = controller;
            controller.CurrentSolidManipulationMode = SolidManipulationMode.Face;

            controller.RubberBand.ShowGrabhandles = false;
            controller.SetCursor(Cursors.Default);
        }

        public override void Deinitialize()
        {
            controller.RubberBand.ShowGrabhandles = true;
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
            SolidRenderOperation render = new SolidRenderOperation
            {
                Viewport = viewport,
                Graphics = graphics
            };

            // Draw all solid
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

            graphics.EndDraw();

            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                GL.Disable(EnableCap.DepthTest);
        }

        public override bool OnMouseDown(Point mouseCurPos, MouseButtons button, BaseViewport viewport)
        {
            MapObjectGroup selectedMapObjectGroup = controller.Selection;
            MapObject rootMapObject = controller.GetMapObjectIfHit(mouseCurPos.X, mouseCurPos.Y, viewport);
            if (viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
            {
                if (rootMapObject == null)
                {
                    SetAllFacesSelected(false);
                    selectedMapObjectGroup.Clear();
                }
                else
                {
                    SolidFaceHitOperation operation = new SolidFaceHitOperation(viewport, mouseCurPos);
                    rootMapObject.PerformOperation(operation);

                    SolidFace hitFace = operation.hitFace;
                    if (modifierKey == Keys.Control)
                    {
                        if (!AreAnyFacesSelected(rootMapObject))
                            controller.Selection.Add(rootMapObject);

                        hitFace.Selected = !hitFace.Selected;

                        if (!AreAnyFacesSelected(rootMapObject))
                            controller.Selection.Remove(rootMapObject);
                    }
                    else if (!hitFace.Selected)
                    {
                        SetAllFacesSelected(false);
                        selectedMapObjectGroup.Clear();

                        hitFace.Selected = true;
                        selectedMapObjectGroup.Add(rootMapObject);
                    }
                }
            }
            else if(rootMapObject == null) // deselect faces when pressing in empty ortho viewport space
            {
                SetAllFacesSelected(false);
                controller.Selection.Clear();
            }
            
            controller.UpdateUserInterface();

            return true;
        }

        private bool AreAnyFacesSelected(MapObject subjectMapObject)
        {
            bool areAnySelected = false;
            CustomOperation selectionOperation = new CustomOperation();
            selectionOperation.OnSolid += (solid) =>
            {
                foreach (SolidFace face in solid.Faces)
                {
                    if (face.Selected)
                    {
                        areAnySelected = true;
                        break;
                    }
                }
            };

            selectionOperation.OnMapObjectGroup += (mapObjectGroup) =>
            {
                foreach (MapObject mapObject in mapObjectGroup.MapObjectList)
                {
                    mapObject.PerformOperation(selectionOperation);
                    if (areAnySelected)
                        break;
                }
            };

            subjectMapObject.PerformOperation(selectionOperation);

            return areAnySelected;
        }
    }
}
