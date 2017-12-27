using System.Drawing;
using OpenTK;
using RuneGear.General;
using RuneGear.General.Viewport;

namespace RuneGear.MapObjects.SolidMapObject.Operations
{
    public class SolidRenderOperation : IMapObjectOperation
    {
        public Graphics.Graphics Graphics { private get; set; }
        public BaseViewport Viewport { private get; set; }

        public void Visit(Solid solid)
        {
            if(Viewport.ViewportType == BaseViewport.ViewportTypes.PERSPECTIVE)
                RenderPerspSolid(solid);
            else
                RenderOrthoSolid(solid);
        }

        private void RenderOrthoSolid(Solid solid)
        {
            IEditorController controller = Viewport.Controller;
            Color solidColor = solid.Selected ? Color.Red : solid.Color;
            switch (controller.CurrentSolidManipulationMode)
            {
                case SolidManipulationMode.Solid:
                    switch (Viewport.RenderMode)
                    {
                        case BaseViewport.RenderModes.WIREFRAME:
                            Graphics.DrawWireframeSolid(solid, solidColor);
                            if (!solid.Selected)
                                DrawCornerPoints(Graphics, solid, Color.White);
                            break;
                        case BaseViewport.RenderModes.SOLID:
                            DrawSolid(Graphics, solid, solidColor);
                            break;
                        case BaseViewport.RenderModes.TEXTURED:
                            DrawSolid(Graphics, solid, solid.Selected ? Color.Red : Color.White, true);
                            break;
                    }
                    break;
                case SolidManipulationMode.Face:

                        switch (Viewport.RenderMode)
                        {
                            case BaseViewport.RenderModes.WIREFRAME:
                                Graphics.DrawWireframeSolid(solid, solidColor);
                                //DrawCornerPoints(Renderer, solid, Color.White);
                                break;
                            case BaseViewport.RenderModes.SOLID:
                            solid.Faces.ForEach((face) =>
                            {
                                SolidVertex[] positions = solid.GetVerticesForFace(face);
                                Graphics.DrawSolidPolygon(positions, face.Normal, 
                                    face.Selected ? Color.Red : solid.Color);
                            });
                            break;

                        case BaseViewport.RenderModes.TEXTURED:
                            solid.Faces.ForEach((face) =>
                            {
                                SolidVertex[] positions = solid.GetVerticesForFace(face);
                                Graphics.DrawTexturedSolidPolygon(positions, face.Normal,
                                    face.Selected ? Color.Red : Color.White, face.Texture);
                            });
                            break;
                    }
                    break;
                case SolidManipulationMode.Vertex:
                    switch (Viewport.RenderMode)
                    {
                        case BaseViewport.RenderModes.WIREFRAME:
                            Graphics.DrawWireframeSolid(solid, solidColor);
                            break;
                        case BaseViewport.RenderModes.SOLID:
                            DrawSolid(Graphics, solid, solidColor);
                            break;
                        case BaseViewport.RenderModes.TEXTURED:
                            DrawSolid(Graphics, solid, solid.Selected ? Color.Red : Color.White, true);
                            break;
                    }
                    break;
            }

            //Color centerColor = controller.CurrentSolidManipulationMode == SolidManipulationMode.Face
            //    ? Viewport.RenderMode == BaseViewport.RenderModes.SOLID && solid.Selected ? Color.Red: solid.Color
            //    : solid.Selected ? Color.Red : solid.Color;
            DrawCenter(Graphics, solid, solidColor);
        }

        private void RenderPerspSolid(Solid solid)
        {
            Color color = solid.Selected ? Color.Red : solid.Color;
            IEditorController controller = Viewport.Controller;
            switch (controller.CurrentSolidManipulationMode)
            {
                case SolidManipulationMode.Solid:
                    switch (Viewport.RenderMode)
                    {
                        case BaseViewport.RenderModes.WIREFRAME:
                            Graphics.DrawWireframeSolid(solid, color);
                            break;
                        case BaseViewport.RenderModes.SOLID:
                            DrawSolid(Graphics, solid, color);
                            if (solid.Selected)
                                Graphics.DrawWireframeSolid(solid, Color.Yellow);
                            break;

                        case BaseViewport.RenderModes.TEXTURED:
                            DrawSolid(Graphics, solid, solid.Selected ? Color.Red : Color.White, true);
                            if (solid.Selected)
                                Graphics.DrawWireframeSolid(solid, Color.Yellow);
                            break;
                    }
                    break;
                case SolidManipulationMode.Face:
                    solid.Faces.ForEach((face) =>
                    {
                        Color faceColor = face.Selected ? Color.Red : solid.Color;
                        SolidVertex[] positions = solid.GetVerticesForFace(face);
                        switch (Viewport.RenderMode)
                        {
                            case BaseViewport.RenderModes.WIREFRAME:
                                Graphics.DrawWireframeSolidPolygon(positions, face.Normal, faceColor);
                                break;
                            case BaseViewport.RenderModes.SOLID:
                                Graphics.DrawSolidPolygon(positions, face.Normal, faceColor);
                                break;
                            case BaseViewport.RenderModes.TEXTURED:
                                Graphics.DrawTexturedSolidPolygon(positions, face.Normal, face.Selected ? Color.Red : Color.White, face.Texture);
                                break;
                        }
                    });
                    break;
                case SolidManipulationMode.Vertex:
                    switch (Viewport.RenderMode)
                    {
                        case BaseViewport.RenderModes.WIREFRAME:
                            Graphics.DrawWireframeSolid(solid, Color.White);
                            break;
                        case BaseViewport.RenderModes.SOLID:
                            DrawSolid(Graphics, solid, color);
                            break;
                        case BaseViewport.RenderModes.TEXTURED:
                            DrawSolid(Graphics, solid, solid.Selected ? Color.Red : Color.White, true);
                            break;
                    }
                    break;
            }
        }

        public void Visit(RubberBand mapObject)
        {
        }

        public void Visit(MapObjectGroup mapObjectGroup)
        {
            foreach (MapObject mapObject in mapObjectGroup.MapObjectList)
            {
                mapObject.PerformOperation(this);
            }
        }

        /// <summary>
        /// Draw corner point indicators
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="solid"></param>
        /// <param name="color"></param>
        private void DrawCornerPoints(Graphics.Graphics graphics, Solid solid, Color color)
        {
            // create this viewport base vectors
            float halfIndicitorSize = 1.8f * Viewport.Zoom;
            Matrix4 viewportMatrix = Viewport.Camera.GetWorldMatrix().ClearTranslation();
            Vector3 horizontalVector = viewportMatrix.Row0.Xyz * halfIndicitorSize;
            Vector3 verticalVector = viewportMatrix.Row1.Xyz * halfIndicitorSize;

            foreach (Vector3 position in solid.VertexPositions)
            {
                // create 4 points for the rectangle
                Vector3 bottomLeft = position - horizontalVector - verticalVector;
                Vector3 bottomRight = position + horizontalVector - verticalVector;
                Vector3 topRight = position + horizontalVector + verticalVector;
                Vector3 topLeft = position - horizontalVector + verticalVector;

                graphics.DrawSolidRectangle(bottomLeft, bottomRight, topRight, topLeft, color);
            }
        }

        /// <summary>
        /// Draw center
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="solid"></param>
        /// <param name="color"></param>
        private void DrawCenter(Graphics.Graphics graphics, MapObject solid, Color color)
        {
            // create this viewport base vectors
            float halfIndicitorSize = 5f * Viewport.Zoom;
            Matrix4 viewportMatrix = Viewport.Camera.GetWorldMatrix().ClearTranslation();
            Vector3 horizontalVector = viewportMatrix.Row0.Xyz * halfIndicitorSize;
            Vector3 verticalVector = viewportMatrix.Row1.Xyz * halfIndicitorSize;

            // draw center x
            Vector3 center = solid.Bounds.Center;
            Vector3 diagonalOne = (-horizontalVector + verticalVector).Normalized() * halfIndicitorSize;
            Vector3 diagonalTwo = (-horizontalVector - verticalVector).Normalized() * halfIndicitorSize;

            graphics.DrawLine(center - diagonalOne, center + diagonalOne, RuneGear.Graphics.Graphics.LineType.LineNormal, color);
            graphics.DrawLine(center - diagonalTwo, center + diagonalTwo, RuneGear.Graphics.Graphics.LineType.LineNormal, color);
        }

        public void DrawSolid(Graphics.Graphics graphics, Solid solid, Color color, bool textured = false)
        {
            // extract triangles from faces
            // trianglefan to trianglelist
            foreach (SolidFace face in solid.Faces)
            {
                if (face.Indices.Count < 3)
                    continue;

                SolidVertex[] vertices = solid.GetVerticesForFace(face);
                if (textured)
                    graphics.DrawTexturedSolidPolygon(vertices, face.Normal, color, face.Texture);
                else
                    graphics.DrawSolidPolygon(vertices, face.Normal, color);
            }
        }
    }
}
