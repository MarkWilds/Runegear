using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RuneGear.General;
using RuneGear.Properties;
using RuneGear.Utilities;

namespace RuneGear.Forms
{
    public partial class TextureBrowserDialog : Form
    {
        enum NodeType
        {
            Directory = 0,
            Texture    
        }

        // Events
        public Action<TextureItem> OnTextureSelected { get; set; }
        public Action<TextureItem> OnApplyPressed { get; set; }

        // Data members
        private readonly TextureCollection textureCollection;
        private readonly ImageList textureImageList;
        private readonly ImageList treeImageList;

        private int textureDimensions = 128;
        private int treeIconDimensions = 16;
        private string currentGroupName;
        private TextureItem? lastSelectedTexture;

        public TextureBrowserDialog(TextureCollection collection)
        {
            InitializeComponent();

            textureCollection = collection;
            textureImageList = new ImageList();
            treeImageList = new ImageList();
            textureTreeView.HandleCreated += (sender, eventArgs) =>
            {
                ((Control)sender).DoubleBuffered(true);
            };

//            textureListView.HideSelection = false;
            textureListView.HandleCreated += (sender, eventArgs) =>
            {
                ((Control)sender).DoubleBuffered(true);
            };
        }

        private void TextureBrowserDialog_Load(object sender, EventArgs e)
        {
            GeneralUtility.SetSpacing(textureListView, (short)(textureDimensions + 16), (short)(textureDimensions + 24));

            // instantiate list view
            textureListView.LargeImageList = textureImageList;
            textureImageList.ImageSize = new Size(textureDimensions, textureDimensions);
            textureImageList.ColorDepth = ColorDepth.Depth24Bit;

            // configure tree list
            treeImageList.ImageSize = new Size(treeIconDimensions, treeIconDimensions);
            treeImageList.ColorDepth = ColorDepth.Depth24Bit;
            textureTreeView.ImageList = treeImageList;
            treeImageList.Images.Add(Resources.folder_icon);
            treeImageList.Images.Add(Resources.texture_icon);
            
            TextureCollection textureManager = textureCollection;
            textureManager.OnTextureCreated = OnTextureCreated;
            textureManager.OnTextureDeleted = OnTextureDeleted;
            textureManager.OnTextureRenamed = OnTextureRenamed;

            textureListView.DrawItem += OnDrawListViewItem;

            foreach (string groupName in textureManager.TextureGroupNames)
            {
                TreeNode groupNode = new TreeNode
                {
                    Name = groupName,
                    Text = groupName,
                    Tag = NodeType.Directory,
                    ImageIndex = 0,
                    SelectedImageIndex = 0
                };

                textureTreeView.Nodes.Add(groupNode);

                List<TextureItem> textures = textureManager.GetTexturesByGroup(groupName);
                textures?.ForEach((t) =>
                {
                    TreeNode node = new TreeNode
                    {
                        Name = t.Id,
                        Text = t.Name,
                        Tag = NodeType.Texture,
                        ImageIndex = 1,
                        SelectedImageIndex = 1
                    };

                    groupNode.Nodes.Add(node);
                });
            }
        }

        private void OnDrawListViewItem(object sender, DrawListViewItemEventArgs e)
        {
            Brush backgroundBrush = new SolidBrush(Color.FromArgb(255, 16, 16, 64));
            Brush nameBrush = System.Drawing.Brushes.White;
            int leftOffset = (e.Bounds.Width - textureDimensions) / 2;
            bool isSelected = e.State.HasFlag(ListViewItemStates.Selected) || e.Item.Name == (lastSelectedTexture.HasValue ? lastSelectedTexture.Value.Id : "");
            if (isSelected)
            {
                backgroundBrush = new SolidBrush(Color.FromArgb(255, 64, 64, 200));
            }

            // draw texture
            Image image = textureImageList.Images[e.Item.ImageIndex];
            e.Graphics.DrawImage(image, e.Bounds.X + leftOffset, e.Bounds.Y);

            // draw name background
            Rectangle nameBackgroundRect = new Rectangle
            {
                X = e.Bounds.X + leftOffset,
                Y = e.Bounds.Y + textureDimensions,
                Width = textureDimensions,
                Height = e.Bounds.Height - textureDimensions
            };

            e.Graphics.FillRectangle(backgroundBrush, nameBackgroundRect);

            // draw name
            using (Font font = new Font("Segoe UI", 8f, FontStyle.Regular, GraphicsUnit.Point))
            {
                SizeF textSize = e.Graphics.MeasureString(e.Item.Text, font);
                float textHeightOffset = (e.Bounds.Height - (textureDimensions + textSize.Height)) / 2;
                PointF fontPositon = new PointF(e.Bounds.X + leftOffset + 8, e.Bounds.Y + textureDimensions + textHeightOffset);

                e.Graphics.DrawString(e.Item.Text, font, nameBrush, fontPositon);
            }

            if (isSelected)
            {
                e.Graphics.DrawLine(Pens.White, e.Bounds.X + leftOffset, e.Bounds.Y + textureDimensions, 
                    e.Bounds.X + textureDimensions + leftOffset, e.Bounds.Y + textureDimensions);
                e.Graphics.DrawRectangle(Pens.White, new Rectangle
                {
                    X = e.Bounds.X + leftOffset,
                    Y = e.Bounds.Y,
                    Width = textureDimensions,
                    Height = e.Bounds.Height-1
                });
            }

            backgroundBrush.Dispose();
        }

        private void textureListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            TextureCollection textureManager = textureCollection;
            ListViewItem item = new ListViewItem();
            TextureItem textureItem = textureManager.GetTexturesByGroup(currentGroupName)[e.ItemIndex];
            string itemId = $"{textureItem.Id}";

            if (!textureImageList.Images.ContainsKey(itemId))
                textureImageList.Images.Add(itemId, textureItem.Image);

            item.Name = itemId;
            item.Text = textureItem.Name;
            item.ForeColor = Color.White;
            item.ImageKey = itemId;
            item.ImageIndex = textureImageList.Images.IndexOfKey(itemId);
            e.Item = item;
        }

        private void textureListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView senderList = (ListView)sender;
            ListViewItem clickedItem = senderList.HitTest(e.Location).Item;
            if (clickedItem != null)
            {
                TextureCollection textureManager = textureCollection;
                TextureItem? textureItem = textureManager.GetTextureItemById(clickedItem.Name);

                if (textureItem.HasValue)
                {
                    textureCollection.SelectedTexture = textureItem.Value;
                    OnTextureSelected?.Invoke(textureItem.Value);
                }
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (lastSelectedTexture.HasValue)
            {
                textureCollection.SelectedTexture = lastSelectedTexture.Value;
                OnTextureSelected?.Invoke(lastSelectedTexture.Value);
            }

            Close();
        }
        private void applyButton_Click(object sender, EventArgs e)
        {
            if (lastSelectedTexture.HasValue)
            {
                textureCollection.SelectedTexture = lastSelectedTexture.Value;
                OnTextureSelected?.Invoke(lastSelectedTexture.Value);
                OnApplyPressed?.Invoke(lastSelectedTexture.Value);
            }

            Close();
        }

        private void textureListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            TextureItem? textureItem = textureCollection.GetTextureItemById(e.Item.Name);
            if (textureItem.HasValue)
                lastSelectedTexture = textureItem.Value;
        }

        private void textureTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NodeType type = (NodeType)e.Node.Tag;
            TextureCollection textureManager = textureCollection;
            textureImageList.Images.Clear();

            switch (type)
            {
                case NodeType.Directory:
                    if (currentGroupName != e.Node.Name)
                    {
                        currentGroupName = e.Node.Name;
                        lastSelectedTexture = null;
                    }
                    break;
                case NodeType.Texture:
                    TextureItem? texture = textureManager.GetTextureItemById(e.Node.Name);
                    if (texture.HasValue)
                    {
                        currentGroupName = texture.Value.Group;
                    }
                    break;
            }

            textureListView.VirtualListSize = textureManager.GetTexturesByGroup(currentGroupName).Count;
        }

        private void OnTextureCreated(TextureItem texture)
        {
            if (!IsHandleCreated)
                return;

            BeginInvoke(new Action(() =>
            {
                TreeNode[] groupNodes = textureTreeView.Nodes.Find(texture.Group, false);
                if (groupNodes.Length > 0)
                {
                    TreeNode groupNode = groupNodes[0];
                    TreeNode newTextureNode = new TreeNode
                    {
                        Name = texture.Id,
                        Text = texture.Name,
                        Tag = NodeType.Texture
                    };

                    groupNode.Nodes.Add(newTextureNode);
                    textureTreeView.Invalidate();
                    textureListView.Refresh();
                }
            }));
        }
    
        private void OnTextureDeleted(TextureItem texture)
        {
            if (!IsHandleCreated)
                return;

            BeginInvoke(new Action(() =>
            {
                TreeNode[] groupNodes = textureTreeView.Nodes.Find(texture.Group, false);
                if (groupNodes.Length > 0)
                {
                    TreeNode groupNode = groupNodes[0];
                    TreeNode[] textureNode = groupNode.Nodes.Find(texture.Id, false);
                    if (textureNode.Length > 0)
                    {
                        textureNode[0].Remove();
                        textureTreeView.Invalidate();
                        textureListView.Refresh();
                    }
                }
            }));
        }

        private void OnTextureRenamed(string oldId, TextureItem texture)
        {
            if (!IsHandleCreated)
                return;

            BeginInvoke(new Action(() =>
            {
                TreeNode[] groupNodes = textureTreeView.Nodes.Find(texture.Group, false);
                if (groupNodes.Length > 0)
                {
                    TreeNode groupNode = groupNodes[0];
                    TreeNode[] textureNode = groupNode.Nodes.Find(oldId, false);

                    if (textureNode.Length > 0)
                    {
                        textureNode[0].Name = texture.Id;
                        textureNode[0].Text = texture.Name;
                        textureTreeView.Invalidate();
                        textureListView.Refresh();
                    }
                }
            }));
        }
    }
}
