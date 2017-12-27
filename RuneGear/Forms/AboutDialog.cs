using System;
using System.Windows.Forms;

namespace RuneGear.Forms
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void OkeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
