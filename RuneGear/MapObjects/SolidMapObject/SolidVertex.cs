using OpenTK;

namespace RuneGear.MapObjects.SolidMapObject
{
    /// <summary>
    /// Solid vertex, only used as data object
    /// </summary>
    public struct SolidVertex
    {
        public Vector3 Position { get; set; }
        public Vector2 DiffuseUv { get; set; }
    }
}
