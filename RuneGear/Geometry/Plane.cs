using System;
using OpenTK;
using RuneGear.Utilities.Extensions;

namespace RuneGear.Geometry
{
    public class Plane : ICloneable
    {
        public Vector3 Normal { get; }

        public float Distance { get; }

        public Plane(Vector3 norm, float d)
        {
            Normal = norm;
            Distance = d;
        }

        public object Clone()
        {
            return new Plane(Normal, Distance);
        }

        // get world axis from face normal
        public Vector3 GetClosestAxisForPlaneNormal()
        {
            Vector3 norm = Normal.Absolute();
            return norm.X >= norm.Y && norm.X >= norm.Z
                ? Vector3.UnitX
                : (norm.Y >= norm.Z)
                    ? Vector3.UnitY
                    : Vector3.UnitZ;
        }
    }
}
