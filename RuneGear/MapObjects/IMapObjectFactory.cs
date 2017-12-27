using RuneGear.Geometry;

namespace RuneGear.MapObjects
{
    /// <summary>
    /// Factory interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IMapObjectFactory<T> where T : MapObject
    {
        T CreateMapObject(string key, AABB bounds);
    }
}
