using System;
using RuneGear.MapObjects.SolidMapObject;

namespace RuneGear.MapObjects
{
    public class CustomOperation : IMapObjectOperation
    {
        public Action<Solid> OnSolid;
        public Action<RubberBand> OnRubberBand;
        public Action<MapObjectGroup> OnMapObjectGroup;

        public void Visit(Solid solid)
        {
            OnSolid?.Invoke(solid);
        }

        public void Visit(RubberBand mapObject)
        {
            OnRubberBand?.Invoke(mapObject);
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            OnMapObjectGroup?.Invoke(mapObjectGroup);
        }
    }
}
