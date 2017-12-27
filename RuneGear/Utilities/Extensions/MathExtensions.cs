using System;
using OpenTK;

namespace RuneGear.Utilities.Extensions
{
    public static class MathExtensions
    {
        public static Matrix4 CreateOrthographicOffCenterLH(float left, float right, float bottom, float top, float near, float far)
        {
            Matrix4 result;
            CreateOrthographicOffCenterLH(left, right, bottom, top, near, far, out result);
            return result;
        }

        public static Matrix4 CreateOrthographicLH(float width, float height, float near, float far)
        {
            Matrix4 result;
            CreateOrthographicOffCenterLH(-width / 2, width / 2, -height / 2, height / 2, near, far, out result);
            return result;
        }

        /// <summary>
        /// Create a left handed orthographic matrix, in row major order
        /// </summary>
        public static void CreateOrthographicOffCenterLH(float left, float right, float bottom, float top, float near, float far, out Matrix4 result)
        {
            result = Matrix4.Identity;

            result.Row0.X = 2.0f / (right - left);
            result.Row1.Y = 2.0f / (top - bottom);
            result.Row2.Z = 2.0f / (far - near);

            result.Row3.X = -(right + left) / (right - left);
            result.Row3.Y = -(top + bottom) / (top - bottom);
            result.Row3.Z = -(far + near) / (far - near);
        }

        /// <summary>
        /// Creates a left handed perspective matrix, in row major order
        /// </summary>
        public static Matrix4 CreatePerspectiveLH(float fov, float width, float height, float near, float far)
        {
            Matrix4 result = Matrix4.Identity;

            float aspect = width / height;
            float degToRad = (float)(Math.PI / 180.0);
            float tanFov = (float)Math.Tan(fov / 2 * degToRad);

            float tan = 1.0f / tanFov;

            result.Row0.X = tan;
            result.Row1.Y = tan * aspect;
            result.Row2.Z = (far + near) / (far - near);
            result.Row2.W = 1;
            result.Row3.Z = -(2 * near * far) / (far - near);

            return result;
        }

        public static double AngleTo(this Vector3 source, Vector3 dest)
        {
            if (source == dest)
            {
                return 0;
            }
            double dot = (Vector3.Dot(source, dest)) / (source.Length * dest.Length);
            return Math.Acos(dot);
        }

        public static double SignedAngleTo(this Vector3 source, Vector3 dest, Vector3 planeNormal)
        {
            var angle = source.AngleTo(dest);
            Vector3 cross = Vector3.Cross(source, dest).Normalized();

            return (Vector3.Dot(cross, planeNormal) < 0 ? -angle : angle);
        }

        public static Vector3 KeepVectorAbsolute(Vector3 vector)
        {
            vector.X = Math.Abs(vector.X);
            vector.Y = Math.Abs(vector.Y);
            vector.Z = Math.Abs(vector.Z);

            return vector;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 TransformL(this Vector3 vec, Matrix4 mat)
        {
            Vector3 result;
            TransformL(vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void TransformL(this Vector3 vec, ref Matrix4 mat, out Vector3 result)
        {
            Vector4 v4 = new Vector4(vec.X, vec.Y, vec.Z, 1.0f);
            Vector4.Transform(ref v4, ref mat, out v4);
            result = v4.Xyz;
        }

        public static Vector3 Absolute(this Vector3 vec)
        {
            return new Vector3(Math.Abs(vec.X), Math.Abs(vec.Y), Math.Abs(vec.Z));
        }
    }
}
