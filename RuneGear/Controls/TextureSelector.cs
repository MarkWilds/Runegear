using System;
using System.Windows.Forms;
using RuneGear.Forms;
using RuneGear.General;

namespace RuneGear.Controls
{
    public partial class TextureSelector : UserControl
    {
        public TextureCollection TextureCollection { get; set; }

        public TextureItem CurrentTextureItem { get; set; }

        public Button ApplyButton => applyButton;
        public Button ReplaceButton => replaceButton;

        public TextureSelector()
        {
            InitializeComponent();
        }

        private void TextureSelector_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                TextureCollection.OnTextureLoadingProgress += OnTextureLoadingProgress;
        }

        private void OnTextureLoadingProgress(string message, int progress)
        {
            if (progress >= 100)
            {
                if (TextureCollection.SelectedTexture.HasValue)
                {
                    TextureItem selectedTexture = TextureCollection.SelectedTexture.Value;
                    CurrentTextureItem = selectedTexture;
                    BeginInvoke(new Action(() =>
                    {
                        texturePreview.Image = selectedTexture.Image;
                        textureName.Text = selectedTexture.Name;
                    }));
                }
            }
        }

        private void OnBrowseButtonClicked(object sender, EventArgs e)
        {
            TextureBrowserDialog textureBrowser = new TextureBrowserDialog(TextureCollection);
            textureBrowser.OnTextureSelected += OnTextureBrowserTextureSelected;
            textureBrowser.OnApplyPressed += OnTextureBrowserApplyPressed;
            textureBrowser.ShowDialog();
        }

        private void OnTextureBrowserApplyPressed(TextureItem textureItem)
        {
            applyButton.PerformClick();
        }

        private void OnTextureBrowserTextureSelected(TextureItem textureItem)
        {
            CurrentTextureItem = textureItem;
            textureName.Text = textureItem.Name;
            texturePreview.Image = textureItem.Image;
        }
    }
}
