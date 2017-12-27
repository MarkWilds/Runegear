using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Creation
{
    /// <summary>
    /// Solid map object factory manages the creation of solid map objects
    /// </summary>
    public class SolidFactory : IMapObjectFactory<Solid>
    {
        private readonly Dictionary<string, SolidCreator<Solid>> factoryMap;
        private readonly Dictionary<string, UserControl> creationControlMap;

        public SolidFactory()
        {
            factoryMap = new Dictionary<string,SolidCreator<Solid>>();
            creationControlMap = new Dictionary<string, UserControl>();
        }

        public Solid CreateMapObject(string key, AABB bounds)
        {
            if (bounds == null)
                throw new ArgumentNullException("AABB bounds was null");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key cannot be null or empty");

            if (!factoryMap.ContainsKey(key))
                throw new Exception("This solid factory has not been registered");

            SolidCreator<Solid> creator = factoryMap[key];
            return creator.CreateSolid(bounds);
        }

        public void RegisterCreator(SolidCreator<Solid> creator, UserControl creatorControl)
        {
            if (creator == null)
                throw new ArgumentNullException("SolidCreator<Solid> creator was null");

            if (string.IsNullOrEmpty(creator.Name))
                throw new ArgumentException("name cannot be null or empty");

            if (factoryMap.ContainsKey(creator.Name))
                throw new Exception("Factory with this key already exists");

            creationControlMap[creator.Name] = creatorControl;
            factoryMap[creator.Name] = creator;
        }

        public UserControl GetSolidPropertyControl(string key)
        {
            return !creationControlMap.ContainsKey(key) ? null : creationControlMap[key];
        }
    }
}
