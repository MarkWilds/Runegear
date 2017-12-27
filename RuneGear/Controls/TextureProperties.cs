using System;
using System.Linq;
using System.Windows.Forms;
using RuneGear.MapObjects;
using RuneGear.MapObjects.SolidMapObject;

namespace RuneGear.Controls
{
    public partial class TextureProperties : UserControl
    {
        public enum JustifyType
        {
            None = 0,
            Fit,
            Center,
            Left,
            Right,
            Top,
            Bottom
        }

        public enum PropertyName
        {
            None = 0,
            UScale,
            VScale,
            UShift,
            VShift,
            Rotation,
            TextureLock
        }

        public Action<TextureProperties, JustifyType> OnJustify;
        public Action<TextureProperties, PropertyName> OnPropertiesChanged;

        public float UScale => (float)uScaleNumeric.Value;
        public float VScale => (float)vScaleNumeric.Value;
        public float UShift => (float)uShiftNumeric.Value;
        public float VShift => (float)vShiftNumeric.Value;
        public float Rotation => (float)rotationNumeric.Value;
        public bool TreatAsOne => TreatAsOneCheckbox.Checked;
        public bool TextureLocked => textureLockButton.Checked;

        private bool ignoreChangedEvent;

        public TextureProperties()
        {
            InitializeComponent();
            ResetValues();
        }

        private void OnResetButtonPressed(object sender, EventArgs e)
        {
            ResetValues(true);
        }

        private void OnTexturePropertiesChanged(object sender, EventArgs e)
        {
            if (ignoreChangedEvent)
                return;

            Control control = sender as Control;
            if (control == null)
                return;

            PropertyName name = GetPropertyName((string) control.Tag);
            if(name != PropertyName.None)
                OnPropertiesChanged?.Invoke(this, name);
        }

        private void OnJustifyButtonClicked(object sender, EventArgs e)
        {
            Button justifyButton = sender as Button;
            if (justifyButton == null)
                return;

            JustifyType type = GetJustifyType((string)justifyButton.Tag);
            if(type != JustifyType.None)
                OnJustify?.Invoke(this, type);
        }

        public void UpdateProperties(MapObjectGroup selection)
        {
            ignoreChangedEvent = true;
            bool uShiftIsDifferent = false;
            bool vShiftIsDifferent = false;
            bool uScaleIsDifferent = false;
            bool vScaleIsDifferent = false;
            bool rotationIsDifferent = false;
            bool textureLockDifferent = false;
            int faceCount = 0;

            if (!selection.Empty)
            {
                CustomOperation operation = new CustomOperation
                {
                    OnSolid = (solid) => solid.Faces.Where(f => f.Selected).ToList().ForEach((face) =>
                    {
                        faceCount++;

                        uScaleNumeric.Value = (decimal)face.TextureMapping.UScale;
                        if (uScaleNumeric.Value != (decimal) face.TextureMapping.UScale)
                            uScaleIsDifferent = true;

                        vScaleNumeric.Value = (decimal)face.TextureMapping.VScale;
                        if (vScaleNumeric.Value != (decimal)face.TextureMapping.VScale)
                            vScaleIsDifferent = true;

                        uShiftNumeric.Value = (decimal)face.TextureMapping.UShift;
                        if (uShiftNumeric.Value != (decimal)face.TextureMapping.UShift)
                            uShiftIsDifferent = true;

                        vShiftNumeric.Value = (decimal)face.TextureMapping.VShift;
                        if (vShiftNumeric.Value != (decimal)face.TextureMapping.VShift)
                            vShiftIsDifferent = true;

                        rotationNumeric.Value = (decimal)face.TextureMapping.Rotation;
                        if (rotationNumeric.Value != (decimal)face.TextureMapping.Rotation)
                            rotationIsDifferent = true;

                        textureLockButton.Checked = face.TextureMapping.TextureLocked;
                        if (textureLockButton.Checked != face.TextureMapping.TextureLocked)
                            textureLockDifferent = true;
                    })
                };
                operation.OnMapObjectGroup = (group) =>
                {
                    group.MapObjectList.ForEach((subGroup) => subGroup.PerformOperation(operation));
                };
                selection.PerformOperation(operation);
            }
            else
            {
                ResetValues();
            }

            TreatAsOneCheckbox.Enabled = faceCount > 1;

            // check if we need to set default values
            if (uScaleIsDifferent)
                uScaleNumeric.Value = TextureMapping.DefaultScaleValue;

            if (vScaleIsDifferent)
                vScaleNumeric.Value = TextureMapping.DefaultScaleValue;

            if (uShiftIsDifferent)
                vShiftNumeric.Value = TextureMapping.DefaultShiftValue;

            if (vShiftIsDifferent)
                vShiftNumeric.Value = TextureMapping.DefaultShiftValue;

            if (rotationIsDifferent)
                rotationNumeric.Value = TextureMapping.DefaultRotationValue;

            if (textureLockDifferent)
                textureLockButton.Checked = TextureMapping.DefaultTextureLockValue;

            ignoreChangedEvent = false;
        }

        private void ResetValues(bool callEvent = false)
        {
            ignoreChangedEvent = !callEvent;
            uScaleNumeric.Value = TextureMapping.DefaultScaleValue;
            vScaleNumeric.Value = TextureMapping.DefaultScaleValue;
            uShiftNumeric.Value = TextureMapping.DefaultShiftValue;
            vShiftNumeric.Value = TextureMapping.DefaultShiftValue;
            rotationNumeric.Value = TextureMapping.DefaultRotationValue;
            textureLockButton.Checked = TextureMapping.DefaultTextureLockValue;
            ignoreChangedEvent = callEvent;
        }

        private PropertyName GetPropertyName(string tag)
        {
            switch (tag)
            {
                case "uscale":
                    return PropertyName.UScale;
                case "vscale":
                    return PropertyName.VScale;
                case "ushift":
                    return PropertyName.UShift;
                case "vshift":
                    return PropertyName.VShift;
                case "rotation":
                    return PropertyName.Rotation;
                case "textureLock":
                    return PropertyName.TextureLock;
                default:
                    return PropertyName.None;
            }
        }

        private JustifyType GetJustifyType(string tag)
        {
            switch (tag)
            {
                case "fit":
                    return JustifyType.Fit;
                case "center":
                    return JustifyType.Center;
                case "left":
                    return JustifyType.Left;
                case "right":
                    return JustifyType.Right;
                case "top":
                    return JustifyType.Top;
                case "bottom":
                    return JustifyType.Bottom;
                default:
                    return JustifyType.None;
            }
        }
    }
}
