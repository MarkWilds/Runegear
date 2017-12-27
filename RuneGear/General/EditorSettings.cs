using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using RuneGear.FileSystem;

namespace RuneGear.General
{
    public class EditorSettings
    {
        private readonly string settingsPath;
        private IniParser settingsParser;

        // settings not public to users
        public string TextureFolder { get; private set; }

        //settings
        public Dictionary<string, Color> Colors { get; set; }
        public int HideLinesLower { get; set; }
        public int MajorLineEvery { get; set; }
        
        public bool GlobalTextureLock { get; set; }

        public EditorSettings()
        {
            Colors = new Dictionary<string, Color>();
            settingsPath = AppDomain.CurrentDomain.BaseDirectory + "settings.ini";
            SetInternEditorSettings();
        }

        public void SetInternEditorSettings()
        {
            TextureFolder = "Textures";
        }

        /// <summary>
        /// Load the settings from ini file.
        /// </summary>
        public void Load()
        {
            try
            {
                settingsParser = new IniParser();
                settingsParser.InitializeParser(settingsPath);
                InitializeSettings();
            }
            catch (FileNotFoundException ex)
            {
                // could not find the file
                ResetDefaults();
                Save();
            }
        }

        public void Save()
        {
            settingsParser.SaveSettings();
        }

        private void InitializeSettings()
        {
            // load general
            HideLinesLower = settingsParser.GetSetting<int>("General", "hideLinesLower");
            MajorLineEvery = settingsParser.GetSetting<int>("General", "majorLineEvery");

            // load colors
            string[] colorArray = settingsParser.EnumSection("Colors");

            foreach (string colorKey in colorArray)
            {
                string rgb = settingsParser.GetSetting<string>("Colors", colorKey);
                string[] rgbValues = rgb.Split(new char[] {','}, 3);
                int r = Int32.Parse(rgbValues[0]);
                int g = Int32.Parse(rgbValues[1]);
                int b = Int32.Parse(rgbValues[2]);

                Colors.Add(colorKey, Color.FromArgb(255, r, g, b));
            }
        }

        public void ResetDefaults()
        {
            // Add general
            HideLinesLower = 4;
            MajorLineEvery = 8;

            settingsParser.AddSetting("General", "hideLinesLower", HideLinesLower.ToString());
            settingsParser.AddSetting("General", "majorLineEvery", MajorLineEvery.ToString());

            // Add colors
            Colors.Clear();
            Colors.Add("minorGridColor", Color.FromArgb(255, 32, 32, 32));
            Colors.Add("majorGridColor", Color.FromArgb(255, 64, 64, 64));
            Colors.Add("originGridColor", Color.FromArgb(255, 192, 192, 192));
            Colors.Add("selectBrushColor", Color.Red);
            Colors.Add("createBrushColor", Color.Yellow);

            foreach (KeyValuePair<string, Color> keyValuePair in Colors)
            {
                Color color = keyValuePair.Value;
                string rgb = color.R + "," + color.G + "," + color.B;

                settingsParser.AddSetting("Colors", keyValuePair.Key, rgb);
            }
        }

        public Color GetColor(string key)
        {
            if (Colors.ContainsKey(key))
                return Colors[key];

            return Color.White;
        }
    }
}