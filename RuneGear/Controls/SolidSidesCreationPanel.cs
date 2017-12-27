using System.Windows.Forms;

namespace RuneGear.Controls
{
    public partial class SolidSidesCreationPanel : UserControl
    {
        public SolidSidesCreationPanel()
        {
            InitializeComponent();
        }

        public int Sides => (int)numericSides.Value;
    }
}
