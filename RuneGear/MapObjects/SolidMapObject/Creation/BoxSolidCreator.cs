using OpenTK;
using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Creation
{
    class BoxSolidCreator : SolidCreator<Solid>
    {
        public BoxSolidCreator(string name)
            :base(name) { }

        public override Solid CreateSolid(AABB bounds)
        {
            Solid brush = new Solid {Bounds = (AABB) bounds.Clone()};

            Vector3 min = bounds.Min;
            Vector3 max = bounds.Max;

            // create 4 top corners
            Vector3 topLeftMax = new Vector3(min.X, max.Y, max.Z);
            Vector3 topRightMax = new Vector3(max.X, max.Y, max.Z);
            Vector3 bottomRightMax = new Vector3(max.X, min.Y, max.Z);
            Vector3 bottomLeftMax = new Vector3(min.X, min.Y, max.Z);

            // create 4 bottom corners
            Vector3 topLeftMin = new Vector3(min.X, max.Y, min.Z);
            Vector3 topRightMin = new Vector3(max.X, max.Y, min.Z);
            Vector3 bottomRightMin = new Vector3(max.X, min.Y, min.Z);
            Vector3 bottomLeftMin = new Vector3(min.X, min.Y, min.Z);

            brush.VertexPositions.Add(topLeftMax);
            brush.VertexPositions.Add(topRightMax);
            brush.VertexPositions.Add(bottomRightMax);
            brush.VertexPositions.Add(bottomLeftMax);

            brush.VertexPositions.Add(topLeftMin);
            brush.VertexPositions.Add(topRightMin);
            brush.VertexPositions.Add(bottomRightMin);
            brush.VertexPositions.Add(bottomLeftMin);

            // front
            brush.Faces.Add(CreateFace(7, 6, 5, 4));

            // back
            brush.Faces.Add(CreateFace(2, 3, 0, 1));

            // top
            brush.Faces.Add(CreateFace(4, 5, 1, 0));

            // bottom
            brush.Faces.Add(CreateFace(3, 2, 6, 7));

            // left
            brush.Faces.Add(CreateFace(3, 7, 4, 0));

            // right
            brush.Faces.Add(CreateFace(6, 2, 1, 5));

            return brush;
        }

        public SolidFace CreateFace(params int[] indices)
        {
            SolidFace face = new SolidFace();

            // save as trianglefan
            foreach (int index in indices)
            {
                face.Indices.Add(CreateIndex(index));
            }

            return face;
        }

        private SolidIndex CreateIndex(int index)
        {
            return new SolidIndex { Index = index };
        }
    }
}
