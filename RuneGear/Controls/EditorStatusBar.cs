using System;
using System.Windows.Forms;
using OpenTK;
using RuneGear.General.Viewport;
using RuneGear.MapObjects;

namespace RuneGear.Controls
{
    public partial class EditorStatusBar : ToolStrip
    {
        private const string StandardMessageText = "Click and drag to draw an object";
        private const string GridUnitsText = "Grid: {0} units";
        private const string ZoomText = "Zoom: {0}%";
        private const string PositionText = "Position: {0}, {1}, {2}";
        private const string DimensionsText = "Dimensions: {0} x {1} x {2}";

        public string Message
        {
            set
            {
                editorStatusbarText.Text = value;
            }
        }

        public EditorStatusBar()
        {
            InitializeComponent();
            editorStatusbarText.Text = StandardMessageText;
        }

        public void UpdateStatusBar(BaseViewport viewport)
        {
            editorStatusbarGrid.Text = viewport.GridSize > 0 ? string.Format(GridUnitsText, viewport.GridSize) : string.Empty;
            editorStatusbarZoom.Text = viewport.Zoom > 0.0f ? string.Format(ZoomText, Math.Round(1.0f / viewport.Zoom * 100.0f, 2)) : string.Empty;

            MapObjectGroup selectedMapObjectGroup = viewport.Controller.Selection;
            if(selectedMapObjectGroup.Empty)
            {
                ResetData();
                return;
            }

            Vector3 center = selectedMapObjectGroup.Bounds.Center;
            editorStatusbarPositon.Text = string.Format(PositionText, Math.Round(center.X, 0), Math.Round(center.Y, 0), Math.Round(center.Z, 0));

            Vector3 dimensions = selectedMapObjectGroup.Bounds.Max - selectedMapObjectGroup.Bounds.Min;
            editorStatusbarDimensions.Text = string.Format(DimensionsText, Math.Round(dimensions.X, 0), Math.Round(dimensions.Y, 0), Math.Round(dimensions.Z, 0));
        }

        public void ResetData()
        {
            editorStatusbarPositon.Text = string.Empty;
            editorStatusbarDimensions.Text = string.Empty;
        }
    }
}
