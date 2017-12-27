namespace RuneGear.Controls
{
    partial class TextureProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureProperties));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TreatAsOneCheckbox = new System.Windows.Forms.CheckBox();
            this.rightButton = new System.Windows.Forms.Button();
            this.bottomButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.centerButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.fitButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.rotationNumeric = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.vShiftNumeric = new System.Windows.Forms.NumericUpDown();
            this.vScaleNumeric = new System.Windows.Forms.NumericUpDown();
            this.uShiftNumeric = new System.Windows.Forms.NumericUpDown();
            this.uScaleNumeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textureLockButton = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rotationNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vShiftNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vScaleNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uShiftNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uScaleNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textureLockButton);
            this.groupBox1.Controls.Add(this.TreatAsOneCheckbox);
            this.groupBox1.Controls.Add(this.rightButton);
            this.groupBox1.Controls.Add(this.bottomButton);
            this.groupBox1.Controls.Add(this.leftButton);
            this.groupBox1.Controls.Add(this.centerButton);
            this.groupBox1.Controls.Add(this.topButton);
            this.groupBox1.Controls.Add(this.fitButton);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.resetButton);
            this.groupBox1.Controls.Add(this.rotationNumeric);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.vShiftNumeric);
            this.groupBox1.Controls.Add(this.vScaleNumeric);
            this.groupBox1.Controls.Add(this.uShiftNumeric);
            this.groupBox1.Controls.Add(this.uScaleNumeric);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 271);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modify";
            // 
            // TreatAsOneCheckbox
            // 
            this.TreatAsOneCheckbox.Enabled = false;
            this.TreatAsOneCheckbox.Location = new System.Drawing.Point(6, 139);
            this.TreatAsOneCheckbox.Name = "TreatAsOneCheckbox";
            this.TreatAsOneCheckbox.Size = new System.Drawing.Size(86, 17);
            this.TreatAsOneCheckbox.TabIndex = 36;
            this.TreatAsOneCheckbox.Text = "Treat as one";
            this.TreatAsOneCheckbox.UseVisualStyleBackColor = true;
            // 
            // rightButton
            // 
            this.rightButton.Location = new System.Drawing.Point(70, 206);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(26, 26);
            this.rightButton.TabIndex = 35;
            this.rightButton.Tag = "right";
            this.rightButton.Text = "R";
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.Click += new System.EventHandler(this.OnJustifyButtonClicked);
            // 
            // bottomButton
            // 
            this.bottomButton.Location = new System.Drawing.Point(38, 238);
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.Size = new System.Drawing.Size(26, 26);
            this.bottomButton.TabIndex = 34;
            this.bottomButton.Tag = "bottom";
            this.bottomButton.Text = "B";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.OnJustifyButtonClicked);
            // 
            // leftButton
            // 
            this.leftButton.Location = new System.Drawing.Point(6, 206);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(26, 26);
            this.leftButton.TabIndex = 33;
            this.leftButton.Tag = "left";
            this.leftButton.Text = "L";
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.OnJustifyButtonClicked);
            // 
            // centerButton
            // 
            this.centerButton.Location = new System.Drawing.Point(38, 206);
            this.centerButton.Name = "centerButton";
            this.centerButton.Size = new System.Drawing.Size(26, 26);
            this.centerButton.TabIndex = 32;
            this.centerButton.Tag = "center";
            this.centerButton.Text = "C";
            this.centerButton.UseVisualStyleBackColor = true;
            this.centerButton.Click += new System.EventHandler(this.OnJustifyButtonClicked);
            // 
            // topButton
            // 
            this.topButton.Location = new System.Drawing.Point(38, 174);
            this.topButton.Name = "topButton";
            this.topButton.Size = new System.Drawing.Size(26, 26);
            this.topButton.TabIndex = 31;
            this.topButton.Tag = "top";
            this.topButton.Text = "T";
            this.topButton.UseVisualStyleBackColor = true;
            this.topButton.Click += new System.EventHandler(this.OnJustifyButtonClicked);
            // 
            // fitButton
            // 
            this.fitButton.Location = new System.Drawing.Point(6, 174);
            this.fitButton.Name = "fitButton";
            this.fitButton.Size = new System.Drawing.Size(26, 26);
            this.fitButton.TabIndex = 30;
            this.fitButton.Tag = "fit";
            this.fitButton.Text = "Fit";
            this.fitButton.UseVisualStyleBackColor = true;
            this.fitButton.Click += new System.EventHandler(this.OnJustifyButtonClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Justify:";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(58, 97);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(65, 37);
            this.resetButton.TabIndex = 28;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.OnResetButtonPressed);
            // 
            // rotationNumeric
            // 
            this.rotationNumeric.Location = new System.Drawing.Point(6, 113);
            this.rotationNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotationNumeric.Name = "rotationNumeric";
            this.rotationNumeric.Size = new System.Drawing.Size(47, 20);
            this.rotationNumeric.TabIndex = 27;
            this.rotationNumeric.Tag = "rotation";
            this.rotationNumeric.ValueChanged += new System.EventHandler(this.OnTexturePropertiesChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Rotation:";
            // 
            // vShiftNumeric
            // 
            this.vShiftNumeric.DecimalPlaces = 1;
            this.vShiftNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.vShiftNumeric.Location = new System.Drawing.Point(78, 65);
            this.vShiftNumeric.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.vShiftNumeric.Minimum = new decimal(new int[] {
            4096,
            0,
            0,
            -2147483648});
            this.vShiftNumeric.Name = "vShiftNumeric";
            this.vShiftNumeric.Size = new System.Drawing.Size(45, 20);
            this.vShiftNumeric.TabIndex = 25;
            this.vShiftNumeric.Tag = "vshift";
            this.vShiftNumeric.ValueChanged += new System.EventHandler(this.OnTexturePropertiesChanged);
            // 
            // vScaleNumeric
            // 
            this.vScaleNumeric.DecimalPlaces = 1;
            this.vScaleNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.vScaleNumeric.Location = new System.Drawing.Point(22, 65);
            this.vScaleNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.vScaleNumeric.Name = "vScaleNumeric";
            this.vScaleNumeric.Size = new System.Drawing.Size(45, 20);
            this.vScaleNumeric.TabIndex = 24;
            this.vScaleNumeric.Tag = "vscale";
            this.vScaleNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.vScaleNumeric.ValueChanged += new System.EventHandler(this.OnTexturePropertiesChanged);
            // 
            // uShiftNumeric
            // 
            this.uShiftNumeric.DecimalPlaces = 1;
            this.uShiftNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.uShiftNumeric.Location = new System.Drawing.Point(78, 39);
            this.uShiftNumeric.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.uShiftNumeric.Minimum = new decimal(new int[] {
            4096,
            0,
            0,
            -2147483648});
            this.uShiftNumeric.Name = "uShiftNumeric";
            this.uShiftNumeric.Size = new System.Drawing.Size(45, 20);
            this.uShiftNumeric.TabIndex = 23;
            this.uShiftNumeric.Tag = "ushift";
            this.uShiftNumeric.ValueChanged += new System.EventHandler(this.OnTexturePropertiesChanged);
            // 
            // uScaleNumeric
            // 
            this.uScaleNumeric.DecimalPlaces = 1;
            this.uScaleNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.uScaleNumeric.Location = new System.Drawing.Point(22, 39);
            this.uScaleNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.uScaleNumeric.Name = "uScaleNumeric";
            this.uScaleNumeric.Size = new System.Drawing.Size(45, 20);
            this.uScaleNumeric.TabIndex = 22;
            this.uScaleNumeric.Tag = "uscale";
            this.uScaleNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.uScaleNumeric.ValueChanged += new System.EventHandler(this.OnTexturePropertiesChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "v:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "u:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Scale:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Shift:";
            // 
            // textureLockButton
            // 
            this.textureLockButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.textureLockButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("textureLockButton.BackgroundImage")));
            this.textureLockButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.textureLockButton.Location = new System.Drawing.Point(97, 239);
            this.textureLockButton.MaximumSize = new System.Drawing.Size(26, 26);
            this.textureLockButton.Name = "textureLockButton";
            this.textureLockButton.Size = new System.Drawing.Size(26, 26);
            this.textureLockButton.TabIndex = 37;
            this.textureLockButton.Tag = "textureLock";
            this.textureLockButton.UseVisualStyleBackColor = true;
            this.textureLockButton.Click += new System.EventHandler(this.OnTexturePropertiesChanged);
            // 
            // TextureProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "TextureProperties";
            this.Size = new System.Drawing.Size(135, 276);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rotationNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vShiftNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vScaleNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uShiftNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uScaleNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button centerButton;
        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.Button fitButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.NumericUpDown rotationNumeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown vShiftNumeric;
        private System.Windows.Forms.NumericUpDown vScaleNumeric;
        private System.Windows.Forms.NumericUpDown uShiftNumeric;
        private System.Windows.Forms.NumericUpDown uScaleNumeric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox TreatAsOneCheckbox;
        private System.Windows.Forms.CheckBox textureLockButton;
    }
}
