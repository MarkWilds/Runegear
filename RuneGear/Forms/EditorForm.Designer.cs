using RuneGear.Controls;
using RuneGear.Controls.ExpandableControl;

namespace RuneGear.Forms
{
    partial class EditorForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.editorMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.editorViewport = new RuneGear.Controls.EditorGlViewport();
            this.propertiesControl = new System.Windows.Forms.TabControl();
            this.solidsTab = new System.Windows.Forms.TabPage();
            this.generalTabLayout = new RuneGear.Controls.ExpandableControl.ExpandableFlowLayoutPanel();
            this.brushesPanel = new RuneGear.Controls.ExpandableControl.ExpandablePanel();
            this.modeGroupbox = new System.Windows.Forms.GroupBox();
            this.modeVertexButton = new System.Windows.Forms.CheckBox();
            this.modeFaceButton = new System.Windows.Forms.CheckBox();
            this.modeBrushButton = new System.Windows.Forms.CheckBox();
            this.creationGroupbox = new System.Windows.Forms.GroupBox();
            this.creationFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.creationButtonsPanel = new RuneGear.Controls.CreationButtonsPanel();
            this.CsgGroupbox = new System.Windows.Forms.GroupBox();
            this.csgExtrudeButton = new System.Windows.Forms.CheckBox();
            this.csgHollowButton = new System.Windows.Forms.CheckBox();
            this.csgSliceButton = new System.Windows.Forms.CheckBox();
            this.csgCarveButton = new System.Windows.Forms.CheckBox();
            this.solidProperties = new RuneGear.Controls.SolidProperties();
            this.expandablePanel1 = new RuneGear.Controls.ExpandableControl.ExpandablePanel();
            this.textureSelector = new RuneGear.Controls.TextureSelector();
            this.textureProperties = new RuneGear.Controls.TextureProperties();
            this.editorStatusbar = new RuneGear.Controls.EditorStatusBar();
            this.toolbarNew = new System.Windows.Forms.ToolStripButton();
            this.toolbarOpen = new System.Windows.Forms.ToolStripButton();
            this.toolbarSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarCut = new System.Windows.Forms.ToolStripButton();
            this.toolbarCopy = new System.Windows.Forms.ToolStripButton();
            this.toolbarPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarUndo = new System.Windows.Forms.ToolStripButton();
            this.toolbarRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarWireframe = new System.Windows.Forms.ToolStripButton();
            this.toolbarSolid = new System.Windows.Forms.ToolStripButton();
            this.toolbarTextured = new System.Windows.Forms.ToolStripButton();
            this.toolbarTexturedLighted = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarGroup = new System.Windows.Forms.ToolStripButton();
            this.toolbarUngroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarSettings = new System.Windows.Forms.ToolStripButton();
            this.editorToolbar = new System.Windows.Forms.ToolStrip();
            this.toolbarAlignLeft = new System.Windows.Forms.ToolStripButton();
            this.toolbarAlignRight = new System.Windows.Forms.ToolStripButton();
            this.toolbarAlignTop = new System.Windows.Forms.ToolStripButton();
            this.toolbarAlignBottom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarFlipHorizontal = new System.Windows.Forms.ToolStripButton();
            this.toolbarFlipVertical = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarTextureLock = new System.Windows.Forms.ToolStripButton();
            this.geometryBrushesTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.groupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ungroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.texturedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorMenu.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.propertiesControl.SuspendLayout();
            this.solidsTab.SuspendLayout();
            this.generalTabLayout.SuspendLayout();
            this.brushesPanel.WorkingArea.SuspendLayout();
            this.brushesPanel.SuspendLayout();
            this.modeGroupbox.SuspendLayout();
            this.creationGroupbox.SuspendLayout();
            this.creationFlowLayout.SuspendLayout();
            this.CsgGroupbox.SuspendLayout();
            this.expandablePanel1.WorkingArea.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            this.editorToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // editorMenu
            // 
            this.editorMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.editorMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.editorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.editorMenu.Location = new System.Drawing.Point(0, 0);
            this.editorMenu.Name = "editorMenu";
            this.editorMenu.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.editorMenu.Size = new System.Drawing.Size(1008, 24);
            this.editorMenu.TabIndex = 1;
            this.editorMenu.Text = "editorMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator9,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator7,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(109, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(109, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator10,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(104, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wireframeToolStripMenuItem,
            this.solidToolStripMenuItem,
            this.texturedToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupToolStripMenuItem,
            this.ungroupToolStripMenuItem,
            this.toolStripMenuItem1,
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 22);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Tag = "About";
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // mainContainer
            // 
            this.mainContainer.ColumnCount = 2;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.mainContainer.Controls.Add(this.editorViewport, 0, 0);
            this.mainContainer.Controls.Add(this.propertiesControl, 1, 0);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 55);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.RowCount = 1;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Size = new System.Drawing.Size(1008, 650);
            this.mainContainer.TabIndex = 4;
            // 
            // editorViewport
            // 
            this.editorViewport.BackColor = System.Drawing.Color.Black;
            this.editorViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorViewport.Location = new System.Drawing.Point(0, 0);
            this.editorViewport.Margin = new System.Windows.Forms.Padding(0);
            this.editorViewport.Name = "editorViewport";
            this.editorViewport.Size = new System.Drawing.Size(812, 650);
            this.editorViewport.TabIndex = 0;
            this.editorViewport.VSync = false;
            // 
            // propertiesControl
            // 
            this.propertiesControl.Controls.Add(this.solidsTab);
            this.propertiesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesControl.Location = new System.Drawing.Point(815, 3);
            this.propertiesControl.Name = "propertiesControl";
            this.propertiesControl.SelectedIndex = 0;
            this.propertiesControl.Size = new System.Drawing.Size(190, 644);
            this.propertiesControl.TabIndex = 2;
            // 
            // solidsTab
            // 
            this.solidsTab.Controls.Add(this.generalTabLayout);
            this.solidsTab.Location = new System.Drawing.Point(4, 22);
            this.solidsTab.Name = "solidsTab";
            this.solidsTab.Padding = new System.Windows.Forms.Padding(3);
            this.solidsTab.Size = new System.Drawing.Size(182, 618);
            this.solidsTab.TabIndex = 0;
            this.solidsTab.Text = "Solids";
            this.solidsTab.UseVisualStyleBackColor = true;
            // 
            // generalTabLayout
            // 
            this.generalTabLayout.AutoScroll = true;
            this.generalTabLayout.AutoScrollMinSize = new System.Drawing.Size(10, 300);
            this.generalTabLayout.Controls.Add(this.brushesPanel);
            this.generalTabLayout.Controls.Add(this.expandablePanel1);
            this.generalTabLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalTabLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.generalTabLayout.Location = new System.Drawing.Point(3, 3);
            this.generalTabLayout.Name = "generalTabLayout";
            this.generalTabLayout.Size = new System.Drawing.Size(176, 612);
            this.generalTabLayout.TabIndex = 1;
            this.generalTabLayout.WrapContents = false;
            // 
            // brushesPanel
            // 
            this.brushesPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brushesPanel.Caption = "Geometry";
            this.brushesPanel.ExpandedHeight = 0;
            this.brushesPanel.IsExpanded = true;
            this.brushesPanel.Location = new System.Drawing.Point(3, 3);
            this.brushesPanel.MinimumSize = new System.Drawing.Size(150, 32);
            this.brushesPanel.Name = "brushesPanel";
            this.brushesPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.brushesPanel.Size = new System.Drawing.Size(150, 260);
            this.brushesPanel.TabIndex = 7;
            // 
            // brushesPanel.WorkingArea
            // 
            this.brushesPanel.WorkingArea.AutoSize = true;
            this.brushesPanel.WorkingArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brushesPanel.WorkingArea.Controls.Add(this.modeGroupbox);
            this.brushesPanel.WorkingArea.Controls.Add(this.creationGroupbox);
            this.brushesPanel.WorkingArea.Controls.Add(this.CsgGroupbox);
            this.brushesPanel.WorkingArea.Controls.Add(this.solidProperties);
            this.brushesPanel.WorkingArea.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.brushesPanel.WorkingArea.Location = new System.Drawing.Point(3, 31);
            this.brushesPanel.WorkingArea.MinimumSize = new System.Drawing.Size(143, 40);
            this.brushesPanel.WorkingArea.Name = "WorkingArea";
            this.brushesPanel.WorkingArea.Size = new System.Drawing.Size(143, 226);
            this.brushesPanel.WorkingArea.TabIndex = 1;
            this.brushesPanel.WorkingArea.WrapContents = false;
            // 
            // modeGroupbox
            // 
            this.modeGroupbox.Controls.Add(this.modeVertexButton);
            this.modeGroupbox.Controls.Add(this.modeFaceButton);
            this.modeGroupbox.Controls.Add(this.modeBrushButton);
            this.modeGroupbox.Location = new System.Drawing.Point(3, 3);
            this.modeGroupbox.Name = "modeGroupbox";
            this.modeGroupbox.Size = new System.Drawing.Size(135, 46);
            this.modeGroupbox.TabIndex = 2;
            this.modeGroupbox.TabStop = false;
            this.modeGroupbox.Text = "Mode";
            // 
            // modeVertexButton
            // 
            this.modeVertexButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.modeVertexButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("modeVertexButton.BackgroundImage")));
            this.modeVertexButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modeVertexButton.FlatAppearance.BorderSize = 0;
            this.modeVertexButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.modeVertexButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.modeVertexButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.modeVertexButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modeVertexButton.Location = new System.Drawing.Point(72, 16);
            this.modeVertexButton.Name = "modeVertexButton";
            this.modeVertexButton.Size = new System.Drawing.Size(24, 24);
            this.modeVertexButton.TabIndex = 2;
            this.modeVertexButton.Tag = "brushmanipulation;vertex";
            this.modeVertexButton.UseVisualStyleBackColor = true;
            this.modeVertexButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // modeFaceButton
            // 
            this.modeFaceButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.modeFaceButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("modeFaceButton.BackgroundImage")));
            this.modeFaceButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modeFaceButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.modeFaceButton.FlatAppearance.BorderSize = 0;
            this.modeFaceButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.modeFaceButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.modeFaceButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.modeFaceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modeFaceButton.Location = new System.Drawing.Point(42, 16);
            this.modeFaceButton.Name = "modeFaceButton";
            this.modeFaceButton.Size = new System.Drawing.Size(24, 24);
            this.modeFaceButton.TabIndex = 1;
            this.modeFaceButton.Tag = "brushmanipulation;face";
            this.modeFaceButton.UseVisualStyleBackColor = true;
            this.modeFaceButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // modeBrushButton
            // 
            this.modeBrushButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.modeBrushButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("modeBrushButton.BackgroundImage")));
            this.modeBrushButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modeBrushButton.FlatAppearance.BorderSize = 0;
            this.modeBrushButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.modeBrushButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.modeBrushButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.modeBrushButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modeBrushButton.Location = new System.Drawing.Point(12, 16);
            this.modeBrushButton.Name = "modeBrushButton";
            this.modeBrushButton.Size = new System.Drawing.Size(24, 24);
            this.modeBrushButton.TabIndex = 0;
            this.modeBrushButton.Tag = "brushmanipulation;brush";
            this.modeBrushButton.UseVisualStyleBackColor = true;
            this.modeBrushButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // creationGroupbox
            // 
            this.creationGroupbox.AutoSize = true;
            this.creationGroupbox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.creationGroupbox.Controls.Add(this.creationFlowLayout);
            this.creationGroupbox.Location = new System.Drawing.Point(3, 55);
            this.creationGroupbox.MinimumSize = new System.Drawing.Size(135, 46);
            this.creationGroupbox.Name = "creationGroupbox";
            this.creationGroupbox.Padding = new System.Windows.Forms.Padding(0);
            this.creationGroupbox.Size = new System.Drawing.Size(135, 46);
            this.creationGroupbox.TabIndex = 1;
            this.creationGroupbox.TabStop = false;
            this.creationGroupbox.Text = "Creation";
            // 
            // creationFlowLayout
            // 
            this.creationFlowLayout.AutoSize = true;
            this.creationFlowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.creationFlowLayout.Controls.Add(this.creationButtonsPanel);
            this.creationFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.creationFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.creationFlowLayout.Location = new System.Drawing.Point(0, 13);
            this.creationFlowLayout.Name = "creationFlowLayout";
            this.creationFlowLayout.Size = new System.Drawing.Size(135, 33);
            this.creationFlowLayout.TabIndex = 0;
            this.creationFlowLayout.WrapContents = false;
            // 
            // creationButtonsPanel
            // 
            this.creationButtonsPanel.Location = new System.Drawing.Point(3, 1);
            this.creationButtonsPanel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.creationButtonsPanel.Name = "creationButtonsPanel";
            this.creationButtonsPanel.Size = new System.Drawing.Size(129, 27);
            this.creationButtonsPanel.TabIndex = 0;
            // 
            // CsgGroupbox
            // 
            this.CsgGroupbox.Controls.Add(this.csgExtrudeButton);
            this.CsgGroupbox.Controls.Add(this.csgHollowButton);
            this.CsgGroupbox.Controls.Add(this.csgSliceButton);
            this.CsgGroupbox.Controls.Add(this.csgCarveButton);
            this.CsgGroupbox.Location = new System.Drawing.Point(3, 107);
            this.CsgGroupbox.Name = "CsgGroupbox";
            this.CsgGroupbox.Size = new System.Drawing.Size(135, 46);
            this.CsgGroupbox.TabIndex = 3;
            this.CsgGroupbox.TabStop = false;
            this.CsgGroupbox.Text = "Csg";
            // 
            // csgExtrudeButton
            // 
            this.csgExtrudeButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.csgExtrudeButton.BackColor = System.Drawing.Color.Transparent;
            this.csgExtrudeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("csgExtrudeButton.BackgroundImage")));
            this.csgExtrudeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.csgExtrudeButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.csgExtrudeButton.FlatAppearance.BorderSize = 0;
            this.csgExtrudeButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.csgExtrudeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.csgExtrudeButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.csgExtrudeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.csgExtrudeButton.Location = new System.Drawing.Point(102, 16);
            this.csgExtrudeButton.Name = "csgExtrudeButton";
            this.csgExtrudeButton.Size = new System.Drawing.Size(24, 24);
            this.csgExtrudeButton.TabIndex = 5;
            this.csgExtrudeButton.Tag = "csg;extrude";
            this.csgExtrudeButton.UseVisualStyleBackColor = false;
            this.csgExtrudeButton.Visible = false;
            this.csgExtrudeButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // csgHollowButton
            // 
            this.csgHollowButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.csgHollowButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("csgHollowButton.BackgroundImage")));
            this.csgHollowButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.csgHollowButton.FlatAppearance.BorderSize = 0;
            this.csgHollowButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.csgHollowButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.csgHollowButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.csgHollowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.csgHollowButton.Location = new System.Drawing.Point(42, 16);
            this.csgHollowButton.Name = "csgHollowButton";
            this.csgHollowButton.Size = new System.Drawing.Size(24, 24);
            this.csgHollowButton.TabIndex = 8;
            this.csgHollowButton.Tag = "csg;hollow";
            this.csgHollowButton.UseVisualStyleBackColor = true;
            this.csgHollowButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // csgSliceButton
            // 
            this.csgSliceButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.csgSliceButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("csgSliceButton.BackgroundImage")));
            this.csgSliceButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.csgSliceButton.FlatAppearance.BorderSize = 0;
            this.csgSliceButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.csgSliceButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.csgSliceButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.csgSliceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.csgSliceButton.Location = new System.Drawing.Point(72, 16);
            this.csgSliceButton.Name = "csgSliceButton";
            this.csgSliceButton.Size = new System.Drawing.Size(24, 24);
            this.csgSliceButton.TabIndex = 6;
            this.csgSliceButton.Tag = "csg;slice";
            this.csgSliceButton.UseVisualStyleBackColor = true;
            this.csgSliceButton.Visible = false;
            this.csgSliceButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // csgCarveButton
            // 
            this.csgCarveButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.csgCarveButton.BackColor = System.Drawing.Color.Transparent;
            this.csgCarveButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("csgCarveButton.BackgroundImage")));
            this.csgCarveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.csgCarveButton.FlatAppearance.BorderSize = 0;
            this.csgCarveButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.csgCarveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.csgCarveButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.csgCarveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.csgCarveButton.Location = new System.Drawing.Point(12, 16);
            this.csgCarveButton.Name = "csgCarveButton";
            this.csgCarveButton.Size = new System.Drawing.Size(24, 24);
            this.csgCarveButton.TabIndex = 7;
            this.csgCarveButton.Tag = "csg;carve";
            this.csgCarveButton.UseVisualStyleBackColor = false;
            this.csgCarveButton.Click += new System.EventHandler(this.HandleGeometryBarItems);
            // 
            // solidProperties
            // 
            this.solidProperties.Enabled = false;
            this.solidProperties.Location = new System.Drawing.Point(3, 159);
            this.solidProperties.Name = "solidProperties";
            this.solidProperties.Size = new System.Drawing.Size(135, 64);
            this.solidProperties.TabIndex = 7;
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.expandablePanel1.Caption = "Textures";
            this.expandablePanel1.ExpandedHeight = 0;
            this.expandablePanel1.IsExpanded = true;
            this.expandablePanel1.Location = new System.Drawing.Point(3, 269);
            this.expandablePanel1.MinimumSize = new System.Drawing.Size(150, 32);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.expandablePanel1.Size = new System.Drawing.Size(150, 526);
            this.expandablePanel1.TabIndex = 8;
            // 
            // expandablePanel1.WorkingArea
            // 
            this.expandablePanel1.WorkingArea.AutoSize = true;
            this.expandablePanel1.WorkingArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.expandablePanel1.WorkingArea.Controls.Add(this.textureSelector);
            this.expandablePanel1.WorkingArea.Controls.Add(this.textureProperties);
            this.expandablePanel1.WorkingArea.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.expandablePanel1.WorkingArea.Location = new System.Drawing.Point(3, 31);
            this.expandablePanel1.WorkingArea.MinimumSize = new System.Drawing.Size(143, 40);
            this.expandablePanel1.WorkingArea.Name = "WorkingArea";
            this.expandablePanel1.WorkingArea.Size = new System.Drawing.Size(143, 492);
            this.expandablePanel1.WorkingArea.TabIndex = 1;
            this.expandablePanel1.WorkingArea.WrapContents = false;
            // 
            // textureSelector
            // 
            this.textureSelector.Location = new System.Drawing.Point(3, 3);
            this.textureSelector.Name = "textureSelector";
            this.textureSelector.Size = new System.Drawing.Size(135, 204);
            this.textureSelector.TabIndex = 1;
            this.textureSelector.TextureCollection = null;
            // 
            // textureProperties
            // 
            this.textureProperties.Location = new System.Drawing.Point(3, 213);
            this.textureProperties.Name = "textureProperties";
            this.textureProperties.Size = new System.Drawing.Size(135, 276);
            this.textureProperties.TabIndex = 0;
            // 
            // editorStatusbar
            // 
            this.editorStatusbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.editorStatusbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.editorStatusbar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.editorStatusbar.Location = new System.Drawing.Point(0, 705);
            this.editorStatusbar.Name = "editorStatusbar";
            this.editorStatusbar.Size = new System.Drawing.Size(1008, 25);
            this.editorStatusbar.TabIndex = 3;
            this.editorStatusbar.Text = "editorStatusbar";
            // 
            // toolbarNew
            // 
            this.toolbarNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarNew.Image = ((System.Drawing.Image)(resources.GetObject("toolbarNew.Image")));
            this.toolbarNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarNew.Name = "toolbarNew";
            this.toolbarNew.Size = new System.Drawing.Size(28, 28);
            this.toolbarNew.Tag = "new";
            this.toolbarNew.Text = "new";
            this.toolbarNew.ToolTipText = "Creates a new empty map";
            this.toolbarNew.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarOpen
            // 
            this.toolbarOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolbarOpen.Image")));
            this.toolbarOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarOpen.Name = "toolbarOpen";
            this.toolbarOpen.Size = new System.Drawing.Size(28, 28);
            this.toolbarOpen.Tag = "open";
            this.toolbarOpen.Text = "open";
            this.toolbarOpen.ToolTipText = "Opens a map";
            this.toolbarOpen.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarSave
            // 
            this.toolbarSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarSave.Enabled = false;
            this.toolbarSave.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSave.Image")));
            this.toolbarSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarSave.Name = "toolbarSave";
            this.toolbarSave.Size = new System.Drawing.Size(28, 28);
            this.toolbarSave.Tag = "save";
            this.toolbarSave.Text = "save";
            this.toolbarSave.ToolTipText = "Saves the current map";
            this.toolbarSave.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolbarCut
            // 
            this.toolbarCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarCut.Enabled = false;
            this.toolbarCut.Image = ((System.Drawing.Image)(resources.GetObject("toolbarCut.Image")));
            this.toolbarCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarCut.Name = "toolbarCut";
            this.toolbarCut.Size = new System.Drawing.Size(28, 28);
            this.toolbarCut.Tag = "cut";
            this.toolbarCut.Text = "cut";
            this.toolbarCut.ToolTipText = "Cut";
            this.toolbarCut.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarCopy
            // 
            this.toolbarCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarCopy.Enabled = false;
            this.toolbarCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolbarCopy.Image")));
            this.toolbarCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarCopy.Name = "toolbarCopy";
            this.toolbarCopy.Size = new System.Drawing.Size(28, 28);
            this.toolbarCopy.Tag = "copy";
            this.toolbarCopy.Text = "copy";
            this.toolbarCopy.ToolTipText = "Copy";
            this.toolbarCopy.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarPaste
            // 
            this.toolbarPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarPaste.Enabled = false;
            this.toolbarPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolbarPaste.Image")));
            this.toolbarPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarPaste.Name = "toolbarPaste";
            this.toolbarPaste.Size = new System.Drawing.Size(28, 28);
            this.toolbarPaste.Tag = "paste";
            this.toolbarPaste.Text = "paste";
            this.toolbarPaste.ToolTipText = "Paste";
            this.toolbarPaste.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolbarUndo
            // 
            this.toolbarUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarUndo.Enabled = false;
            this.toolbarUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolbarUndo.Image")));
            this.toolbarUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarUndo.Name = "toolbarUndo";
            this.toolbarUndo.Size = new System.Drawing.Size(28, 28);
            this.toolbarUndo.Tag = "undo";
            this.toolbarUndo.Text = "undo";
            this.toolbarUndo.ToolTipText = "Undo";
            // 
            // toolbarRedo
            // 
            this.toolbarRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarRedo.Enabled = false;
            this.toolbarRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolbarRedo.Image")));
            this.toolbarRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarRedo.Name = "toolbarRedo";
            this.toolbarRedo.Size = new System.Drawing.Size(28, 28);
            this.toolbarRedo.Tag = "redo";
            this.toolbarRedo.Text = "redo";
            this.toolbarRedo.ToolTipText = "Redo";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // toolbarWireframe
            // 
            this.toolbarWireframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarWireframe.Image = ((System.Drawing.Image)(resources.GetObject("toolbarWireframe.Image")));
            this.toolbarWireframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarWireframe.Name = "toolbarWireframe";
            this.toolbarWireframe.Size = new System.Drawing.Size(28, 28);
            this.toolbarWireframe.Tag = "wireframe";
            this.toolbarWireframe.Text = "rendermode";
            this.toolbarWireframe.ToolTipText = "Wireframe";
            this.toolbarWireframe.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarSolid
            // 
            this.toolbarSolid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarSolid.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSolid.Image")));
            this.toolbarSolid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarSolid.Name = "toolbarSolid";
            this.toolbarSolid.Size = new System.Drawing.Size(28, 28);
            this.toolbarSolid.Tag = "solid";
            this.toolbarSolid.Text = "rendermode";
            this.toolbarSolid.ToolTipText = "Solid";
            this.toolbarSolid.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarTextured
            // 
            this.toolbarTextured.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarTextured.Image = ((System.Drawing.Image)(resources.GetObject("toolbarTextured.Image")));
            this.toolbarTextured.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarTextured.Name = "toolbarTextured";
            this.toolbarTextured.Size = new System.Drawing.Size(28, 28);
            this.toolbarTextured.Tag = "textured";
            this.toolbarTextured.Text = "rendermode";
            this.toolbarTextured.ToolTipText = "Textured";
            this.toolbarTextured.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarTexturedLighted
            // 
            this.toolbarTexturedLighted.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarTexturedLighted.Enabled = false;
            this.toolbarTexturedLighted.Image = ((System.Drawing.Image)(resources.GetObject("toolbarTexturedLighted.Image")));
            this.toolbarTexturedLighted.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarTexturedLighted.Name = "toolbarTexturedLighted";
            this.toolbarTexturedLighted.Size = new System.Drawing.Size(28, 28);
            this.toolbarTexturedLighted.Tag = "texturedlighted";
            this.toolbarTexturedLighted.Text = "rendermode";
            this.toolbarTexturedLighted.ToolTipText = "Textured Lighted";
            this.toolbarTexturedLighted.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            this.toolStripSeparator3.Visible = false;
            // 
            // toolbarGroup
            // 
            this.toolbarGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarGroup.Enabled = false;
            this.toolbarGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolbarGroup.Image")));
            this.toolbarGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarGroup.Name = "toolbarGroup";
            this.toolbarGroup.Size = new System.Drawing.Size(28, 28);
            this.toolbarGroup.Tag = "group";
            this.toolbarGroup.Text = "group";
            this.toolbarGroup.ToolTipText = "Group";
            this.toolbarGroup.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolbarUngroup
            // 
            this.toolbarUngroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarUngroup.Enabled = false;
            this.toolbarUngroup.Image = ((System.Drawing.Image)(resources.GetObject("toolbarUngroup.Image")));
            this.toolbarUngroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarUngroup.Name = "toolbarUngroup";
            this.toolbarUngroup.Size = new System.Drawing.Size(28, 28);
            this.toolbarUngroup.Tag = "ungroup";
            this.toolbarUngroup.Text = "ungroup";
            this.toolbarUngroup.ToolTipText = "Ungroup";
            this.toolbarUngroup.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 31);
            // 
            // toolbarSettings
            // 
            this.toolbarSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSettings.Image")));
            this.toolbarSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarSettings.Name = "toolbarSettings";
            this.toolbarSettings.Size = new System.Drawing.Size(28, 28);
            this.toolbarSettings.Tag = "settings";
            this.toolbarSettings.Text = "settings";
            this.toolbarSettings.ToolTipText = "Settings";
            this.toolbarSettings.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // editorToolbar
            // 
            this.editorToolbar.CanOverflow = false;
            this.editorToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.editorToolbar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.editorToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbarNew,
            this.toolbarOpen,
            this.toolbarSave,
            this.toolStripSeparator2,
            this.toolbarCut,
            this.toolbarCopy,
            this.toolbarPaste,
            this.toolStripSeparator4,
            this.toolbarUndo,
            this.toolbarRedo,
            this.toolStripSeparator5,
            this.toolbarWireframe,
            this.toolbarSolid,
            this.toolbarTextured,
            this.toolbarTexturedLighted,
            this.toolStripSeparator3,
            this.toolbarAlignLeft,
            this.toolbarAlignRight,
            this.toolbarAlignTop,
            this.toolbarAlignBottom,
            this.toolStripSeparator6,
            this.toolbarFlipHorizontal,
            this.toolbarFlipVertical,
            this.toolStripSeparator1,
            this.toolbarGroup,
            this.toolbarUngroup,
            this.toolStripSeparator8,
            this.toolbarTextureLock,
            this.toolbarSettings});
            this.editorToolbar.Location = new System.Drawing.Point(0, 24);
            this.editorToolbar.Name = "editorToolbar";
            this.editorToolbar.Size = new System.Drawing.Size(1008, 31);
            this.editorToolbar.TabIndex = 2;
            this.editorToolbar.Text = "editorToolbar";
            // 
            // toolbarAlignLeft
            // 
            this.toolbarAlignLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarAlignLeft.Image = ((System.Drawing.Image)(resources.GetObject("toolbarAlignLeft.Image")));
            this.toolbarAlignLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarAlignLeft.Name = "toolbarAlignLeft";
            this.toolbarAlignLeft.Size = new System.Drawing.Size(28, 28);
            this.toolbarAlignLeft.Tag = "alignleft";
            this.toolbarAlignLeft.Text = "extraTool";
            this.toolbarAlignLeft.ToolTipText = "Align left";
            this.toolbarAlignLeft.Visible = false;
            // 
            // toolbarAlignRight
            // 
            this.toolbarAlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarAlignRight.Image = ((System.Drawing.Image)(resources.GetObject("toolbarAlignRight.Image")));
            this.toolbarAlignRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarAlignRight.Name = "toolbarAlignRight";
            this.toolbarAlignRight.Size = new System.Drawing.Size(28, 28);
            this.toolbarAlignRight.Tag = "alignright";
            this.toolbarAlignRight.Text = "extraTool";
            this.toolbarAlignRight.ToolTipText = "Align right";
            this.toolbarAlignRight.Visible = false;
            // 
            // toolbarAlignTop
            // 
            this.toolbarAlignTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarAlignTop.Image = ((System.Drawing.Image)(resources.GetObject("toolbarAlignTop.Image")));
            this.toolbarAlignTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarAlignTop.Name = "toolbarAlignTop";
            this.toolbarAlignTop.Size = new System.Drawing.Size(28, 28);
            this.toolbarAlignTop.Tag = "aligntop";
            this.toolbarAlignTop.Text = "extraTool";
            this.toolbarAlignTop.ToolTipText = "Align top";
            this.toolbarAlignTop.Visible = false;
            // 
            // toolbarAlignBottom
            // 
            this.toolbarAlignBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarAlignBottom.Image = ((System.Drawing.Image)(resources.GetObject("toolbarAlignBottom.Image")));
            this.toolbarAlignBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarAlignBottom.Name = "toolbarAlignBottom";
            this.toolbarAlignBottom.Size = new System.Drawing.Size(28, 28);
            this.toolbarAlignBottom.Tag = "alignbottom";
            this.toolbarAlignBottom.Text = "extraTool";
            this.toolbarAlignBottom.ToolTipText = "Align bottom";
            this.toolbarAlignBottom.Visible = false;
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            this.toolStripSeparator6.Visible = false;
            // 
            // toolbarFlipHorizontal
            // 
            this.toolbarFlipHorizontal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarFlipHorizontal.Image = ((System.Drawing.Image)(resources.GetObject("toolbarFlipHorizontal.Image")));
            this.toolbarFlipHorizontal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarFlipHorizontal.Name = "toolbarFlipHorizontal";
            this.toolbarFlipHorizontal.Size = new System.Drawing.Size(28, 28);
            this.toolbarFlipHorizontal.Tag = "fliphorizontal";
            this.toolbarFlipHorizontal.Text = "extraTool";
            this.toolbarFlipHorizontal.ToolTipText = "Flip horizontal";
            this.toolbarFlipHorizontal.Visible = false;
            // 
            // toolbarFlipVertical
            // 
            this.toolbarFlipVertical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarFlipVertical.Image = ((System.Drawing.Image)(resources.GetObject("toolbarFlipVertical.Image")));
            this.toolbarFlipVertical.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarFlipVertical.Name = "toolbarFlipVertical";
            this.toolbarFlipVertical.Size = new System.Drawing.Size(28, 28);
            this.toolbarFlipVertical.Tag = "flipvertical";
            this.toolbarFlipVertical.Text = "extraTool";
            this.toolbarFlipVertical.ToolTipText = "Flip vertical";
            this.toolbarFlipVertical.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolbarTextureLock
            // 
            this.toolbarTextureLock.CheckOnClick = true;
            this.toolbarTextureLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarTextureLock.Image = ((System.Drawing.Image)(resources.GetObject("toolbarTextureLock.Image")));
            this.toolbarTextureLock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarTextureLock.Name = "toolbarTextureLock";
            this.toolbarTextureLock.Size = new System.Drawing.Size(28, 28);
            this.toolbarTextureLock.Tag = "textureLock";
            this.toolbarTextureLock.Text = "textureLock";
            this.toolbarTextureLock.ToolTipText = "Texture Lock";
            this.toolbarTextureLock.Visible = false;
            this.toolbarTextureLock.Click += new System.EventHandler(this.OnHandleToolBarItems);
            // 
            // groupToolStripMenuItem
            // 
            this.groupToolStripMenuItem.Name = "groupToolStripMenuItem";
            this.groupToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.groupToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.groupToolStripMenuItem.Text = "Group";
            this.groupToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // ungroupToolStripMenuItem
            // 
            this.ungroupToolStripMenuItem.Name = "ungroupToolStripMenuItem";
            this.ungroupToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
            this.ungroupToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ungroupToolStripMenuItem.Text = "Ungroup";
            this.ungroupToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.wireframeToolStripMenuItem.Tag = "wireframe";
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // solidToolStripMenuItem
            // 
            this.solidToolStripMenuItem.Name = "solidToolStripMenuItem";
            this.solidToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.solidToolStripMenuItem.Tag = "solid";
            this.solidToolStripMenuItem.Text = "Solid";
            this.solidToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // texturedToolStripMenuItem
            // 
            this.texturedToolStripMenuItem.Name = "texturedToolStripMenuItem";
            this.texturedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.texturedToolStripMenuItem.Tag = "textured";
            this.texturedToolStripMenuItem.Text = "Textured";
            this.texturedToolStripMenuItem.Click += new System.EventHandler(this.OnHandleMenuItems);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.editorStatusbar);
            this.Controls.Add(this.editorToolbar);
            this.Controls.Add(this.editorMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.editorMenu;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(1023, 766);
            this.Name = "EditorForm";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rune Gear";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.editorMenu.ResumeLayout(false);
            this.editorMenu.PerformLayout();
            this.mainContainer.ResumeLayout(false);
            this.propertiesControl.ResumeLayout(false);
            this.solidsTab.ResumeLayout(false);
            this.generalTabLayout.ResumeLayout(false);
            this.brushesPanel.WorkingArea.ResumeLayout(false);
            this.brushesPanel.WorkingArea.PerformLayout();
            this.brushesPanel.ResumeLayout(false);
            this.brushesPanel.PerformLayout();
            this.modeGroupbox.ResumeLayout(false);
            this.creationGroupbox.ResumeLayout(false);
            this.creationGroupbox.PerformLayout();
            this.creationFlowLayout.ResumeLayout(false);
            this.CsgGroupbox.ResumeLayout(false);
            this.expandablePanel1.WorkingArea.ResumeLayout(false);
            this.expandablePanel1.ResumeLayout(false);
            this.expandablePanel1.PerformLayout();
            this.editorToolbar.ResumeLayout(false);
            this.editorToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip editorMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.TabControl propertiesControl;
        private System.Windows.Forms.TabPage solidsTab;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private ExpandableFlowLayoutPanel generalTabLayout;
        private EditorStatusBar editorStatusbar;
        private EditorGlViewport editorViewport;
        private System.Windows.Forms.ToolStripButton toolbarNew;
        private System.Windows.Forms.ToolStripButton toolbarOpen;
        private System.Windows.Forms.ToolStripButton toolbarSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolbarCut;
        private System.Windows.Forms.ToolStripButton toolbarCopy;
        private System.Windows.Forms.ToolStripButton toolbarPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolbarUndo;
        private System.Windows.Forms.ToolStripButton toolbarRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolbarWireframe;
        private System.Windows.Forms.ToolStripButton toolbarSolid;
        private System.Windows.Forms.ToolStripButton toolbarTextured;
        private System.Windows.Forms.ToolStripButton toolbarTexturedLighted;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolbarGroup;
        private System.Windows.Forms.ToolStripButton toolbarUngroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolbarSettings;
        private System.Windows.Forms.ToolStrip editorToolbar;
        private System.Windows.Forms.ToolStripButton toolbarAlignLeft;
        private System.Windows.Forms.ToolStripButton toolbarAlignRight;
        private System.Windows.Forms.ToolStripButton toolbarAlignTop;
        private System.Windows.Forms.ToolStripButton toolbarAlignBottom;
        private System.Windows.Forms.ToolStripButton toolbarFlipHorizontal;
        private System.Windows.Forms.ToolStripButton toolbarFlipVertical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolTip geometryBrushesTooltip;
        private ExpandablePanel brushesPanel;
        private System.Windows.Forms.GroupBox modeGroupbox;
        private System.Windows.Forms.CheckBox modeVertexButton;
        private System.Windows.Forms.CheckBox modeFaceButton;
        private System.Windows.Forms.CheckBox modeBrushButton;
        private System.Windows.Forms.GroupBox creationGroupbox;
        private System.Windows.Forms.GroupBox CsgGroupbox;
        private System.Windows.Forms.CheckBox csgExtrudeButton;
        private System.Windows.Forms.CheckBox csgHollowButton;
        private System.Windows.Forms.CheckBox csgSliceButton;
        private System.Windows.Forms.CheckBox csgCarveButton;
        private SolidProperties solidProperties;
        private ExpandablePanel expandablePanel1;
        private TextureSelector textureSelector;
        private TextureProperties textureProperties;
        private System.Windows.Forms.FlowLayoutPanel creationFlowLayout;
        private CreationButtonsPanel creationButtonsPanel;
        private System.Windows.Forms.ToolStripButton toolbarTextureLock;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ungroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem texturedToolStripMenuItem;
    }
}

