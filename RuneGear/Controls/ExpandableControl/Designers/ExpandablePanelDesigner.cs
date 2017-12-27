using System.ComponentModel;
using System.Windows.Forms.Design;

namespace RuneGear.Controls.ExpandableControl.Designers
{
    public class ExpandablePanelDesigner : ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            ExpandablePanel panel = component as ExpandablePanel;
            if (panel != null)
            {
                EnableDesignMode(((ExpandablePanel) Control).WorkingArea, "WorkingArea");
            }
        }
    }
}
