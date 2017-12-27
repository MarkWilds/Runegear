using System;
using OpenTK;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.Utilities;

namespace RuneGear.Geometry
{
    public class Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        #region intersection

        /// <summary>
        /// Check if ray intersects with AABB
        /// Slabs technique
        /// </summary>
        /// <returns></returns>
        public bool IsIntersecting(AABB aabb, float expand = 0.0f)
        {
            float t = 0.0f;
            Vector3 dirFraction;
            Vector3 origin = Origin;
            Vector3 direction = Direction;
            dirFraction.X = 1.0f / direction.X;
            dirFraction.Y = 1.0f / direction.Y;
            dirFraction.Z = 1.0f / direction.Z;

            Vector3 min = aabb.Min - (Vector3.One * expand);
            Vector3 max = aabb.Max + (Vector3.One * expand);

            float t1 = (min.X - origin.X) * dirFraction.X;
            float t2 = (max.X - origin.X) * dirFraction.X;
            float t3 = (min.Y - origin.Y) * dirFraction.Y;
            float t4 = (max.Y - origin.Y) * dirFraction.Y;
            float t5 = (min.Z - origin.Z) * dirFraction.Z;
            float t6 = (max.Z - origin.Z) * dirFraction.Z;

            float tMin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            float tMax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            if (tMax < 0)
            {
                t = tMax;
                return false;
            }

            if (tMin > tMax)
            {
                t = tMax;
                return false;
            }

            t = tMin;
            return true;
        }

        /// <summary>
        /// Checks if ray intersects with a sphere
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="tRef"></param>
        /// <returns></returns>
        public bool IsIntersecting(Vector3 position, float radius, ref float tRef)
        {
            Vector3 m = Origin - position;
            float b = Vector3.Dot(m, Direction);
            float c = Vector3.Dot(m, m) - radius * radius;

            // outside sphere or pointing away
            if (c > 0.0f && b > 0.0f)
                return false;

            float discriminant = b * b - c;

            // ray misses sphere
            if (discriminant < 0.0f)
                return false;

            tRef = (float)(-b - Math.Sqrt(discriminant));

            // if t is negative ray started in the sphere
            if (tRef < 0.0f)
                tRef = 0.0f;

//            Vector3 pointOnSphere = Origin + tRef * Direction;

            return true;
        }

        /// <summary>
        /// Checks if a ray intersects with a convex polygon
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public bool IsIntersecting(params Vector3[] positions)
        {
            if (positions.Length < 3)
                return false;

            Vector3 a = positions[1] - positions[0];
            Vector3 b = positions[2] - positions[0];

            Vector3 polygonNormal = Vector3.Cross(b, a).Normalized();

            float distanceToPlane = Vector3.Dot(polygonNormal, Origin - positions[0]);
            float denom = Vector3.Dot(polygonNormal, Direction);

            if (Math.Abs(denom) > GeneralUtility.Epsilon)
            {
                float t = -distanceToPlane / denom;
                Vector3 pointOnPlane = Origin + t * Direction;

                // check if the point is between the sides of the polygon
                for (int i = 0; i < positions.Length; i++)
                {
                    int nextIndex = i + 1;
                    if (nextIndex == positions.Length)
                        nextIndex = 0;

                    Vector3 sideNormal = Vector3.Cross(polygonNormal, positions[nextIndex] - positions[i]).Normalized();

                    float distanceToSide = Vector3.Dot(pointOnPlane - positions[i], sideNormal);
                    if (distanceToSide > GeneralUtility.Epsilon)
                        return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a ray is colliding with a solid by checking against it planes
        /// Uses ray vs convex polyheron algorithm
        /// We can do this because the solid brushes are always convex!!!
        /// Else we had to use a check against polygon also
        /// </summary>
        /// <param name="solid"></param>
        /// <param name="hitFace"></param>
        /// <param name="tRef"></param>
        /// <returns></returns>
        public bool IsIntersecting(Solid solid, ref SolidFace hitFace,
            ref float tRef)
        {
            float tfirst = 0.0f;
            float tlast = float.MaxValue;
            Vector3 origin = Origin;
            Vector3 direction = Direction;

            foreach (SolidFace face in solid.Faces)
            {
                if (face.Indices.Count < 3)
                    continue;

                int index = face.Indices[0].Index;
                Vector3 pointOnPlane = solid.VertexPositions[index];

                float distanceToPlane = Vector3.Dot(face.Normal, origin - pointOnPlane);
                float denom = Vector3.Dot(direction, face.Normal);

                if (Math.Abs(denom) < GeneralUtility.Epsilon)
                {
                    if (distanceToPlane > 0.0f)
                        return false;
                }
                else
                {
                    float t = -distanceToPlane / denom;
                    if (denom < -GeneralUtility.Epsilon)
                    {
                        if (t > tfirst)
                        {
                            tfirst = t;
                            hitFace = face;
                        }
                    }
                    else
                    {
                        if (t < tlast) tlast = t;
                    }

                    if (tfirst > tlast) return false;
                }
            }

            tRef = tfirst;
            return true;
        }

        #endregion

        /// <summary>
        /// Returns intersection point on ray or null if there is no intersection.
        /// Taken from "Real time collision detection"
        /// </summary>
        public bool IsCloseEnough(Line line, float distance, ref float tRef)
        {
            Vector3 segDirection = line.End - line.Start;
            float a = Direction.LengthSquared;
            float b = Vector3.Dot(Direction, segDirection);
            float e = segDirection.LengthSquared;

            float d = a * e - b * b;

            //lines are not parallel
            if (Math.Abs(d) > GeneralUtility.Epsilon)
            {
                Vector3 r = Origin - line.Start;
                float c = Vector3.Dot(Direction, r);
                float f = Vector3.Dot(segDirection, r);

                float s = (b * f - c * e) / d;
                float t = (a * f - c * b) / d;

                Vector3 closestPointLine1 = Origin + Direction * s;
                Vector3 closestPointLine2 = line.Start + segDirection * t;

                tRef = s;

                if (t <= 0.0f || t >= 1.0f)
                    return false;

                float distanceBetweenPoints = (closestPointLine2 - closestPointLine1).Length;
                if (distanceBetweenPoints > distance)
                    return false;

                return true;
            }

            return false;
        }
    }
}
