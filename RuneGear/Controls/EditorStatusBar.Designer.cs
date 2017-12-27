namespace RuneGear.Controls
{
    partial class EditorStatusBar
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
            components = new System.ComponentModel.Container();
            this.editorStatusbarText = new System.Windows.Forms.ToolStripStatusLabel();
            this.editorStatusbarGrid = new System.Windows.Forms.ToolStripStatusLabel();
            this.editorStatusbarZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.editorStatusbarPositon = new System.Windows.Forms.ToolStripStatusLabel();
            this.editorStatusbarDimensions = new System.Windows.Forms.ToolStripStatusLabel();

            this.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorStatusbarText,
            this.editorStatusbarGrid,
            this.editorStatusbarZoom,
            this.editorStatusbarPositon,
            this.editorStatusbarDimensions});

            // 
            // editorStatusbarText
            // 
            this.editorStatusbarText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editorStatusbarText.Name = "editorStatusbarText";
            this.editorStatusbarText.AutoSize = false;
            this.editorStatusbarText.Size = new System.Drawing.Size(256, 17);
            this.editorStatusbarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editorStatusbarText.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            
            // 
            // editorStatusbarGrid
            // 
            this.editorStatusbarGrid.Name = "editorStatusbarGrid";
            this.editorStatusbarGrid.AutoSize = false;
            this.editorStatusbarGrid.Size = new System.Drawing.Size(96, 17);
            this.editorStatusbarGrid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.editorStatusbarGrid.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            // 
            // editorStatusbarZoom
            // 
            this.editorStatusbarZoom.Name = "editorStatusbarZoom";
            this.editorStatusbarZoom.AutoSize = false;
            this.editorStatusbarZoom.Size = new System.Drawing.Size(96, 17);
            this.editorStatusbarZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.editorStatusbarZoom.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            // 
            // editorStatusbarPositon
            // 
            this.editorStatusbarPositon.Name = "editorStatusbarPositon";
            this.editorStatusbarPositon.AutoSize = false;
            this.editorStatusbarPositon.Size = new System.Drawing.Size(196, 17);
            this.editorStatusbarPositon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.editorStatusbarPositon.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            // 
            // editorStatusbarDimensions
            // 
            this.editorStatusbarDimensions.Name = "editorStatusbarDimensions";
            this.editorStatusbarDimensions.AutoSize = false;
            this.editorStatusbarDimensions.Size = new System.Drawing.Size(196, 17);
            this.editorStatusbarDimensions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.editorStatusbarDimensions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel editorStatusbarText;
        private System.Windows.Forms.ToolStripStatusLabel editorStatusbarGrid;
        private System.Windows.Forms.ToolStripStatusLabel editorStatusbarZoom;
        private System.Windows.Forms.ToolStripStatusLabel editorStatusbarPositon;
        private System.Windows.Forms.ToolStripStatusLabel editorStatusbarDimensions;
    }
}
