using System.Windows.Forms;

namespace RuneGear.Controls.ExpandableControl
{
    public partial class ExpandableFlowLayoutPanel : FlowLayoutPanel
    {
        public ExpandableFlowLayoutPanel()
        {
            InitializeComponent();
            HorizontalScroll.Enabled = false;
        }

        ///// <summary>
        ///// Handle the resize of panel
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnSizeChanged(System.EventArgs e)
        //{
        //    // for each child control
        //    foreach (Control c in Controls)
        //    {
        //        FillControlWidth(c); // scale the width to fill free horizontal space
        //    }

        //    // get all another resize stuff from base class
        //    base.OnSizeChanged(e);
        //}

        ///// <summary>
        ///// Scale the width to fill free horizontal space of current panel
        ///// </summary>
        ///// <param name="c">control for scalling</param>
        //protected void FillControlWidth(Control c)
        //{
        //    c.Size = new System.Drawing.Size(ClientSize.Width - c.Margin.Left - c.Margin.Right, c.Height);
        //}
    }
}
