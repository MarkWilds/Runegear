using OpenTK;
using RuneGear.Geometry;

namespace RuneGear.MapObjects.SolidMapObject.Creation
{
    public class WedgeSolidCreator : SolidCreator<Solid>
    {
        public WedgeSolidCreator(string name)
        : base(name){ }

        public override Solid CreateSolid(AABB bounds)
        {
            Solid brush = new Solid {Bounds = (AABB) bounds.Clone()};

            Vector3 start = bounds.Min;
            Vector3 end = bounds.Max;

            // create 4 top corners
            Vector3 topLeftMax = new Vector3(start.X, end.Y, end.Z);
            Vector3 bottomRightMax = new Vector3(end.X, start.Y, end.Z);
            Vector3 bottomLeftMax = new Vector3(start.X, start.Y, end.Z);

            // create 4 bottom corners
            Vector3 topLeftMin = new Vector3(start.X, end.Y, start.Z);
            Vector3 bottomRightMin = new Vector3(end.X, start.Y, start.Z);
            Vector3 bottomLeftMin = new Vector3(start.X, start.Y, start.Z);

            brush.VertexPositions.Add(topLeftMax);
            brush.VertexPositions.Add(bottomRightMax);
            brush.VertexPositions.Add(bottomLeftMax);

            brush.VertexPositions.Add(topLeftMin);
            brush.VertexPositions.Add(bottomRightMin);
            brush.VertexPositions.Add(bottomLeftMin);

            // right
            brush.Faces.Add(CreateFace(5, 4, 3));

            // left 
            brush.Faces.Add(CreateFace(1, 2, 0));
            
            // bottom
            brush.Faces.Add(CreateFace(2, 1, 4, 5));

            // back
            brush.Faces.Add(CreateFace(2, 5, 3, 0));

            // top
            brush.Faces.Add(CreateFace(4, 1, 0, 3));

            return brush;
        }

        public SolidFace CreateFace(params int[] indices)
        {
            SolidFace face = new SolidFace();

            // save as trianglefan
            foreach(int index in indices)
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
