using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using RuneGear.Graphics.Textures;
using RuneGear.Properties;

namespace RuneGear.General
{
    public class TextureCollection : IDisposable
    {
        private const int ImageDimension = 128;
        private readonly string textureFolder;
        private readonly List<TextureItem> defaultTextures;
        private readonly Dictionary<string, TextureItem> texturesById;
        private readonly Dictionary<string, List<TextureItem>> texturesByGroup;
        private readonly Dictionary<string, Texture2D> textureCache;

        public Action<TextureItem> OnTextureCreated;
        public Action<TextureItem> OnTextureDeleted;
        public Action<string, TextureItem> OnTextureRenamed;
        public Action<string, int> OnTextureLoadingProgress;

        public List<TextureItem> Textures => texturesById.Values.ToList();
        public List<string> TextureGroupNames => texturesByGroup.Keys.ToList();
        public TextureItem? SelectedTexture { get; set; }

        public TextureCollection(string folder)
        {
            textureFolder = folder;
            defaultTextures = new List<TextureItem>();
            texturesById = new Dictionary<string, TextureItem>();
            texturesByGroup = new Dictionary<string, List<TextureItem>>();
            textureCache = new Dictionary<string, Texture2D>();

            FillDefaultTextures();
        }

        public List<TextureItem> GetTexturesByGroup(string group)
        {
            return texturesByGroup[group] ?? texturesByGroup[group];
        }

        public TextureItem? GetTextureItemById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            if (texturesById.ContainsKey(id))
                return texturesById[id];

            return null;
        }

        public void Dispose()
        {
            // dispose loaded texture items
            foreach (KeyValuePair<string, TextureItem> keyValuePair in texturesById)
            {
                keyValuePair.Value.Image.Dispose();
            }

            // dispose loaded 2D opengl textures
            foreach (KeyValuePair<string, Texture2D> keyValuePair in textureCache)
            {
                keyValuePair.Value.Dispose();
            }

            textureCache.Clear();
            texturesById.Clear();
            texturesByGroup.Clear();
        }

        public Texture2D LoadTexture2D(string textureId)
        {
            if (textureCache.ContainsKey(textureId))
            {
                return textureCache[textureId];
            }

            TextureItem textureItem = texturesById[textureId];
            Texture2D gpuTexture = textureItem.ToTexture2D();

            textureCache[textureId] = gpuTexture;
            return textureCache[textureId];
        }

        private void FillDefaultTextures()
        {
            string protoFolder = "Prototype";
            string specialFolder = "Special";

            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_00.png", Image = Resources.tile_00});
            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_01.png", Image = Resources.tile_01 });
            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_02.png", Image = Resources.tile_02 });
            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_03.png", Image = Resources.tile_03 });
            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_04.png", Image = Resources.tile_04 });
            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_05.png", Image = Resources.tile_05 });
            defaultTextures.Add(new TextureItem {Group = protoFolder, Name = "tile_06.png", Image = Resources.tile_06 });
            defaultTextures.Add(new TextureItem {Group = specialFolder, Name = "null.png", Image = Resources.nullTexture });
        }

        public async void LoadTextureCollection()
        {
            OnTextureLoadingProgress?.Invoke("Checking on textures", 0);

            await Task.Run(() =>
            {
                if (!Directory.Exists(textureFolder))
                {
                    OnTextureLoadingProgress?.Invoke("Creating texture folder", 0);
                    CreateFolder(textureFolder);
                }

                foreach (int progress in LoadTextures())
                {
                    OnTextureLoadingProgress?.Invoke("Loading Textures", progress);
                }

                OnTextureLoadingProgress?.Invoke("Done", 100);
            });
        }

        /// <summary>
        /// Get group name from the filepath with the assumption that it starts with 'textureFolder' variable
        /// and that group comes after 'textureFolder'
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetGroupFromPath(string filePath)
        {
            string[] splitPath = filePath.Split(Path.DirectorySeparatorChar);

            if (splitPath.Length > 0 && splitPath[0].Contains(textureFolder))
            {
                return splitPath[1];
            }

            return string.Empty;
        }

        private IEnumerable<int> LoadTextures()
        {
            // get al groupFolders
            string[] textureDirectories = Directory.GetDirectories(textureFolder);
            var groupFiles = new Dictionary<string, List<string>>();
            foreach (string folder in textureDirectories)
            {
                string groupName = Path.GetFileNameWithoutExtension(folder);

                if (groupName != null)
                    groupFiles[groupName] = Directory.GetFiles(folder).ToList();
            }

            CreateDefaultTextures(groupFiles);

            // load textures
            int totalTextures = groupFiles.Sum((keyValuePair) => keyValuePair.Value.Count);
            int processedTextures = 1;
            foreach (var keyValuePair in groupFiles)
            {
                List<string> textureFiles = keyValuePair.Value;
                foreach (string texturePath in textureFiles)
                {
                    CreateTexture(keyValuePair.Key, texturePath);
                    yield return (int)((processedTextures++ / (float)totalTextures) * 100);
                }
            }

            // set selected texture if any
            if (totalTextures > 0)
                SelectedTexture = texturesById.Values.First();

            yield return 100;
        }

        public bool CreateFolder(string folder)
        {
            try
            {
                Directory.CreateDirectory(folder);

                return true;
            }
            catch (Exception e)
            {
                // TODO logging
                MessageBox.Show(e.Message);
            }

            return false;
        }

        private Image CreateThumbnail(string filePath)
        {
            // TODO logging and exception handling?
            Image texture = Image.FromFile(filePath);
            return texture.GetThumbnailImage(ImageDimension, ImageDimension, null, IntPtr.Zero);
        }

        private void CreateDefaultTextures(Dictionary<string, List<string>> groupfiles)
        {
            foreach (TextureItem defaultItem in defaultTextures)
            {
                try
                {
                    string filePath = $@"{textureFolder}\{defaultItem.Group}\{defaultItem.Name}";
                    string groupFolder = $@"{textureFolder}\{defaultItem.Group}";

                    if (!Directory.Exists(groupFolder))
                        CreateFolder(groupFolder);
                    else if (File.Exists(filePath))
                        continue;

                    EncoderParameters encoder = new EncoderParameters();
                    defaultItem.Image.Save(filePath, ImageFormat.Png);

                    string group = defaultItem.Group;
                    if(!groupfiles.ContainsKey(group))
                        groupfiles[group] = new List<string>();

                    groupfiles[group].Add(filePath);
                }
                catch (Exception e)
                {
                    // TODO add logging
                }
            }
        }

        // TODO add logging
        private void CreateTexture(string group, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(group))
                return;

            string[] splitPath = filePath.Split(Path.DirectorySeparatorChar);
            if (splitPath.Length < 3)
                return;

            TextureItem textureItem = new TextureItem();
            string textureName = Path.GetFileNameWithoutExtension(filePath);

            textureItem.Id = $@"{group}/{textureName}";
            textureItem.Name = textureName;
            textureItem.Group = group;
            textureItem.FilePath = $"{filePath}";
            textureItem.Image = CreateThumbnail(textureItem.FilePath);

            if(!texturesByGroup.ContainsKey(group))
                texturesByGroup[group] = new List<TextureItem>();

            texturesByGroup[group].Add(textureItem);
            texturesById[textureItem.Id] = textureItem;

            OnTextureCreated?.Invoke(textureItem);
        }

        private void DeleteTexture(string filePath)
        {
            string groupName = GetGroupFromPath(filePath);

            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(groupName))
                return;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string textureId = $@"{groupName}/{fileName}";

            if (texturesById.ContainsKey(textureId))
            {
                TextureItem item = texturesById[textureId];
                item.Image.Dispose();
                texturesById.Remove(textureId);

                OnTextureDeleted?.Invoke(item);
            }
            else
            {
                // TODO add logging
            }
        }

        private void StartFileWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(textureFolder);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
            watcher.Changed += OnFileChanged;
            watcher.Created += OnFileChanged;
            watcher.Deleted += OnFileChanged;
            watcher.Renamed += OnFileRenamed;
            watcher.Filter = "*.png";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                         | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Size;
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            string oldFileName = Path.GetFileNameWithoutExtension(e.OldName);
            string groupName = Path.GetDirectoryName(e.OldName);
            string oldTextureId = $@"{groupName}/{oldFileName}";

            if (texturesById.ContainsKey(oldTextureId))
            {
                TextureItem item = texturesById[oldTextureId];
                string fileName = Path.GetFileNameWithoutExtension(e.Name);
                string newTextureId = $@"{item.Group}/{fileName}";
                item.Name = fileName;
                item.FilePath = e.FullPath;
                item.Id = newTextureId;

                texturesById[newTextureId] = item;
                texturesById.Remove(oldTextureId);

                OnTextureRenamed?.Invoke(oldTextureId, item);
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    CreateTexture(GetGroupFromPath(e.FullPath), e.FullPath);
                    break;
                case WatcherChangeTypes.Deleted:
                    DeleteTexture(e.FullPath);
                    break;
            }
        }
    }
}
