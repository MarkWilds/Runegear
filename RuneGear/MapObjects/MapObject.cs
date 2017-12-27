using System;
using RuneGear.Geometry;

namespace RuneGear.MapObjects
{
    /// <summary>
    /// Base map object
    /// </summary>
    public abstract class MapObject : ICloneable
    {       
        public AABB Bounds { get; set; }

        public abstract bool Selected { get; set; }

        protected MapObject()
        {
            Bounds = new AABB();
        }

        /// <summary>
        /// Clone the map object
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// Accept a visitor to perform operations on this brush
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void PerformOperation(IMapObjectOperation visitor);
    }
}