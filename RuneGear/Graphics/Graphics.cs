using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RuneGear.Geometry;
using RuneGear.Graphics.Buffers;
using RuneGear.Graphics.Shaders;
using RuneGear.Graphics.Textures;
using RuneGear.MapObjects.SolidMapObject;
using RuneGear.Utilities.Extensions;

namespace RuneGear.Graphics
{
    /// <summary>
    /// OpenGL 3 renderer uses !!!counter clockwise winding!!!
    /// </summary>
    public class Graphics
    {
        public enum LineType
        {
            LineNormal = 0,
            LineDashed
        }

        private struct PrimitiveVertex
        {
            public Vector3 position;
            public Vector3 color;
            public Vector3 normal;
            public Vector2 uv;

            public static int SizeInBytes => Vector3.SizeInBytes * 3 + Vector2.SizeInBytes;
        }

        // data members
        private readonly VertexBuffer deviceBuffer;
        private readonly ColorShader colorShader;
        private readonly LitColorShader litColorShader;
        private readonly LitTextureShader litTexturerShader;

        private readonly List<PrimitiveVertex> lineBuffer;
        private readonly List<PrimitiveVertex> polygonBuffer;
        private readonly List<PrimitiveVertex> litPolygonBuffer;
        private readonly Dictionary<Texture2D, List<PrimitiveVertex>> texturedPolygonBuffer;

        private Matrix4 currentViewProjectionMatrix;
        private int vertexArrayObjectId;
        private int reservedStartingSpace;
        private bool beginEndPair;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Graphics()
            : this(1024) { }

        /// <summary>
        /// custom constructor
        /// </summary>
        public Graphics(int reserveVertexCount)
        {
            lineBuffer = new List<PrimitiveVertex>();
            polygonBuffer = new List<PrimitiveVertex>();
            litPolygonBuffer = new List<PrimitiveVertex>();
            texturedPolygonBuffer = new Dictionary<Texture2D, List<PrimitiveVertex>>();

            deviceBuffer = new VertexBuffer();
            colorShader = new ColorShader();
            litColorShader = new LitColorShader();
            litTexturerShader = new LitTextureShader();

            reservedStartingSpace = reserveVertexCount;
            beginEndPair = false;
        }

        /// <summary>
        /// Init the renderer
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            // set standard settings
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);

            // generate global vertex array object
            vertexArrayObjectId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObjectId);

            // create resources
            deviceBuffer.Create(reservedStartingSpace, BufferUsageHint.StreamDraw);
            colorShader.Create();
            litColorShader.Create();
            litTexturerShader.Create();

            return true;
        }

        /// <summary>
        /// Deinitialize
        /// </summary>
        public void DeInit()
        {
            colorShader.Dispose();
            litColorShader.Dispose();
            litTexturerShader.Dispose();
            deviceBuffer.Dispose();

            // dispose of global vao
            GL.DeleteVertexArray(vertexArrayObjectId);
        }

        /// <summary>
        /// Clears the screen with a color
        /// </summary>
        /// <param name="bgColor"></param>
        public void Clear(Color bgColor)
        {
            GL.ClearColor(bgColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Begins the batching
        /// </summary>
        /// <param name="viewProjMat"></param>
        public void BeginDraw(Matrix4 viewProjMat)
        {
            if (beginEndPair)
                return;

            lineBuffer.Clear();
            polygonBuffer.Clear();
            litPolygonBuffer.Clear();
            texturedPolygonBuffer.Clear();

            currentViewProjectionMatrix = viewProjMat;
            beginEndPair = true;
        }

        /// <summary>
        /// Ends the batching
        /// </summary>
        public void EndDraw()
        {
            if (!beginEndPair)
                return;

            beginEndPair = false;

            deviceBuffer.Bind();

            // if we have lighted polygons to draw
            if (litPolygonBuffer.Count > 0)
            {
                litColorShader.Start();
                litColorShader.SetViewProjectionMatrix(currentViewProjectionMatrix);
                litColorShader.SpecifyAttributes(PrimitiveVertex.SizeInBytes);

                deviceBuffer.Data(litPolygonBuffer, litPolygonBuffer.Count * PrimitiveVertex.SizeInBytes, BufferUsageHint.StreamDraw);
                GL.DrawArrays(PrimitiveType.Triangles, 0, litPolygonBuffer.Count);
            }

            // start initializing color shader
            colorShader.Start();
            colorShader.SetViewProjectionMatrix(currentViewProjectionMatrix);
            colorShader.SpecifyAttributes(PrimitiveVertex.SizeInBytes);

            // if we have polygons to draw
            if (polygonBuffer.Count > 0)
            {
                deviceBuffer.Data(polygonBuffer, polygonBuffer.Count * PrimitiveVertex.SizeInBytes, BufferUsageHint.StreamDraw);
                GL.DrawArrays(PrimitiveType.Triangles, 0, polygonBuffer.Count);
            }

            // if we have lines to draw
            if (lineBuffer.Count > 0)
            {
                deviceBuffer.Data(lineBuffer, lineBuffer.Count * PrimitiveVertex.SizeInBytes, BufferUsageHint.StreamDraw);
                GL.DrawArrays(PrimitiveType.Lines, 0, lineBuffer.Count);
            }

            // if we have textured polygons to draw
            if (texturedPolygonBuffer.Count > 0)
            {
                // setup texture shader
                litTexturerShader.Start();
                litTexturerShader.SetViewProjectionMatrix(currentViewProjectionMatrix);
//                litTexturerShader.SetTextureUnit(0);
                litTexturerShader.SpecifyAttributes(PrimitiveVertex.SizeInBytes);

                foreach (var keyValuePair in texturedPolygonBuffer)
                {
                    // bind texture
                    Texture2D texture = keyValuePair.Key;
                    texture.Bind();

                    // draw textures
                    List<PrimitiveVertex> vertexList = keyValuePair.Value;
                    deviceBuffer.Data(vertexList, vertexList.Count * PrimitiveVertex.SizeInBytes, BufferUsageHint.StreamDraw);
                    GL.DrawArrays(PrimitiveType.Triangles, 0, vertexList.Count);
                }
            }
        }

        public void DrawLine(Vector3 start, Vector3 end, LineType type, Color color, float zoomFactor = 1.0f)
        {
            if(!beginEndPair)
            {
                // Begin must be called before end
                return;
            }

            if(type == LineType.LineNormal)
                DrawNormalLine(start, end, color);
            else if (type == LineType.LineDashed)
                DrawDashedLine(start, end, color, zoomFactor);
        }

        private void DrawNormalLine(Vector3 start, Vector3 end, Color color)
        {
            PrimitiveVertex vStart = new PrimitiveVertex();
            PrimitiveVertex vEnd = new PrimitiveVertex();

            // set positions
            vStart.position = start;
            vEnd.position = end;

            // set colors
            vStart.color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
            vEnd.color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);

            lineBuffer.Add(vStart);
            lineBuffer.Add(vEnd);
        }

        private void DrawDashedLine(Vector3 start, Vector3 end, Color color, float zoomFactor = 1.0f)
        {
            PrimitiveVertex vStart = new PrimitiveVertex();
            PrimitiveVertex vEnd = new PrimitiveVertex();

            float lineSize = 4 * zoomFactor;
            float lineLength = (end - start).Length;
            
            int linesToDraw = (int)(lineLength / lineSize) / 2;
            float emptySpace = (lineLength - linesToDraw * lineSize) / (linesToDraw + 1);

            Vector3 lineDir = (end - start);
            lineDir.Normalize();

            // set colors
            vStart.color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
            vEnd.color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);

            start = start + lineDir * emptySpace;

            for (int i = 0; i < linesToDraw; i++)
            {
                vStart.position = start;
                vEnd.position = start + lineDir * lineSize;
                start = vEnd.position + lineDir * emptySpace;

                lineBuffer.Add(vStart);
                lineBuffer.Add(vEnd);
            }
        }

        public void DrawRectangle(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight, Vector3 topLeft, LineType type, Color color)
        {
            if (!beginEndPair)
            {
                // Begin must be called before end
                return;
            }

            DrawLine(bottomLeft, bottomRight, type, color);
            DrawLine(bottomRight, topRight, type, color);
            DrawLine(topRight, topLeft, type, color);
            DrawLine(topLeft, bottomLeft, type, color);
        }

        public void DrawSolidRectangle(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight, Vector3 topLeft, Color color)
        {
            if (!beginEndPair)
            {
                // Begin must be called before end
                return;
            }

            PrimitiveVertex a = new PrimitiveVertex();
            PrimitiveVertex b = new PrimitiveVertex();
            PrimitiveVertex c = new PrimitiveVertex();
            PrimitiveVertex d = new PrimitiveVertex();

            Vector3 rectangleColor = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
            a.color = rectangleColor;
            b.color = rectangleColor;
            c.color = rectangleColor;
            d.color = rectangleColor;

            // counter clockwise
            a.position = bottomLeft;
            b.position = bottomRight;
            c.position = topRight;
            d.position = topLeft;

            // create first polygon
            polygonBuffer.Add(a);
            polygonBuffer.Add(c);
            polygonBuffer.Add(d);

            // create second polygon
            polygonBuffer.Add(a);
            polygonBuffer.Add(b);
            polygonBuffer.Add(c);
        }

        public void DrawSolidCircle(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight, Vector3 topLeft, Color color)
        {
            if (!beginEndPair)
            {
                // Begin must be called before end
                return;
            }

            Vector3 position = (topLeft + bottomRight) * 0.5f;
            float radius = (topLeft - topRight).Length / 2.0f;
            Vector2 dimensions = new Vector2
            {
                X = radius,
                Y = radius
            };

            Vector3 hBase = (topRight - topLeft).Normalized();
            Vector3 vBase = (topLeft - bottomLeft).Normalized();

            int piePieces = 12;
            float degToRad = (float)(Math.PI / 180.0);
            float radPiece = (360.0f / piePieces * degToRad);

            Vector3 lastVec = position + hBase * dimensions.X;
            PrimitiveVertex vert = new PrimitiveVertex {color = new Vector3(color.R, color.G, color.B)};

            for (int i = 1; i <= piePieces; i++)
            {
                vert.position = position;
                polygonBuffer.Add(vert);

                vert.position = lastVec;
                polygonBuffer.Add(vert);

                float cos = (float) Math.Cos(i*radPiece)*dimensions.X;
                float sin = (float) Math.Sin(i*radPiece)*dimensions.Y;

                vert.position = position + hBase * cos + vBase * sin;
                polygonBuffer.Add(vert);

                lastVec = vert.position;
            }
        }

        public void DrawWireframeAabb(AABB box, LineType type, Color color, Matrix4 transformationMatrix, float zoomFactor = 1.0f)
        {
            if (!beginEndPair)
            {
                // Begin must be called before end
                return;
            }

            Vector3 min = box.Min;
            Vector3 max = box.Max;

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

            if (transformationMatrix != Matrix4.Identity)
            {
                //translate to local space
                topLeftMax = topLeftMax.TransformL(transformationMatrix);
                topRightMax = topRightMax.TransformL(transformationMatrix);
                bottomRightMax = bottomRightMax.TransformL(transformationMatrix);
                bottomLeftMax = bottomLeftMax.TransformL(transformationMatrix);
                topLeftMin = topLeftMin.TransformL(transformationMatrix);
                topRightMin = topRightMin.TransformL(transformationMatrix);
                bottomRightMin = bottomRightMin.TransformL(transformationMatrix);
                bottomLeftMin = bottomLeftMin.TransformL(transformationMatrix);
            }

            // draw lines between all points to create a cube
            DrawLine(topLeftMax, topRightMax, type, color, zoomFactor);
            DrawLine(topRightMax, bottomRightMax, type, color, zoomFactor);
            DrawLine(bottomRightMax, bottomLeftMax, type, color, zoomFactor);
            DrawLine(bottomLeftMax, topLeftMax, type, color, zoomFactor);

            DrawLine(topLeftMin, topRightMin, type, color, zoomFactor);
            DrawLine(topRightMin, bottomRightMin, type, color, zoomFactor);
            DrawLine(bottomRightMin, bottomLeftMin, type, color, zoomFactor);
            DrawLine(bottomLeftMin, topLeftMin, type, color, zoomFactor);

            DrawLine(topLeftMin, topLeftMax, type, color, zoomFactor);
            DrawLine(topRightMin, topRightMax, type, color, zoomFactor);
            DrawLine(bottomRightMin, bottomRightMax, type, color, zoomFactor);
            DrawLine(bottomLeftMin, bottomLeftMax, type, color, zoomFactor);
        }

        public void DrawWireframeSolid(Solid brush, Color color)
        {
            foreach (SolidFace face in brush.Faces)
            {
                if (face.Indices.Count < 3)
                    continue;

                SolidVertex[] positions = brush.GetVerticesForFace(face);
                DrawWireframeSolidPolygon(positions, face.Normal, color);
            }
        }

        public void DrawWireframeSolidPolygon(SolidVertex[] vertices, Vector3 normal, Color color)
        {
            if (vertices.Length < 3)
                return;

            int vertexCount = vertices.Length;
            for (int i = 0; i < vertexCount; i++)
            {
                DrawLine(vertices[i].Position, vertices[(i + 1) % vertexCount].Position, LineType.LineNormal, color);
            }
        }

        public void DrawSolidPolygon(SolidVertex[] vertices, Vector3 normal, Color color)
        {
            if (vertices.Length < 3)
                return;

            PrimitiveVertex vertex = new PrimitiveVertex
            {
                color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f),
                normal = normal
            };

            // trianglelize polygon and add to lit polygon buffer
            for (int i = 3; i < vertices.Length + 1; i++)
            {
                vertex.position = vertices[0].Position;
                litPolygonBuffer.Add(vertex);

                vertex.position = vertices[i - 2].Position;
                litPolygonBuffer.Add(vertex);

                vertex.position = vertices[i - 1].Position;
                litPolygonBuffer.Add(vertex);
            }
        }

        public void DrawTexturedSolidPolygon(SolidVertex[] vertices, Vector3 normal, Color color, Texture2D texture)
        {
            if (vertices.Length < 3)
                return;

            if (!texturedPolygonBuffer.ContainsKey(texture))
                texturedPolygonBuffer[texture] = new List<PrimitiveVertex>();

            List<PrimitiveVertex> vertexBuffer = texturedPolygonBuffer[texture];
            PrimitiveVertex vertex = new PrimitiveVertex
            {
                color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f),
                normal = normal
            };

            // trianglelize polygon and add to texture vertex polygon buffer
            for (int i = 3; i < vertices.Length + 1; i++)
            {
                vertex.position = vertices[0].Position;
                vertex.uv = vertices[0].DiffuseUv;
                vertexBuffer.Add(vertex);

                vertex.position = vertices[i - 2].Position;
                vertex.uv = vertices[i - 2].DiffuseUv;
                vertexBuffer.Add(vertex);

                vertex.position = vertices[i - 1].Position;
                vertex.uv = vertices[i - 1].DiffuseUv;
                vertexBuffer.Add(vertex);
            }
        }
    }
}
