using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RuneGear.General;

namespace RuneGear.Forms
{
    public partial class SettingsDialog : Form
    {
        private readonly EditorSettings mSettings;

        public SettingsDialog(EditorSettings settings)
        {
            InitializeComponent();

            mSettings = settings;

            SetColors();
        }

        private void SetColors()
        {
            foreach (KeyValuePair<string, Color> colorKeyValue in mSettings.Colors)
            {
                ListViewItem colorItem = new ListViewItem();

                ListViewItem.ListViewSubItem colorValue = new ListViewItem.ListViewSubItem();
                colorValue.BackColor = colorKeyValue.Value;
                colorValue.Text = colorKeyValue.Value.ToString();
                colorItem.SubItems.Add(colorValue);

                colorItem.Text = colorKeyValue.Key;
                colorListView.Items.Add(colorItem);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            mSettings.Save();
        }

        private void okeButton_Click(object sender, EventArgs e)
        {
            mSettings.Save();
            Close();
        }
    }

    public class ColorListSubViewItem : ListViewItem.ListViewSubItem
    {
        Button colorButton;

        public ColorListSubViewItem()
        {
            colorButton = new Button();
        }
    }
}