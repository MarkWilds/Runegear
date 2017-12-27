using System.Drawing;
using System.Windows.Forms;
using RuneGear.General;
using RuneGear.General.Viewport;
using RuneGear.MapObjects;

namespace RuneGear.Tools
{
    public abstract class BaseTool
    {
        protected IEditorController controller;

        public virtual void Initialize(IEditorController controller) { }
        public virtual void Deinitialize() { }
        public virtual void OnRender(Graphics.Graphics graphics, BaseViewport viewport) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }
        public virtual void OnKeyUp(KeyEventArgs e) { }
        public virtual bool OnMouseDown(Point mouseCurPos, MouseButtons button, BaseViewport viewport) { return false; }
        public virtual bool OnMouseMove(Point mouseCurPos, Point mousePrevPos, BaseViewport viewport) { return false; }
        public virtual bool OnMouseUp(Point mouseCurPos, MouseButtons button, BaseViewport viewport) { return false; }
        
        protected void SetAllFacesSelected(bool condition)
        {
            CustomOperation faceSelectionOperation = new CustomOperation();
            faceSelectionOperation.OnSolid += (solid) => solid.Selected = condition;
            faceSelectionOperation.OnMapObjectGroup += (mapObjectGroup) =>
            {
                mapObjectGroup.MapObjectList.ForEach((group) => group.PerformOperation(faceSelectionOperation));
            };
            controller.Selection.PerformOperation(faceSelectionOperation);
        }
    }
}
