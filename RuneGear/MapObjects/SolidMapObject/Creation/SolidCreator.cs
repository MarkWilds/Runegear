using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Creation
{
    /// <summary>
    /// Allows the creation of a specific brush if extended and registered to a brush factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SolidCreator<T> where T : MapObject
    {
        public string Name { get; }

        protected SolidCreator(string name)
        {
            this.Name = name;
        }

        public abstract T CreateSolid(AABB bounds);
    }
}
