using System;
using System.Collections;
using System.Collections.Generic;
using RuneGear.MapObjects;

namespace RuneGear.General
{
    public class SceneDocument : IEnumerable<MapObject>
    {
        // Events
        public class DirtyArgs : EventArgs
        {
            public bool IsDirty { get; private set; }

            public DirtyArgs(bool isDirty)
            {
                IsDirty = isDirty;
            }
        }

        // is called when the dirty state of the document changes
        public event EventHandler<DirtyArgs> OnDirty;

        // Consts
        private const string DefaultMapName = "Unknown";

        // General
        private readonly List<MapObject> mapObjects;

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set { SetDirty(value); }
        }

        public string MapName { get; set; }
        public string AbsoluteFileName { get; set; }

        public SceneDocument()
            : this(DefaultMapName) { }

        public SceneDocument(string name)
        {
            MapName = name;
            mapObjects = new List<MapObject>();
            SetDirty(false);
        }

        public void AddMapObject(MapObject mapObject)
        {
            mapObjects.Add(mapObject);
            SetDirty(true);
        }

        public void RemoveMapObject(MapObject mapObject)
        {
            mapObjects.Remove(mapObject);
            SetDirty(true);
        }

        public void Clear()
        {
            MapName = DefaultMapName;
            mapObjects.Clear();
            SetDirty(false);
        }

        private void SetDirty(bool dirty)
        {
            isDirty = dirty;
            OnDirty?.Invoke(this, new DirtyArgs(IsDirty));
        }

        public IEnumerator<MapObject> GetEnumerator()
        {
            return mapObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
