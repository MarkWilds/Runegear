using System.Windows.Forms;
using OpenTK;
using RuneGear.General.Viewport;
using RuneGear.MapObjects;
using RuneGear.Tools;

namespace RuneGear.General
{
    public enum SolidManipulationMode
    {
        Solid = 0,
        Face,
        Vertex
    }

    public interface IEditorController
    {
        SceneDocument SceneDocument { get; }

        EditorSettings EditorSettings { get; }

        RubberBand RubberBand { get; }

        MapObjectGroup Selection { get; }

        BaseTool CurrentTool { get; }

        TextureCollection TextureCollection { get; }

        SolidManipulationMode CurrentSolidManipulationMode { get; set; }

        void UpdateUserInterface();

        void RenderViewports();

        void SetCursor(Cursor cursor);

        void CreateSolid(Matrix4 transform);

        MapObject GetMapObjectIfHit(int x, int y, BaseViewport viewport);
    }
}