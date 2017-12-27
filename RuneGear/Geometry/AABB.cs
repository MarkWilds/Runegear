using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK;

namespace RuneGear.Geometry
{
    public class AABB : ICloneable
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        /// <summary>
        /// Gets the center of the box
        /// </summary>
        public Vector3 Center => Min + (Max - Min) * 0.5f;

        /// <summary>
        /// If the box has volume in any of the axis it has volume
        /// </summary>
        public bool HasVolume2D
        {
            get
            {
                // check if the brush has valid bounds
                float absX = Math.Abs(Min.X - Max.X);
                float absY = Math.Abs(Min.Y - Max.Y);
                float absZ = Math.Abs(Min.Z - Max.Z);

                return absX > 0 || absY > 0 || absZ > 0;
            }
        }

        /// <summary>
        /// If the box has volume in all of the axis it has volume
        /// </summary>
        public bool HasVolume3D
        {
            get
            {
                // check if the brush has valid bounds
                float absX = Math.Abs(Min.X - Max.X);
                float absY = Math.Abs(Min.Y - Max.Y);
                float absZ = Math.Abs(Min.Z - Max.Z);

                return absX > 0 && absY > 0 && absZ > 0;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AABB()
        {
            Reset();
        }

        public void Reset()
        {
            Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public void Grow(AABB aabb)
        {
            Grow(aabb.Min);
            Grow(aabb.Max);
        }

        public void Grow(IEnumerable<Vector3> points)
        {
            foreach (Vector3 t in points)
            {
                Grow(t);
            }
        }

        public void Grow(Vector3 vertex)
        {
            Vector3 min = Min;
            Vector3 max = Max;

            min.X = Math.Min(vertex.X, Min.X);
            min.Y = Math.Min(vertex.Y, Min.Y);
            min.Z = Math.Min(vertex.Z, Min.Z);

            max.X = Math.Max(vertex.X, Max.X);
            max.Y = Math.Max(vertex.Y, Max.Y);
            max.Z = Math.Max(vertex.Z, Max.Z);

            Min = min;
            Max = max;
        }

        public object Clone()
        {
            AABB newAabb = new AABB
            {
                Min = Min,
                Max = Max
            };

            return newAabb;
        }
    }
}