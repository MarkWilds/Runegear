namespace RuneGear.Forms
{
    partial class TextureBrowserDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureBrowserDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textureTreeView = new System.Windows.Forms.TreeView();
            this.textureListView = new System.Windows.Forms.ListView();
            this.textureInformationPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            this.searchLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.textureInformationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textureTreeView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textureListView, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textureInformationPanel, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.14966F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.85034F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1270, 741);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textureTreeView
            // 
            this.textureTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureTreeView.Location = new System.Drawing.Point(7, 7);
            this.textureTreeView.Name = "textureTreeView";
            this.tableLayoutPanel1.SetRowSpan(this.textureTreeView, 2);
            this.textureTreeView.Size = new System.Drawing.Size(204, 727);
            this.textureTreeView.TabIndex = 0;
            this.textureTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.textureTreeView_AfterSelect);
            // 
            // textureListView
            // 
            this.textureListView.BackColor = System.Drawing.Color.Black;
            this.textureListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureListView.Location = new System.Drawing.Point(217, 7);
            this.textureListView.MultiSelect = false;
            this.textureListView.Name = "textureListView";
            this.textureListView.OwnerDraw = true;
            this.textureListView.Size = new System.Drawing.Size(1046, 684);
            this.textureListView.TabIndex = 1;
            this.textureListView.UseCompatibleStateImageBehavior = false;
            this.textureListView.VirtualMode = true;
            this.textureListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.textureListView_ItemSelectionChanged);
            this.textureListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.textureListView_RetrieveVirtualItem);
            this.textureListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textureListView_MouseDoubleClick);
            // 
            // textureInformationPanel
            // 
            this.textureInformationPanel.Controls.Add(this.button1);
            this.textureInformationPanel.Controls.Add(this.selectButton);
            this.textureInformationPanel.Controls.Add(this.searchLabel);
            this.textureInformationPanel.Controls.Add(this.textBox1);
            this.textureInformationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureInformationPanel.Location = new System.Drawing.Point(217, 697);
            this.textureInformationPanel.Name = "textureInformationPanel";
            this.textureInformationPanel.Size = new System.Drawing.Size(1046, 37);
            this.textureInformationPanel.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(881, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectButton.Location = new System.Drawing.Point(962, 5);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 2;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(3, 11);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(44, 13);
            this.searchLabel.TabIndex = 1;
            this.searchLabel.Text = "Search:";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(53, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(265, 20);
            this.textBox1.TabIndex = 0;
            // 
            // TextureBrowserDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 741);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "TextureBrowserDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Texture Browser";
            this.Load += new System.EventHandler(this.TextureBrowserDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.textureInformationPanel.ResumeLayout(false);
            this.textureInformationPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView textureTreeView;
        private System.Windows.Forms.ListView textureListView;
        private System.Windows.Forms.Panel textureInformationPanel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button button1;
    }
}