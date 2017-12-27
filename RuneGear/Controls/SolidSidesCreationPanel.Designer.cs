namespace RuneGear.Controls
{
    partial class SolidSidesCreationPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericSides = new System.Windows.Forms.NumericUpDown();
            this.creationSettingsBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericSides)).BeginInit();
            this.creationSettingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sides";
            // 
            // numericSides
            // 
            this.numericSides.Location = new System.Drawing.Point(45, 17);
            this.numericSides.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericSides.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericSides.Name = "numericSides";
            this.numericSides.Size = new System.Drawing.Size(42, 20);
            this.numericSides.TabIndex = 3;
            this.numericSides.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // creationSettingsBox
            // 
            this.creationSettingsBox.Controls.Add(this.label1);
            this.creationSettingsBox.Controls.Add(this.numericSides);
            this.creationSettingsBox.Location = new System.Drawing.Point(2, 0);
            this.creationSettingsBox.Margin = new System.Windows.Forms.Padding(0);
            this.creationSettingsBox.Name = "creationSettingsBox";
            this.creationSettingsBox.Size = new System.Drawing.Size(124, 44);
            this.creationSettingsBox.TabIndex = 4;
            this.creationSettingsBox.TabStop = false;
            this.creationSettingsBox.Text = "Settings";
            // 
            // BrushSidesProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.creationSettingsBox);
            this.Name = "BrushSidesProperty";
            this.Size = new System.Drawing.Size(128, 48);
            ((System.ComponentModel.ISupportInitialize)(this.numericSides)).EndInit();
            this.creationSettingsBox.ResumeLayout(false);
            this.creationSettingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericSides;
        private System.Windows.Forms.GroupBox creationSettingsBox;
    }
}
