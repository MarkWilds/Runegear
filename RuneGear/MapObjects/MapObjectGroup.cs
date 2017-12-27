using System.Collections;
using System.Collections.Generic;
using RuneGear.Geometry;

namespace RuneGear.MapObjects
{
    public class MapObjectGroup : MapObject, IEnumerable<MapObject>
    {
        public List<MapObject> MapObjectList { get; }

        // is this group temporarly
        public bool IsTransient { get; }
        public bool Empty => MapObjectList.Count <= 0;

        public override bool Selected
        {
            get
            {
                if (MapObjectList.Count <= 0)
                    return false;

                return MapObjectList.TrueForAll((mapObject) => mapObject.Selected);
            }

            set
            {
                MapObjectList.ForEach((mapObject) => mapObject.Selected = value);
            }
        }

        public MapObjectGroup(bool isTransient = true)
        {
            MapObjectList = new List<MapObject>();
            IsTransient = isTransient;
        }

        public void Add(MapObject mapObject)
        {
            if (!MapObjectList.Contains(mapObject))
            {
                Bounds.Grow(mapObject.Bounds);
                MapObjectList.Add(mapObject);
            }
        }

        public void Remove(MapObject mapObject)
        {
            if (MapObjectList.Contains(mapObject))
            {
                MapObjectList.Remove(mapObject);
                RegenerateBounds();
            }
        }

        /// <summary>
        /// Recursively check for containment of the map object
        /// </summary>
        /// <param name="subjectMapObject">The map object to check for in the mapObjectGroup tree (if any)</param>
        /// <returns></returns>
        public bool Contains(MapObject subjectMapObject)
        {
            bool contains = false;
            CustomOperation containsOperation = new CustomOperation();
            containsOperation.OnSolid = solid => contains = solid == subjectMapObject;
            containsOperation.OnMapObjectGroup = mapObjectGroup =>
            {
                foreach (MapObject mapObject in mapObjectGroup)
                {
                    contains = mapObject == subjectMapObject;
                    if (contains)
                        break;

                    mapObject.PerformOperation(containsOperation);
                }
            };
            PerformOperation(containsOperation);

            return contains;
        }

        public void Clear()
        {
            Selected = false;
            MapObjectList.Clear();
            Bounds.Reset();
        }

        public void RegenerateBounds()
        {
            Bounds.Reset();

            foreach (MapObject mapObject in MapObjectList)
            {
                Bounds.Grow(mapObject.Bounds);
            }
        }

        public override object Clone()
        {
            MapObjectGroup newMapObject = new MapObjectGroup();
            newMapObject.Bounds = (AABB) Bounds.Clone();
            foreach (MapObject mapObject in MapObjectList)
            {
                newMapObject.MapObjectList.Add((MapObject)mapObject.Clone());
            }

            return newMapObject;
        }

        public override void PerformOperation(IMapObjectOperation visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerator<MapObject> GetEnumerator()
        {
            return MapObjectList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}