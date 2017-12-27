namespace RuneGear.Controls
{
    partial class TextureSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureSelector));
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.replaceButton = new System.Windows.Forms.Button();
            this.textureName = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // texturePreview
            // 
            this.texturePreview.BackColor = System.Drawing.Color.Black;
            this.texturePreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("texturePreview.BackgroundImage")));
            this.texturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.texturePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.texturePreview.InitialImage = null;
            this.texturePreview.Location = new System.Drawing.Point(4, 22);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(128, 128);
            this.texturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.texturePreview.TabIndex = 3;
            this.texturePreview.TabStop = false;
            this.texturePreview.Click += new System.EventHandler(this.OnBrowseButtonClicked);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(4, 178);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(57, 23);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            // 
            // replaceButton
            // 
            this.replaceButton.Location = new System.Drawing.Point(62, 178);
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Size = new System.Drawing.Size(70, 23);
            this.replaceButton.TabIndex = 2;
            this.replaceButton.Text = "Replace";
            this.replaceButton.UseVisualStyleBackColor = true;
            this.replaceButton.Visible = false;
            // 
            // textureName
            // 
            this.textureName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textureName.AutoSize = true;
            this.textureName.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textureName.ForeColor = System.Drawing.Color.Black;
            this.textureName.Location = new System.Drawing.Point(3, 5);
            this.textureName.Name = "textureName";
            this.textureName.Size = new System.Drawing.Size(67, 14);
            this.textureName.TabIndex = 4;
            this.textureName.Text = "No textures";
            this.textureName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(4, 152);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(128, 23);
            this.browseButton.TabIndex = 5;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.OnBrowseButtonClicked);
            // 
            // TextureSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.textureName);
            this.Controls.Add(this.replaceButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.texturePreview);
            this.Name = "TextureSelector";
            this.Size = new System.Drawing.Size(135, 204);
            this.Load += new System.EventHandler(this.TextureSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox texturePreview;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button replaceButton;
        private System.Windows.Forms.Label textureName;
        private System.Windows.Forms.Button browseButton;
    }
}
