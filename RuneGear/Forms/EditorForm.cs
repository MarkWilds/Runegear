using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using RuneGear.Controls;
using RuneGear.FileSystem;
using RuneGear.General;
using RuneGear.General.Viewport;
using RuneGear.Geometry;
using RuneGear.Graphics.Textures;
using RuneGear.MapObjects;
using RuneGear.MapObjects.Operations;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.MapObjects.SolidMapObject.Creation;
using RuneGear.Properties;
using RuneGear.Tools;
using RuneGear.Utilities;
using RuneGear.Utilities.Extensions;

namespace RuneGear.Forms
{
    public partial class EditorForm : Form, IEditorController
    {
        private const string ApplicationName = "Runegear";
        public static Cursor Rotate;

        private SolidFactory solidFactory;
        private UserControl currentSolidCreatorControl;
        private string currentSolidCreatorIdentifier;

        private MapExporter defaultMapSaver;
        private MapImporter defaultMapLoader;
        public SolidManipulationMode CurrentSolidManipulationMode { get; set; }

        public EditorSettings EditorSettings { get; private set; }
        public SceneDocument SceneDocument { get; private set; }
        public TextureCollection TextureCollection { get; private set; }
        public BaseTool CurrentTool { get; set; }

        public RubberBand RubberBand { get; private set; }

        public MapObjectGroup Selection { get; set; }
        public MapObjectGroup CopyBoard { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditorForm()
        {
            InitializeComponent();
            InitializeEditor();
        }

        /// <summary>
        /// Initializes the editor
        /// </summary>
        private void InitializeEditor()
        {
            // events
            Move += editorViewport.OnEditorMoves;
            Activated += editorViewport.OnEditorGotFocus;
            Deactivate += editorViewport.OnEditorLostFocus;
            Disposed += EditorForm_Dispose;

            editorViewport.OnViewportFocus += OnViewportFocus;
            textureSelector.ApplyButton.Click += OnApplySelectedTexture;
            textureSelector.ReplaceButton.Click += OnReplaceSelectedTexture;

            creationButtonsPanel.OnCreationButtonPressed = HandleGeometryBarItems;
            solidProperties.OnPropertiesChanged = OnSolidPropertiesChanged;
            textureProperties.OnPropertiesChanged = OnTexturePropertiesChanged;
            textureProperties.OnJustify = OnTexturePropertiesJustifyClicked;

            // initialization
            EditorSettings = new EditorSettings();
            RubberBand = new RubberBand();
            Selection = new MapObjectGroup();
            CopyBoard = new MapObjectGroup();
            CurrentTool = new SolidManipulationTool();
            defaultMapSaver = new RuneMapExporter();
            defaultMapLoader = new RuneMapImporter();
            TextureCollection = new TextureCollection(EditorSettings.TextureFolder);

            if (!DesignMode)
            {
                EditorSettings.Load();
                SetupSolidFactory();
                solidProperties.UpdateProperties(Selection);
                editorViewport.SetController(this);
                textureSelector.TextureCollection = TextureCollection;
                CurrentTool.Initialize(this);

                Rotate = GeneralUtility.CreateCursor(Resources.rotate, 16, 16);
                NewDocument();
            }
        }

        /// <summary>
        /// Things that need to be done when the GUI is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditorForm_Load(object sender, EventArgs e)
        {
            Hide();
            Opacity = 100;

            creationButtonsPanel.UpdatePrimitiveToolbar("box");
            UpdateSolidManipulationToolbar(SolidManipulationMode.Solid);
            SetSolidManipulationMode("solid");

            // show splash form
            SplashForm splash = new SplashForm();
            splash.OnSplashDone += Show;
            splash.OnSplashIsShown = () =>
            {
                TextureCollection.OnTextureLoadingProgress += splash.SetProgressBar;
                TextureCollection.LoadTextureCollection();
            };

            splash.ShowDialog();
        }

        /// <summary>
        /// Resets editor tool state
        /// </summary>
        private void ResetEditorState()
        {
            editorViewport.ResetViewports();
            RubberBand.SetToZeroVolume();
            Selection.Clear();
            CopyBoard.Clear();
            ResetRenderModeToolbarAndMenu();
            creationButtonsPanel.UpdatePrimitiveToolbar("box");
            SetSolidManipulationMode("solid");
            UpdateSolidManipulationToolbar(SolidManipulationMode.Solid);
            UpdateGroupToolbar();

            CurrentTool = new SolidManipulationTool();
            CurrentTool.Initialize(this);
        }

        /// <summary>
        /// Setup the solid creation factory
        /// </summary>
        private void SetupSolidFactory()
        {
            solidFactory = new SolidFactory();
            SolidSidesCreationPanel control = new SolidSidesCreationPanel();

            // register solid creators
            solidFactory.RegisterCreator(new BoxSolidCreator("box"), null);
            solidFactory.RegisterCreator(new CylinderSolidCreator("cylinder", control), control);
            solidFactory.RegisterCreator(new WedgeSolidCreator("wedge"), null);
            solidFactory.RegisterCreator(new ConeSolidCreator("cone", control), control);

            SetSolidCreator("box");
        }

        /// <summary>
        /// Set the caption for the editor
        /// </summary>
        private void SetupEditorCaption()
        {
            Text = $@"{ApplicationName} - <{SceneDocument.MapName}>";
        }

        /// <summary>
        /// Create a new document
        /// </summary>
        private void NewDocument()
        {
            if (SceneDocument == null)
                SceneDocument = new SceneDocument();
            else
                SceneDocument.Clear();

            SceneDocument.OnDirty += (sender, args) => toolbarSave.Enabled = args.IsDirty;

            SetupEditorCaption();
            ResetEditorState();
            RenderViewports();
        }

        /// <summary>
        /// Initiate document saving
        /// </summary>
        private void SaveDocument(bool saveAs = false)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = "Save a " + defaultMapSaver.FileDescription,
                Filter = $@"{defaultMapSaver.FileTypeName}|*{defaultMapSaver.FileExtensionName}",
                FileName = SceneDocument.MapName
            };

            saveDialog.FileOk += (sender, e) =>
            {
                SceneDocument.AbsoluteFileName = saveDialog.FileName;
                SceneDocument.MapName = Path.GetFileNameWithoutExtension(saveDialog.FileName);
                SaveWithFilestream((FileStream)saveDialog.OpenFile());

                SceneDocument.IsDirty = false;
                SetupEditorCaption();
            };

            bool unsavedMap = String.IsNullOrEmpty(SceneDocument.AbsoluteFileName);
            if (unsavedMap || saveAs)
            {
                saveDialog.ShowDialog();
            }
            else if (File.Exists(SceneDocument.AbsoluteFileName))
            {
                SaveWithFilestream(File.OpenWrite(SceneDocument.AbsoluteFileName));
                SceneDocument.IsDirty = false;
            }
        }

        private void SaveWithFilestream(FileStream stream)
        {
            using (FileStream fileStream = stream)
            {
                try
                {
                    defaultMapSaver.Export(fileStream, SceneDocument);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Initiate document loading
        /// </summary>
        private void OpenDocument()
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = @"Open a " + defaultMapLoader.FileDescription,
                Filter = $@"{defaultMapLoader.FileTypeName}|*{defaultMapLoader.FileExtensionName}"
            };

            openDialog.FileOk += (sender, e) =>
            {
                using (FileStream fileStream = (FileStream)openDialog.OpenFile())
                {
                    try
                    {
                        SceneDocument newDocument = new SceneDocument();
                        defaultMapLoader.Import(fileStream, newDocument);
                        SceneDocument = newDocument;
                        SceneDocument.AbsoluteFileName = openDialog.FileName;
                        SceneDocument.OnDirty += (s, args) => toolbarSave.Enabled = args.IsDirty;

                        SetupEditorCaption();
                        ResetEditorState();
                        LoadUsedTexturesToGpu();
                        RenderViewports();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };

            openDialog.ShowDialog();
        }

        private void LoadUsedTexturesToGpu()
        {
            CustomOperation loadTextureOperation = new CustomOperation();
            loadTextureOperation.OnMapObjectGroup = group =>
            {
                group.MapObjectList.ForEach(g => g.PerformOperation(loadTextureOperation));
            };
            loadTextureOperation.OnSolid = solid =>
            {
                solid.Faces.ForEach(f =>
                {
                    f.Texture = TextureCollection.LoadTexture2D(f.Texture.Identifier);
                    solid.AlignTextureAxisToWorldForFace(f);
                    solid.CalculateTextureCoordinatesForFace(f, true);
                });
            };

            foreach (MapObject mapObject in SceneDocument)
            {
                mapObject.PerformOperation(loadTextureOperation);
            }   
        }

        /// <summary>
        /// Set the cursor for the application
        /// </summary>
        /// <param name="cursor"></param>
        public void SetCursor(Cursor cursor)
        {
            editorViewport.Cursor = cursor;
        }

        /// <summary>
        /// set the new solidCreator
        /// </summary>
        /// <param name="creator"></param>
        private void SetSolidCreator(string creator)
        {
            if (creator == currentSolidCreatorIdentifier)
                return;

            currentSolidCreatorIdentifier = creator;
            creationButtonsPanel.UpdatePrimitiveToolbar(creator);

            UserControl solidCreationControl = solidFactory.GetSolidPropertyControl(currentSolidCreatorIdentifier);
            if (!creationFlowLayout.Controls.Contains(solidCreationControl))
            {
                creationFlowLayout.Controls.Remove(currentSolidCreatorControl);
                creationFlowLayout.Controls.Add(solidCreationControl);
                currentSolidCreatorControl = solidCreationControl;
            }
        }

        /// <summary>
        /// Set the new rendermode
        /// </summary>
        /// <param name="mode"></param>
        private void SetRenderMode(string mode)
        {
            // default is wireframe
            BaseViewport.RenderModes currentRenderMode = BaseViewport.RenderModes.WIREFRAME;
            switch (mode)
            {
                case "solid":
                    currentRenderMode = BaseViewport.RenderModes.SOLID;
                    break;
                case "textured":
                    currentRenderMode = BaseViewport.RenderModes.TEXTURED;
                    break;
                case "texturedlighted":
                    currentRenderMode = BaseViewport.RenderModes.TEXTUREDLIGHTED;
                    break;
            }

            if (editorViewport.SetRendermode(currentRenderMode))
                UpdateRenderModeToolbarAndMenu(currentRenderMode);
        }

        /// <summary>
        /// set the new solid manipulation mode
        /// </summary>
        /// <param name="mode"></param>
        private void SetSolidManipulationMode(string mode)
        {
            CurrentTool?.Deinitialize();

            switch (mode)
            {
                case "brush":
                    CurrentTool = new SolidManipulationTool();
                    textureProperties.Enabled = true;
                    break;
                case "face":
                    CurrentTool = new SolidFaceTool();
                    textureProperties.Enabled = true;
                    break;
                case "vertex":
                    CurrentTool = new SolidVertexTool();
                    textureProperties.Enabled = false;
                    break;
            }

            CurrentTool?.Initialize(this);

            UpdateSolidManipulationToolbar();
            UpdateSolidPropertiesControl();
            UpdateCutCopyPasteToolbar();
            UpdateGroupToolbar();

            RenderViewports();
        }

        /// <summary>
        /// Handles extra toolbar items
        /// </summary>
        /// <param name="id"></param>
        private void HandleExtraTools(string id)
        {
            switch (id)
            {
                case "alignleft":
                    break;
                case "alignright":
                    break;
                case "aligntop":
                    break;
                case "alignbottom":
                    break;
                case "fliphorizontal":
                    break;
                case "flipvertical":
                    break;
            }
        }

        #region mapobject methods

        /// <summary>
        /// Render all viewports currently shown
        /// </summary>
        public void RenderViewports()
        {
            editorViewport.Invalidate();
        }

        public void DeleteMapObject()
        {
            if (!Selection.Empty)
            {
                foreach (MapObject mapObject in Selection)
                {
                    SceneDocument.RemoveMapObject(mapObject);
                }
                
                Selection.Clear();

                UpdateSolidPropertiesControl();
                editorStatusbar.ResetData();
                RenderViewports();
            }
        }

        public void CopyMapObject()
        {
            if (!Selection.Empty)
            {
                CopyBoard.Clear();
                foreach (MapObject mapObject in Selection)
                {
                    CopyBoard.Add((MapObject)mapObject.Clone());
                }

                UpdateCutCopyPasteToolbar();
            }
        }

        public void CutMapObject()
        {
            if (!Selection.Empty)
            {
                CopyMapObject();
                DeleteMapObject();
                UpdateCutCopyPasteToolbar();
            }
        }

        public void PasteMapObject()
        {
            if (!CopyBoard.Empty)
            {
                List<MapObject> clonedMapObjects = new List<MapObject>();
                foreach (MapObject mapObject in CopyBoard)
                {
                    clonedMapObjects.Add((MapObject)mapObject.Clone());
                }

                Selection.Clear();
                foreach (MapObject mapObject in clonedMapObjects)
                {
                    SceneDocument.AddMapObject(mapObject);
                    Selection.Add(mapObject);
                }

                CopyBoard.Clear();
                UpdateCutCopyPasteToolbar();

                RenderViewports();
            }
        }

        private void GroupMapObject()
        {
            if (!Selection.Empty && Selection.MapObjectList.Count > 1)
            {
                // remove the map objects from the document
                foreach (MapObject mapObject in Selection)
                {
                    SceneDocument.RemoveMapObject(mapObject);
                }

                // clone and clear the selected map objects
                MapObject newMapObjectGroup = (MapObject)Selection.Clone();
                Selection.Clear();

                // add map object group with map objects in it to the document and selection map object
                SceneDocument.AddMapObject(newMapObjectGroup);
                Selection.Add(newMapObjectGroup);

                UpdateGroupToolbar();

                RenderViewports();
            }
        }

        private void UngroupMapObject()
        {
            if (!Selection.Empty)
            {
                List<MapObjectGroup> groups = new List<MapObjectGroup>();
                CustomOperation collectGroupsOperation = new CustomOperation
                {
                    OnMapObjectGroup = group =>
                    {
                        if (!group.IsTransient)
                            return;

                        groups.Add(group);
                    }
                };

                // collect all top level groups
                Selection.MapObjectList.ForEach(m => m.PerformOperation(collectGroupsOperation));

                // remove groups that have been ungrouped
                groups.ForEach(group =>
                {
                    group.Selected = false;
                    Selection.Remove(group);
                    SceneDocument.RemoveMapObject(group);

                    group.MapObjectList.ForEach(mapObject =>
                    {
                        mapObject.Selected = true;
                        Selection.Add(mapObject);
                        SceneDocument.AddMapObject(mapObject);
                    });
                });

                groups.Clear();
                UpdateGroupToolbar();
            }
        }

        private void ApplySelectedTexture(TextureItem texture)
        {
            if (Selection.Empty)
                return;

            CustomOperation operation = new CustomOperation();
            operation.OnMapObjectGroup = (group) => group.MapObjectList.ForEach((subMapObject) => subMapObject.PerformOperation(operation));
            operation.OnSolid = (solid) =>
            {
                solid.Faces.ForEach(face =>
                {
                    Texture2D texture2D = TextureCollection.LoadTexture2D(texture.Id);
                    bool sameTexture = face.Texture?.Identifier == texture.Id;
                    if (CurrentSolidManipulationMode == SolidManipulationMode.Solid)
                    {
                        if(!sameTexture)
                            face.Texture = texture2D;
                    }
                    else if (CurrentSolidManipulationMode == SolidManipulationMode.Face)
                    {
                        
                        if (face.Selected && !sameTexture)
                            face.Texture = texture2D;
                    }
                });
            };
            Selection.PerformOperation(operation);
            
            SceneDocument.IsDirty = true;

            RenderViewports();
        }

        /// <summary>
        /// transform the aabb we just drew to X/Y space
        /// create and transform solid back to this viewport space
        /// </summary>
        /// <param name="transform"></param>
        public void CreateSolid(Matrix4 transform)
        {
            if (!RubberBand.Bounds.HasVolume3D)
                return;

            // Transform rubberband to allow creation of correct orientated solid
            AABB creationBounds = (AABB)RubberBand.Bounds.Clone();
            Vector3 min = creationBounds.Min.TransformL(transform);
            Vector3 max = creationBounds.Max.TransformL(transform);

            creationBounds.Reset();
            creationBounds.Grow(min);
            creationBounds.Grow(max);

            // create solid
            Solid solid = solidFactory.CreateMapObject(currentSolidCreatorIdentifier, creationBounds);

            // transform the solid and recreate the bounds with every transformed vertex
            transform.Invert();
            for (int i = 0; i < solid.VertexPositions.Count; i++)
            {
                solid.VertexPositions[i] = solid.VertexPositions[i].TransformL(transform);
            }

            solid.RegenerateBounds();
            solid.CalculateNormals();

            // add to scene and selection
            SceneDocument.AddMapObject(solid);
            solid.Selected = true;
            Selection.Clear();
            Selection.Add(solid);

            // set solid properties
            if (TextureCollection.SelectedTexture.HasValue)
                ApplySelectedTexture(TextureCollection.SelectedTexture.Value);

            // calculate texture coordinates
            solid.Faces.ForEach(solid.AlignTextureAxisToWorldForFace);
            solid.Faces.ForEach( f => solid.CalculateTextureCoordinatesForFace(f, true));
        }

        /// <summary>
        /// Tries to select a map object
        /// TODO add spatial partitioning
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="viewport"></param>
        /// <returns>if a map object is selected it returns true</returns>
        public MapObject GetMapObjectIfHit(int x, int y, BaseViewport viewport)
        {
            HitOperation operation = new HitOperation(viewport, new Point(x, y));

            foreach (MapObject mapObject in SceneDocument)
            {
                mapObject.PerformOperation(operation);
            }

            return operation.HitMapObject;
        }
        
        #endregion

        #region update gui

        /// <summary>
        /// Updates the whole user interface of the editor
        /// </summary>
        public void UpdateUserInterface()
        {
            UpdateSolidPropertiesControl();
            UpdateTexturePropertiesControl();
            UpdateCutCopyPasteToolbar();
            UpdateGroupToolbar();
        }

        /// <summary>
        /// Reset the render modes in the gui
        /// </summary>
        private void ResetRenderModeToolbarAndMenu()
        {
            // toolbar
            toolbarWireframe.CheckState = CheckState.Unchecked;
            toolbarSolid.CheckState = CheckState.Unchecked;
            toolbarTextured.CheckState = CheckState.Unchecked;
            toolbarTexturedLighted.CheckState = CheckState.Unchecked;

            // menu
            wireframeToolStripMenuItem.CheckState = CheckState.Unchecked;
            solidToolStripMenuItem.CheckState = CheckState.Unchecked;
            texturedToolStripMenuItem.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// Reset the solid manipulation modes in the gui
        /// </summary>
        private void ResetSolidManipulationToolbar()
        {
            modeBrushButton.CheckState = CheckState.Unchecked;
            modeFaceButton.CheckState = CheckState.Unchecked;
            modeVertexButton.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// Updates the editor GUI for render modes
        /// </summary>
        /// <param name="renderMode"></param>
        private void UpdateRenderModeToolbarAndMenu(BaseViewport.RenderModes renderMode)
        {
            ResetRenderModeToolbarAndMenu();

            switch (renderMode)
            {
                case BaseViewport.RenderModes.WIREFRAME:
                    toolbarWireframe.CheckState = CheckState.Checked;
                    wireframeToolStripMenuItem.CheckState = CheckState.Checked;
                    break;
                case BaseViewport.RenderModes.SOLID:
                    toolbarSolid.CheckState = CheckState.Checked;
                    solidToolStripMenuItem.CheckState = CheckState.Checked;
                    break;
                case BaseViewport.RenderModes.TEXTURED:
                    toolbarTextured.CheckState = CheckState.Checked;
                    texturedToolStripMenuItem.CheckState = CheckState.Checked;
                    break;
                case BaseViewport.RenderModes.TEXTUREDLIGHTED:
                    toolbarTexturedLighted.CheckState = CheckState.Checked;
                    break;
            }
        }

        /// <summary>
        /// Update tool toolbar in the GUI
        /// </summary>
        private void UpdateCutCopyPasteToolbar()
        {
            toolbarCut.Enabled = !Selection.Empty;
            toolbarCopy.Enabled = !Selection.Empty;
            toolbarPaste.Enabled = !CopyBoard.Empty;
        }

        /// <summary>
        /// Update the gui for the group/ungroup buttons
        /// </summary>
        private void UpdateGroupToolbar()
        {
            int groupCount = 0;
            int mapObjectCount = Selection.MapObjectList.Count;

            CustomOperation countGroupOperation = new CustomOperation();
            countGroupOperation.OnMapObjectGroup = group =>
            {
                groupCount++;
                group.MapObjectList.ForEach(mapObject => mapObject.PerformOperation(countGroupOperation));
            };

            Selection.MapObjectList.ForEach(m => m.PerformOperation(countGroupOperation));

            toolbarGroup.Enabled = mapObjectCount > 1;
            toolbarUngroup.Enabled = groupCount > 0;
        }

        /// <summary>
        /// Updates the editor GUI for solid manipulation modes
        /// </summary>
        /// <param name="mode"></param>
        private void UpdateSolidManipulationToolbar(SolidManipulationMode mode)
        {
            ResetSolidManipulationToolbar();

            switch (mode)
            {
                case SolidManipulationMode.Solid:
                    modeBrushButton.CheckState = CheckState.Checked;
                    break;
                case SolidManipulationMode.Face:
                    modeFaceButton.CheckState = CheckState.Checked;
                    break;
                case SolidManipulationMode.Vertex:
                    modeVertexButton.CheckState = CheckState.Checked;
                    break;
            }
        }

        /// <summary>
        /// Updates the editor GUI for solid manipulation modes for the current mode
        /// </summary>
        private void UpdateSolidManipulationToolbar()
        {
            UpdateSolidManipulationToolbar(CurrentSolidManipulationMode);
        }

        /// <summary>
        /// Update the solid properties control
        /// </summary>
        private void UpdateSolidPropertiesControl()
        {
            solidProperties.UpdateProperties(Selection);
        }

        /// <summary>
        /// Update the texture properties control
        /// </summary>
        private void UpdateTexturePropertiesControl()
        {
            textureProperties.UpdateProperties(Selection);
        }

        #endregion

        #region events

        /// <summary>
        /// Handle menu items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHandleMenuItems(object sender, EventArgs e)
        {
            string menuId = sender.ToString();

            switch (menuId)
            {
                case "New":
                    NewDocument();
                    break;
                case "Open":
                    OpenDocument();
                    break;
                case "Save":
                    SaveDocument();
                    break;
                case "Save as":
                    SaveDocument(true);
                    break;

                case "Cut":
                    CutMapObject();
                    break;
                case "Copy":
                    CopyMapObject();
                    break;
                case "Paste":
                    PasteMapObject();
                    break;
                case "Delete":
                    DeleteMapObject();
                    break;

                case "Group":
                    GroupMapObject();
                    break;
                case "Ungroup":
                    UngroupMapObject();
                    break;
                case "About":
                    AboutDialog aboutDialog = new AboutDialog();
                    aboutDialog.ShowDialog();
                    break;

                case "Wireframe":
                case "Solid":
                case "Textured":
                    SetRenderMode(menuId.ToLower());
                    break;

                case "Settings":
                    SettingsDialog settingsDialog = new SettingsDialog(EditorSettings);
                    settingsDialog.ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// Handle toolbar buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHandleToolBarItems(object sender, EventArgs e)
        {
            string toolId = sender.ToString();
            string tag = ((ToolStripButton) sender).Tag.ToString();

            switch (toolId)
            {
                case "new":
                    NewDocument();
                    break;
                case "open":
                    OpenDocument();
                    break;
                case "save":
                    SaveDocument();
                    break;

                case "cut":
                    CutMapObject();
                    break;
                case "copy":
                    CopyMapObject();
                    break;
                case "paste":
                    PasteMapObject();
                    break;

                case "rendermode":
                    SetRenderMode(tag);
                    break;
                    
                case "textureLock":
                    EditorSettings.GlobalTextureLock = toolbarTextureLock.Checked;
                    break;

                case "settings":
                    SettingsDialog settingsDialog = new SettingsDialog(EditorSettings);
                    settingsDialog.ShowDialog();
                    break;
                case "group":
                    GroupMapObject();
                    break;
                case "ungroup":
                    UngroupMapObject();
                    break;

                case "extratools":
                    HandleExtraTools(tag);
                    break;
            }
        }

        /// <summary>
        /// Handles buttons from the Geometry tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleGeometryBarItems(object sender, EventArgs e)
        {
            string tag = ((Control)sender).Tag.ToString();
            string[] split = tag.Split(';');
            if (split.Length < 1)
                return;

            string toolType = split[0];
            string tool = split[1];

            switch (toolType)
            {
                case "primitive":
                    SetSolidCreator(tool);
                    break;
                case "brushmanipulation":
                    SetSolidManipulationMode(tool);
                    break;
            }
        }

        /// <summary>
        /// Performs solid properties setting
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        private void OnSolidPropertiesChanged(SolidProperties control, SolidProperties.PropertyName name)
        {
            // collect solid and group map objects
            CustomOperation setSolidPropertiesOperation = new CustomOperation();
            setSolidPropertiesOperation.OnMapObjectGroup = group =>
            group.MapObjectList.ForEach(m => m.PerformOperation(setSolidPropertiesOperation));

            setSolidPropertiesOperation.OnSolid = solid =>
            {
                switch (name)
                {
                    case SolidProperties.PropertyName.Detail:
                        solid.Detail = control.IsDetail;
                        break;
                    case SolidProperties.PropertyName.Hidden:
                        solid.Hidden = control.IsHidden;
                        break;
                }
            };
            Selection.PerformOperation(setSolidPropertiesOperation);

            SceneDocument.IsDirty = true;
        }

        /// <summary>
        /// Performs texture properties setting
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        private void OnTexturePropertiesChanged(TextureProperties control, TextureProperties.PropertyName name)
        {
            if (CurrentSolidManipulationMode == SolidManipulationMode.Vertex)
                return;

            // collect solid and group map objects
            CustomOperation setSolidPropertiesOperation = new CustomOperation();
            setSolidPropertiesOperation.OnMapObjectGroup = group =>
            group.MapObjectList.ForEach(m => m.PerformOperation(setSolidPropertiesOperation));

            setSolidPropertiesOperation.OnSolid = solid =>
            {
                solid.Faces.Where(f => f.Selected).ToList().ForEach(face =>
                {
                    switch (name)
                    {
                        case TextureProperties.PropertyName.UScale:
                            face.TextureMapping.UScale = control.UScale;
                            break;
                        case TextureProperties.PropertyName.VScale:
                            face.TextureMapping.VScale = control.VScale;
                            break;
                        case TextureProperties.PropertyName.UShift:
                            face.TextureMapping.UShift = control.UShift;
                            break;
                        case TextureProperties.PropertyName.VShift:
                            face.TextureMapping.VShift = control.VShift;
                            break;
                        case TextureProperties.PropertyName.TextureLock:
                            face.TextureMapping.TextureLocked = control.TextureLocked;
                            break;
                    }

                    // Rotation is handled differently
                    if (name == TextureProperties.PropertyName.Rotation)
                        solid.SetTextureRotationForFace(face, control.Rotation);
                        
                    solid.CalculateTextureCoordinatesForFace(face);
                });
            };
            Selection.PerformOperation(setSolidPropertiesOperation);
            
            SceneDocument.IsDirty = true;

            RenderViewports();
        }

        private void OnTexturePropertiesJustifyClicked(TextureProperties textureProperties, TextureProperties.JustifyType justifyType)
        {
            if (CurrentSolidManipulationMode == SolidManipulationMode.Vertex)
                return;

            // collect solid and group map objects
            CustomOperation setSolidPropertiesOperation = new CustomOperation();
            setSolidPropertiesOperation.OnMapObjectGroup = group =>
                group.MapObjectList.ForEach(m => m.PerformOperation(setSolidPropertiesOperation));
            AABB extends = new AABB();

            // create extends for all selected faces if treat as one is true
            if (textureProperties.TreatAsOne)
            {
                CustomOperation createExtendsOperation = new CustomOperation();
                createExtendsOperation.OnMapObjectGroup = group =>
                    group.MapObjectList.ForEach(m => m.PerformOperation(createExtendsOperation));
                createExtendsOperation.OnSolid = solid =>
                {
                    solid.Faces.Where(f => f.Selected).ToList()
                    .ForEach(face =>
                    {
                        IEnumerable<Vector3> points = solid.GetVerticesForFace(face).Select(v => v.Position);
                        extends.Grow(points);
                    });
                };

                Selection.PerformOperation(createExtendsOperation);
            }

            setSolidPropertiesOperation.OnSolid = solid =>
            {
                solid.Faces.Where(f => f.Selected).ToList().ForEach(face =>
                {
                    // create extends for this face if Treat as one is false
                    if (!textureProperties.TreatAsOne)
                    {
                        extends.Reset();
                        extends.Grow(solid.GetVerticesForFace(face).Select(v => v.Position));
                    }

                    switch (justifyType)
                    {
                        case TextureProperties.JustifyType.Fit:
                            solid.FitTextureToExtendsForFace(extends, face);
                            break;
                        case TextureProperties.JustifyType.Top:
                        case TextureProperties.JustifyType.Bottom:
                        case TextureProperties.JustifyType.Left:
                        case TextureProperties.JustifyType.Right:
                        case TextureProperties.JustifyType.Center:
                            solid.AlignTextureToExtendsForFace(extends, face, justifyType);
                            break;
                    }

                    solid.CalculateTextureCoordinatesForFace(face, true);
                });
            };
            Selection.PerformOperation(setSolidPropertiesOperation);
            
            SceneDocument.IsDirty = true;
            UpdateUserInterface();
            
            RenderViewports();   
        }

        /// <summary>
        /// Called when the replace texture button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplaceSelectedTexture(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Called when a texture is applied through the texture selector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplySelectedTexture(object sender, EventArgs e)
        {
            TextureItem texture = textureSelector.CurrentTextureItem;
            ApplySelectedTexture(texture);
        }

        /// <summary>
        /// Disposes members that need it
        /// </summary>
        public void EditorForm_Dispose(object sender, EventArgs e)
        {
            TextureCollection.Dispose();
        }

        /// <summary>
        /// Handle viewport focus event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnViewportFocus(object sender, EditorGlViewport.ViewportEventArgs e)
        {
            BaseViewport focusedViewport = e.FocusedViewport;

            // set status bar with new information
            editorStatusbar.UpdateStatusBar(focusedViewport);

            // update render mode and other information where needed
            UpdateRenderModeToolbarAndMenu(focusedViewport.RenderMode);
        }

        #endregion
    }
}