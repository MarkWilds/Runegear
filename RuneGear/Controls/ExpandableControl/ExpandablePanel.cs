using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RuneGear.Controls.ExpandableControl
{
    /// <summary>
    /// The ExpandCollapsePanel control displays a header that has a collapsible window that displays content.
    /// </summary>
    [Designer(typeof(Designers.ExpandablePanelDesigner))]
    public partial class ExpandablePanel : UserControl
    {
        /// <summary>
        /// Last stored size of panel's parent control
        /// <remarks>used for handling panel's Anchor property sets to Bottom when panel collapsed
        /// in OnSizeChanged method</remarks>
        /// </summary>
        private Size previousParentSize = Size.Empty;

        /// <summary>
        /// Height of panel in expanded state
        /// </summary>
        private int expandedHeight;

        /// <summary>
        /// Height of panel in collapsed state
        /// </summary>
        private int collapsedHeight;

        private bool isExpanded;
        public int ExpandedHeight
        {
            get { return expandedHeight; }
            set
            {
                expandedHeight = value;
                if (IsExpanded)
                {
                    Height = expandedHeight;
                }
            }
        }

        /// <summary>
        /// Set flag for expand or collapse panel content
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Category("ExpandablePanel")]
        [Description("Expand or collapse panel content. " +
                     "\r\nAttention, for correct work with resizing child controls," +
                     " please set IsExpanded to \"false\" in code (for example in your Form class constructor after InitializeComponent method) and not in Forms Designer!")]
        [Browsable(true)]
        public bool IsExpanded
        {
            get { return isExpanded; }
            set 
            { 
                isExpanded = value;
            }
        }

        /// <summary>
        /// Header of panel
        /// </summary>
        [Category("ExpandablePanel")]
        [Description("Header of panel.")]
        [Browsable(true)]
        public string Caption
        {
            get { return expandableButton.Text; }
            set { expandableButton.Text = value; }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FlowLayoutPanel WorkingArea => contentPanel;

        /// <summary>
        /// Occurs when the panel has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapsePanel")]
        [Description("Occurs when the panel has expanded or collapsed.")]
        [Browsable(true)]
        public event EventHandler<ExpandableEventArgs> ExpandCollapse;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExpandablePanel()
        {
            InitializeComponent();

            collapsedHeight = expandableButton.Location.Y + expandableButton.Size.Height + expandableButton.Margin.Bottom;
            contentPanel.Resize += OnContentSizeChanged;
            IsExpanded = true;
        }

        /// <summary>
        /// Expand panel content
        /// </summary>
        protected virtual void Expand()
        {
//            AutoSize = false;
            contentPanel.Visible = true;
            Size = new Size(Size.Width, expandedHeight);
        }

        public new BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Collapse panel content
        /// </summary>
        protected virtual void Collapse()
        {
//            AutoSize = true;
            contentPanel.Visible = false;
            expandedHeight = Size.Height;
            Size = new Size(Size.Width, collapsedHeight);
        }

        private void DoExpandCollapse()
        {
            if (IsExpanded)
            {
                Expand();
            }
            else
            {
                Collapse();
            }
        }

        private void AnchorContent()
        {
            // resize parent based on content
            if (IsExpanded)
            {
                int horizontalOffset = Padding.Left + Padding.Right + contentPanel.Margin.Left +
                                       contentPanel.Margin.Right;
                int verticalOffset = contentPanel.Margin.Bottom + Padding.Bottom + contentPanel.Location.Y;

                Size = new Size(contentPanel.Width + horizontalOffset, contentPanel.Height + verticalOffset);
            }
        }

        private void OnContentSizeChanged(object sender, EventArgs e)
        {
            AnchorContent();
        }

        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // we always manually scale expand-collapse button for filling the horizontal space of panel:
            int marginTimesTwo = expandableButton.Location.X * 2;
            expandableButton.Size = new Size(ClientSize.Width - marginTimesTwo, expandableButton.Height);

            if (!IsExpanded // if panel collapsed
                && ((Anchor & AnchorStyles.Bottom) != 0) //and panel's Anchor property sets to Bottom
                && Size.Height != collapsedHeight // and panel height is changed (it could happens only if parent control just has resized)
                && Parent != null) // and panel has the parent control
            {
                // main, calculate the parent control resize diff and add it to expandedHeight value:
                expandedHeight += Parent.Height - previousParentSize.Height;

                // reset resized height (by base.OnSizeChanged anchor.Bottom handling) to collapsedHeight value:
                Size = new Size(Size.Width, collapsedHeight);
            }

            // store previous size of parent control (however we need only height)
            if (Parent != null)
                previousParentSize = Parent.Size;

            Invalidate();
        }

        private void expandableButton_Click(object sender, EventArgs e)
        {
            IsExpanded = !IsExpanded;
            DoExpandCollapse();
            expandableButton.Invalidate();
            ExpandCollapse?.Invoke(this, new ExpandableEventArgs(IsExpanded));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Pen borderPen = new Pen(Color.FromArgb(255, 190, 190, 190));
            SolidBrush brush = new SolidBrush(SystemColors.Control);
            System.Drawing.Graphics graphics = e.Graphics;

            // draw border
            int borderOffset = 12;
            Rectangle borderBounds = new Rectangle(0, borderOffset, ClientRectangle.Width - 1,
                ClientRectangle.Height - borderOffset - 1);

            graphics.DrawRectangle(borderPen, borderBounds);

            borderPen.Dispose();
            brush.Dispose();
        }

        private void expandableButton_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Pen borderPen = new Pen(Color.FromArgb(255, 82, 82, 82));
            Color backgroundColor = IsExpanded ? Color.FromArgb(255, 250, 250, 250) : SystemColors.Control;

            Rectangle buttonRectangle = expandableButton.ClientRectangle;
            Rectangle bounds = new Rectangle(buttonRectangle.X, buttonRectangle.Y,
                buttonRectangle.Width - 1, buttonRectangle.Height - 1);

            graphics.Clear(backgroundColor);
            graphics.DrawRectangle(borderPen, bounds);
            DrawIcon(graphics, bounds);
            DrawCaption(graphics, bounds);

            borderPen.Dispose();
        }

        private void DrawIcon(System.Drawing.Graphics canvas, Rectangle bounds)
        {
            int halfHeight = bounds.Height / 2;
            int lineSize = 4;
            int xStart = Padding.Left + 8;
            int xMiddle = Padding.Left + 8 + lineSize / 2;
            int yMiddle = halfHeight;

            canvas.DrawLine(Pens.Black, xStart, yMiddle, xStart + lineSize, yMiddle);
            if (!isExpanded)
                canvas.DrawLine(Pens.Black, xMiddle, yMiddle - lineSize / 2, xMiddle, yMiddle + lineSize / 2);
        }

        private void DrawCaption(System.Drawing.Graphics canvas, Rectangle bounds)
        {
            SizeF textSize = canvas.MeasureString(Caption, Font);
            int halfTextWidth = (int)textSize.Width / 2;
            int halfTextHeight = (int)textSize.Height / 2;
            canvas.DrawString(Caption, Font, Brushes.Black,
                bounds.Width / 2 - halfTextWidth, bounds.Height / 2 - halfTextHeight - 1);
        }
    }
}
