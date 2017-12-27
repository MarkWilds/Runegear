namespace RuneGear.Controls.ExpandableControl
{
    partial class ExpandablePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.contentPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.expandableButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.AutoSize = true;
            this.contentPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.contentPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.contentPanel.Location = new System.Drawing.Point(3, 31);
            this.contentPanel.MinimumSize = new System.Drawing.Size(143, 40);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(143, 40);
            this.contentPanel.TabIndex = 1;
            this.contentPanel.WrapContents = false;
            // 
            // expandableButton
            // 
            this.expandableButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.expandableButton.Location = new System.Drawing.Point(5, 2);
            this.expandableButton.Name = "expandableButton";
            this.expandableButton.Size = new System.Drawing.Size(140, 23);
            this.expandableButton.TabIndex = 2;
            this.expandableButton.Text = "nocaption";
            this.expandableButton.UseVisualStyleBackColor = true;
            this.expandableButton.Click += new System.EventHandler(this.expandableButton_Click);
            this.expandableButton.Paint += new System.Windows.Forms.PaintEventHandler(this.expandableButton_Paint);
            // 
            // ExpandablePanel
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.expandableButton);
            this.Controls.Add(this.contentPanel);
            this.MinimumSize = new System.Drawing.Size(150, 32);
            this.Name = "ExpandablePanel";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(150, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel contentPanel;
        private System.Windows.Forms.Button expandableButton;
    }
}
