using RuneGear.MapObjects.SolidMapObject;

namespace RuneGear.MapObjects
{
    public interface IMapObjectOperation
    {
        void Visit(Solid solid);
        void Visit(RubberBand mapObject);
        void Visit(MapObjectGroup mapObjectGroup);
    }
}