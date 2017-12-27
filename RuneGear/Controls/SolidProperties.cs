using System;
using System.Windows.Forms;
using RuneGear.MapObjects;

namespace RuneGear.Controls
{
    public partial class SolidProperties : UserControl
    {
        public enum PropertyName
        {
            None = 0,
            Detail,
            Hidden
        }

        public Action<SolidProperties, PropertyName> OnPropertiesChanged;

        public bool IsDetail => detailSolid.CheckState == CheckState.Checked;
        public bool IsHidden => hiddenSolid.CheckState == CheckState.Checked;

        public SolidProperties()
        {
            InitializeComponent();
            Enabled = false;
        }

        public void UpdateProperties(MapObjectGroup selection)
        {
            int detailSolids = 0;
            int nonDetailSolids = 0;
            int hiddenSolids = 0;
            int nonHiddenSolids = 0;

            if (!selection.Empty)
            {
                Enabled = true;
                CustomOperation operation = new CustomOperation
                {
                    OnSolid = (solid) =>
                    {
                        // check hidden property
                        if (solid.Hidden)
                            hiddenSolids++;
                        else
                            nonHiddenSolids++;

                        CheckState binairCheckstate = hiddenSolids == 0 ? CheckState.Unchecked : CheckState.Checked;
                        hiddenSolid.CheckState = hiddenSolids > 0 && nonHiddenSolids > 0
                            ? CheckState.Indeterminate
                            : binairCheckstate;

                        // check detail property
                        if (solid.Detail)
                            detailSolids++;
                        else
                            nonDetailSolids++;

                        binairCheckstate = detailSolids == 0 ? CheckState.Unchecked : CheckState.Checked;
                        detailSolid.CheckState = detailSolids > 0 && nonDetailSolids > 0
                            ? CheckState.Indeterminate
                            : binairCheckstate;
                    }
                };
                operation.OnMapObjectGroup = (group) =>
                {
                    group.MapObjectList.ForEach((subGroup) => subGroup.PerformOperation(operation));
                };
                selection.PerformOperation(operation);
            }
            else
            {
                detailSolid.CheckState = CheckState.Unchecked;
                hiddenSolid.CheckState = CheckState.Unchecked;
                Enabled = false;
            }
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = ((CheckBox) sender);
            string controlTag = checkBox.Tag.ToString();

            PropertyName changedValue = PropertyName.Detail;
            if(controlTag == "hidden")
                changedValue = PropertyName.Hidden;
            
            OnPropertiesChanged?.Invoke(this, changedValue);
        }
    }
}
