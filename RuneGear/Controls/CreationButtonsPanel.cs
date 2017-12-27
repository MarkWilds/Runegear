using System;
using System.Windows.Forms;

namespace RuneGear.Controls
{
    public partial class CreationButtonsPanel : UserControl
    {
        public Action<object, EventArgs> OnCreationButtonPressed;

        public CreationButtonsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Resets the primitive currently selected to create a solid
        /// </summary>
        public void ResetPrimitiveToolbar()
        {
            creationBoxButton.CheckState = CheckState.Unchecked;
            creationConeButton.CheckState = CheckState.Unchecked;
            creationCylinderButton.CheckState = CheckState.Unchecked;
            creationWedgeButton.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// Updates the GUI editor for solid primitives
        /// </summary>
        /// <param name="code"></param>
        public void UpdatePrimitiveToolbar(string code)
        {
            ResetPrimitiveToolbar();

            switch (code)
            {
                case "box":
                    creationBoxButton.CheckState = CheckState.Checked;
                    break;
                case "cylinder":
                    creationCylinderButton.CheckState = CheckState.Checked;
                    break;
                case "cone":
                    creationConeButton.CheckState = CheckState.Checked;
                    break;
                case "wedge":
                    creationWedgeButton.CheckState = CheckState.Checked;
                    break;
            }
        }

        private void creationButton_Click(object sender, EventArgs e)
        {
            OnCreationButtonPressed?.Invoke(sender, e);
        }
    }
}
