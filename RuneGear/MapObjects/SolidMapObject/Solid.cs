using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using RuneGear.Controls;
using RuneGear.Geometry;
using RuneGear.Utilities.Extensions;

namespace RuneGear.MapObjects.SolidMapObject
{
    /// <summary>
    /// Concrete implementations of the map object
    /// Represents a solid convex primitive
    /// </summary>
    public class Solid : MapObject
    {
        public Color Color { get; set; }
        public bool Detail { get; set; }
        public bool Hidden { get; set; }
        public List<Vector3> VertexPositions { get; }
        public List<int> SelectedVertices { get; }
        public List<SolidFace> Faces { get; }

        public override bool Selected
        {
            get { return Faces.Any(f => f.Selected); }
            set { Faces.ForEach(f => f.Selected = value); }
        }

        public Solid()
        {
            Faces = new List<SolidFace>();
            VertexPositions = new List<Vector3>();
            SelectedVertices = new List<int>();
            Color = Color.GetRandomBrushColor();
            Detail = false;
            Hidden = false;
        }

        public void RegenerateBounds()
        {
            Bounds.Reset();

            foreach (Vector3 position in VertexPositions)
            {
                Bounds.Grow(position);
            }
        }

        public void CalculateNormals()
        {
            if (VertexPositions.Count < 3)
                return;

            foreach (SolidFace face in Faces)
            {
                if (face.Indices.Count < 3)
                    continue;

                // get correct indices
                int indexOne = face.Indices[0].Index;
                int indexTwo = face.Indices[1].Index;
                int indexThree = face.Indices[2].Index;

                // based on the fact that we use counter clockwise winding
                Vector3 vector = VertexPositions[indexOne];
                Vector3 vectorA = VertexPositions[indexTwo];
                Vector3 vectorB = VertexPositions[indexThree];

                // B X A
                face.Normal = Vector3.Cross(vectorB - vector, vectorA - vector).Normalized();
            }
        }

        public SolidVertex[] GetVerticesForFace(SolidFace face)
        {
            List<SolidVertex> vertices = new List<SolidVertex>();
            foreach (SolidIndex vertexIndex in face.Indices)
            {
                if (vertexIndex.Index > VertexPositions.Count)
                    continue;

                SolidVertex vertex = new SolidVertex
                {
                    DiffuseUv = vertexIndex.DiffuseUv,
                    Position = VertexPositions[vertexIndex.Index]
                };
                vertices.Add(vertex);
            }

            return vertices.ToArray();
        }

        /// <summary>
        /// Align texture axis to a world plane
        /// U is to the right and V is down,
        /// Opengl uses U to the right en V is up
        /// </summary>
        /// <param name="face"></param>
        public void AlignTextureAxisToWorldForFace(SolidFace face)
        {
            if (face.TextureMapping.TextureLocked)
                return;

            Vector3 axisNormal = new Plane(face.Normal, 0.0f).GetClosestAxisForPlaneNormal();

            // generate uv axis
            face.TextureMapping.UAxis = axisNormal == Vector3.UnitX ? Vector3.UnitZ : Vector3.UnitX;
            face.TextureMapping.VAxis = axisNormal == Vector3.UnitY ? -Vector3.UnitZ : -Vector3.UnitY;

            // check if V axis is 90 degrees counterclockwise of U axis else make it
            // for our perspective it is clockwise, but positive rotation mathematically is counterclockwise
            if (face.TextureMapping.VAxis.SignedAngleTo(face.TextureMapping.UAxis, face.Normal) > 0)
                face.TextureMapping.UAxis = -face.TextureMapping.UAxis;

            face.TextureMapping.Rotation = 0;
        }

        private void MinimiseTextureShiftValuesForFace(SolidFace face)
        {
            face.TextureMapping.UShift = face.TextureMapping.UShift % face.Texture.Width;
            face.TextureMapping.VShift = face.TextureMapping.VShift % face.Texture.Height;

            if (face.TextureMapping.UShift < -face.Texture.Width / 2f)
                face.TextureMapping.UShift += face.Texture.Width;

            if (face.TextureMapping.VShift < -face.Texture.Height / 2f)
                face.TextureMapping.VShift += face.Texture.Height;
        }

        /// <summary>
        /// Calculate texture coordinates with face texturemapping values
        /// </summary>
        /// <param name="face"></param>
        /// <param name="minimiseShiftValues"></param>
        public void CalculateTextureCoordinatesForFace(SolidFace face, bool minimiseShiftValues = false)
        {
            if (face.TextureMapping.TextureLocked)
                return;

            TextureMapping mapping = face.TextureMapping;
            foreach (SolidIndex vertexIndex in face.Indices)
            {
                Vector3 position = VertexPositions[vertexIndex.Index];
                double u = (Vector3.Dot(position, mapping.UAxis) / mapping.UScale - mapping.UShift) /
                           face.Texture.Width;
                double v = (Vector3.Dot(position, mapping.VAxis) / mapping.VScale + mapping.VShift) /
                           face.Texture.Height;

                vertexIndex.DiffuseUv = new Vector2((float) u, (float) v);
            }

            if (minimiseShiftValues)
                MinimiseTextureShiftValuesForFace(face);
        }

        /// <summary>
        /// Rotates the texture
        /// </summary>
        /// <param name="face"></param>
        /// <param name="rotate">Absolute Angle in degrees</param>
        public void SetTextureRotationForFace(SolidFace face, float rotate)
        {
            if (face.TextureMapping.TextureLocked)
                return;

            AlignTextureAxisToWorldForFace(face);
            face.TextureMapping.Rotation = rotate;
            Vector3 textureNormal = Vector3.Cross(face.TextureMapping.VAxis, face.TextureMapping.UAxis);
            Matrix3 rotationMatrix = Matrix3.CreateFromAxisAngle(textureNormal, MathHelper.DegreesToRadians(-rotate));

            face.TextureMapping.UAxis = face.TextureMapping.UAxis * rotationMatrix;
            face.TextureMapping.VAxis = face.TextureMapping.VAxis * rotationMatrix;
        }

        /// <summary>
        /// Fits texture to extends for a given face
        /// </summary>
        /// <param name="extends"></param>
        /// <param name="face"></param>
        public void FitTextureToExtendsForFace(AABB extends, SolidFace face)
        {
            if (face.TextureMapping.TextureLocked)
                return;

            // get projected min/max
            float projMinU = Vector3.Dot(extends.Min, face.TextureMapping.UAxis);
            float projMinV = Vector3.Dot(extends.Min, face.TextureMapping.VAxis);

            float projMaxU = Vector3.Dot(extends.Max, face.TextureMapping.UAxis);
            float projMaxV = Vector3.Dot(extends.Max, face.TextureMapping.VAxis);

            // calculate new texture mapping properties
            face.TextureMapping.UScale = Math.Abs((projMaxU - projMinU) / face.Texture.Width);
            face.TextureMapping.VScale = Math.Abs((projMaxV - projMinV) / face.Texture.Height);
            face.TextureMapping.UShift = -projMinU / face.TextureMapping.UScale;
            face.TextureMapping.VShift = -projMinV / face.TextureMapping.VScale;
        }

        /// <summary>
        /// Align texture to extends for a given face
        /// </summary>
        /// <param name="extends"></param>
        /// <param name="face"></param>
        /// <param name="justifyType"></param>
        public void AlignTextureToExtendsForFace(AABB extends, SolidFace face,
            TextureProperties.JustifyType justifyType)
        {
            if (face.TextureMapping.TextureLocked)
                return;

            // get projected min/max
            float projMinU = Vector3.Dot(extends.Min, face.TextureMapping.UAxis) / face.TextureMapping.UScale;
            float projMinV = Vector3.Dot(extends.Min, face.TextureMapping.VAxis) / face.TextureMapping.VScale;

            float projMaxU = Vector3.Dot(extends.Max, face.TextureMapping.UAxis) / face.TextureMapping.UScale;
            float projMaxV = Vector3.Dot(extends.Max, face.TextureMapping.VAxis) / face.TextureMapping.VScale;
            
            // check if V axis is 90 degrees lockwise of U axis
            if (face.TextureMapping.VAxis.SignedAngleTo(face.TextureMapping.UAxis, face.Normal) < 0)
            {
                float temp = projMinU;
                projMinU = projMaxU;
                projMaxU = temp;
            }

            switch (justifyType)
            {
                case TextureProperties.JustifyType.Top:
                    face.TextureMapping.VShift = -projMaxV + face.Texture.Height;
                    break;
                case TextureProperties.JustifyType.Bottom:
                    face.TextureMapping.VShift = -projMinV;
                    break;
                case TextureProperties.JustifyType.Left:
                    face.TextureMapping.UShift = -projMinU;
                    break;
                case TextureProperties.JustifyType.Right:
                    face.TextureMapping.UShift = -projMaxU + face.Texture.Width;
                    break;
                case TextureProperties.JustifyType.Center:
                    float deltaU = (projMinU + projMaxU) / 2;
                    float deltaV = (projMinV + projMaxV) / 2;

                    face.TextureMapping.UShift = -deltaU + face.Texture.Width / 2f;
                    face.TextureMapping.VShift = -deltaV + face.Texture.Height / 2f;
                    break;
            }
        }

        public override object Clone()
        {
            Solid newSolid = new Solid
            {
                Bounds = (AABB) Bounds.Clone(),
                Color = Color.GetRandomBrushColor(),
                Hidden = Hidden,
                Detail = Detail
            };

            newSolid.VertexPositions.AddRange(VertexPositions);
            for (int i = 0; i < Faces.Count; i++)
            {
                newSolid.Faces.Add((SolidFace) Faces[i].Clone());
            }

            return newSolid;
        }

        public override void PerformOperation(IMapObjectOperation visitor)
        {
            visitor.Visit(this);
        }
    }
}