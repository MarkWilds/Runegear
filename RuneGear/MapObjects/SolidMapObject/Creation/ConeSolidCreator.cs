using System;
using OpenTK;
using RuneGear.Controls;
using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Creation
{
    public class ConeSolidCreator : SolidCreator<Solid>
    {
        private readonly SolidSidesCreationPanel sidesPropertyControl;

        public ConeSolidCreator(string name, SolidSidesCreationPanel creatorControl)
            :base(name) 
        {
            sidesPropertyControl = creatorControl;
        }

        public override Solid CreateSolid(AABB bounds)
        {
            int piePieces = sidesPropertyControl.Sides;
            Solid solidCone = new Solid();
            SolidFace bottomFace = new SolidFace();

            // create 4 corners points for the box
            float halfWidth = (bounds.Max.X - bounds.Min.X) / 2.0f;
            float halfHeight = (bounds.Max.Y - bounds.Min.Y) / 2.0f;
            float halfDepth = (bounds.Max.Z - bounds.Min.Z) / 2.0f;

            solidCone.Bounds = (AABB) bounds.Clone();
            solidCone.VertexPositions.Add(new Vector3(bounds.Center.X, bounds.Center.Y, -halfDepth));

            float degToRad = (float)(Math.PI / 180.0);
            float radPiece = (360.0f / piePieces * degToRad);

            // create vertices
            for (int i = 0; i < piePieces; i++)
            {
                SolidIndex index = new SolidIndex();
                Vector3 position = new Vector3
                {
                    X = (float) Math.Cos(i*-radPiece)*halfWidth,
                    Y = (float) Math.Sin(i*-radPiece)*halfHeight,
                    Z = halfDepth
                };
                index.Index = i + 1;

                solidCone.VertexPositions.Add(bounds.Center + position);
                bottomFace.Indices.Add(index);
            }

            solidCone.Faces.Add(bottomFace);
            int vertexCount = solidCone.VertexPositions.Count;

            // generate body
            for (int i = 0; i < piePieces; i++)
            {
                int index = i + 1;
                int nextIndex = index + 1;
                if (nextIndex == vertexCount)
                    nextIndex = 1;

                SolidFace bodyFace = new SolidFace();

                SolidIndex a = new SolidIndex { Index = 0 };
                SolidIndex b = new SolidIndex { Index = index };
                SolidIndex c = new SolidIndex { Index = nextIndex };

                bodyFace.Indices.Add(c);
                bodyFace.Indices.Add(b);
                bodyFace.Indices.Add(a);

                solidCone.Faces.Add(bodyFace);
            }

            return solidCone;
        }
    }
}
