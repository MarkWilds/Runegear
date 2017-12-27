namespace RuneGear.Controls
{
    partial class SolidProperties
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hiddenSolid = new System.Windows.Forms.CheckBox();
            this.detailSolid = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hiddenSolid);
            this.groupBox1.Controls.Add(this.detailSolid);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // hiddenSolid
            // 
            this.hiddenSolid.AutoSize = true;
            this.hiddenSolid.Location = new System.Drawing.Point(6, 43);
            this.hiddenSolid.Name = "hiddenSolid";
            this.hiddenSolid.Size = new System.Drawing.Size(86, 17);
            this.hiddenSolid.TabIndex = 1;
            this.hiddenSolid.Tag = "hidden";
            this.hiddenSolid.Text = "Hidden Solid";
            this.hiddenSolid.UseVisualStyleBackColor = true;
            this.hiddenSolid.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // detailSolid
            // 
            this.detailSolid.AutoSize = true;
            this.detailSolid.Location = new System.Drawing.Point(6, 19);
            this.detailSolid.Name = "detailSolid";
            this.detailSolid.Size = new System.Drawing.Size(79, 17);
            this.detailSolid.TabIndex = 0;
            this.detailSolid.Tag = "detail";
            this.detailSolid.Text = "Detail Solid";
            this.detailSolid.UseVisualStyleBackColor = true;
            this.detailSolid.Click += new System.EventHandler(this.CheckedChanged);
            // 
            // SolidProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "SolidProperties";
            this.Size = new System.Drawing.Size(135, 64);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox hiddenSolid;
        private System.Windows.Forms.CheckBox detailSolid;
    }
}
