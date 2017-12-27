using System;
using OpenTK;
using RuneGear.Controls;
using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Creation
{
    public class CylinderSolidCreator : SolidCreator<Solid>
    {
        private readonly SolidSidesCreationPanel sidesPropertyControl;

        public CylinderSolidCreator(string name, SolidSidesCreationPanel creatorControl)
            : base(name)
        {
            sidesPropertyControl = creatorControl;
        }

        public override Solid CreateSolid(AABB bounds)
        {
            int piePieces = sidesPropertyControl.Sides;
            Solid solidCylinder = new Solid();
            SolidFace topFace = new SolidFace();
            SolidFace bottomFace = new SolidFace();

            solidCylinder.Bounds = (AABB) bounds.Clone();
            Vector3 center = bounds.Center;

            // create 4 corners points for the box
            float halfWidth = (bounds.Max.X - bounds.Min.X)/2.0f;
            float halfHeight = (bounds.Max.Y - bounds.Min.Y)/2.0f;
            float halfDepth = (bounds.Max.Z - bounds.Min.Z)/2.0f;

            float degToRad = (float) (Math.PI/180.0);
            float radPiece = (360.0f/piePieces*degToRad);

            // create top and bottom face
            for (int i = 0; i < piePieces; i++)
            {
                SolidIndex topIndex = new SolidIndex();
                SolidIndex bottomIndex = new SolidIndex();
                Vector3 position = new Vector3
                {
                    X = (float) Math.Cos(i*radPiece)*halfWidth,
                    Y = (float) Math.Sin(i*radPiece)*halfHeight
                };

                solidCylinder.VertexPositions.Add(center + new Vector3(position.X, position.Y, -halfDepth));
                solidCylinder.VertexPositions.Add(center + new Vector3(position.X, position.Y, halfDepth));

                topIndex.Index = i*2;
                bottomIndex.Index = piePieces * 2 - (i * 2 + 1);

                topFace.Indices.Add(topIndex);
                bottomFace.Indices.Add(bottomIndex);
            }

            solidCylinder.Faces.Add(topFace);
            solidCylinder.Faces.Add(bottomFace);

            // generate body
            for (int i = 0; i < piePieces; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex == piePieces)
                    nextIndex = 0;

                SolidFace bodyFace = new SolidFace();
                SolidIndex a = new SolidIndex();
                SolidIndex b = new SolidIndex();
                SolidIndex c = new SolidIndex();
                SolidIndex d = new SolidIndex();

                a.Index = i * 2;
                b.Index = i * 2 + 1;
                c.Index = nextIndex * 2 + 1;
                d.Index = nextIndex * 2;

                bodyFace.Indices.Add(a);
                bodyFace.Indices.Add(b);
                bodyFace.Indices.Add(c);
                bodyFace.Indices.Add(d);

                solidCylinder.Faces.Add(bodyFace);
            }

            return solidCylinder;
        }
    }
}