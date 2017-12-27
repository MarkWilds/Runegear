using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;
using RuneGear.General;
using RuneGear.Graphics.Textures;
using RuneGear.MapObjects;
using RuneGear.MapObjects.SolidMapObject;

namespace RuneGear.FileSystem
{
    public class RuneMapImporter : MapImporter
    {
        public string FileDescription => "";
        public string FileExtensionName => ".rmap";
        public string FileTypeName => "runemap";

        public void Import(FileStream stream, SceneDocument document)
        {
            StreamReader streamReader = new StreamReader(stream);
            JsonTextReader reader = new JsonTextReader(streamReader);

            JObject mapFile = (JObject) JToken.ReadFrom(reader);
            JToken mapobjects = mapFile["mapobjects"];

            MapObjectGroup group = CreateGroup(mapobjects);
            group.MapObjectList.ForEach(document.AddMapObject);

            document.MapName = mapFile["name"].ToString();
        }

        private MapObjectGroup CreateGroup(JToken groupToken)
        {
            MapObjectGroup group = new MapObjectGroup();
            foreach (JObject mapObject in groupToken.Children<JObject>())
            {
                foreach (JProperty property in mapObject.Properties())
                {
                    switch (property.Name)
                    {
                        case "solid":
                            Solid solid = CreateSolid(property.Value);
                            if (solid != null)
                                group.Add(solid);
                            break;
                        case "group":
                            MapObjectGroup newGroup = CreateGroup(property.Value);
                            if (newGroup != null)
                                group.Add(newGroup);
                            break;
                    }
                }
            }

            return group;
        }

        private Solid CreateSolid(JToken token)
        {
            Solid solid = new Solid();
            foreach (JToken child in token.Children())
            {
                JProperty property = child as JProperty;
                if (property == null)
                    continue;

                switch (property.Name)
                {
                    case "color":
                        solid.Color = ExtractColor(property);
                        break;
                    case "hidden":
                        solid.Hidden = property.Value.Value<bool>();
                        break;
                    case "detail":
                        solid.Detail = property.Value.Value<bool>();
                        break;
                    case "positions":
                        List<Vector3> positions = ExtractPositions(property);
                        solid.VertexPositions.AddRange(positions);
                        break;
                    case "faces":
                        List<SolidFace> faces = ExtractFaces(property);
                        if (faces == null)
                            throw new Exception("Faces zijn niet correct toegevoegd");
                        solid.Faces.AddRange(faces);
                        break;
                }
            }

            solid.CalculateNormals();
            solid.RegenerateBounds();

            return solid;
        }

        private Color ExtractColor(JProperty property)
        {
            if (property.Type != JTokenType.Property || !property.HasValues)
                return Color.White;

            string[] values = property.Value.ToString().Split(';');

            if (values.Length < 3)
                return Color.White;

            int r = int.Parse(values[0]);
            int g = int.Parse(values[1]);
            int b = int.Parse(values[2]);
            return Color.FromArgb(255, r, g, b);
        }

        private List<Vector3> ExtractPositions(JProperty property)
        {
            if (property.Type != JTokenType.Property || !property.HasValues)
                return null;

            JArray positionsArray = property.Value as JArray;
            if (positionsArray == null)
                return null;

            List<Vector3> positions = new List<Vector3>();
            foreach (JToken data in positionsArray)
            {
                string[] values = data.Value<string>().Split(';');
                if (values.Length < 3)
                    continue;

                float x = float.Parse(values[0].Replace('.', ','));
                float y = float.Parse(values[1].Replace('.', ','));
                float z = float.Parse(values[2].Replace('.', ','));

                positions.Add(new Vector3(x, y, z));
            }

            if (positions.Count <= 0)
                throw new RunegearFileformatErrorException("Some solid vertices could not be added!");

            return positions;
        }

        private List<SolidFace> ExtractFaces(JProperty property)
        {
            if (property.Type != JTokenType.Property || !property.HasValues)
                return null;

            JArray faceArray = property.Value as JArray;
            if (faceArray == null)
                return null;

            List<SolidFace> faces = new List<SolidFace>();
            foreach (JObject faceObject in faceArray.Children<JObject>())
            {
                SolidFace face = new SolidFace();
                foreach (JProperty data in faceObject.Children<JProperty>())
                {
                    switch (data.Name)
                    {
                        case "texture":
                            Texture2D texture = new Texture2D(data.Value.ToString(), false);
                            face.Texture = texture;
                            break;
                        case "texturemapping":
                            face.TextureMapping = ExtractTexturemapping(data.Value<JProperty>());
                            break;
                        case "indices":
                            List<SolidIndex> indices = ExtractIndices(data.Value.Value<JArray>());
                            face.Indices.AddRange(indices);
                            break;
                    }
                }

                faces.Add(face);
            }

            if (faces.Count <= 0)
                throw new RunegearFileformatErrorException("Some solid faces could not be added!");

            return faces;
        }

        private TextureMapping ExtractTexturemapping(JProperty property)
        {
            TextureMapping textureMapping = new TextureMapping();

            foreach (JProperty mappingProperty in property.Value.Children<JProperty>())
            {
                switch (mappingProperty.Name)
                {
                    case "uscale":
                        textureMapping.UScale = mappingProperty.Value.Value<float>();
                        break;
                    case "vscale":
                        textureMapping.VScale = mappingProperty.Value.Value<float>();
                        break;
                    case "ushift":
                        textureMapping.UShift = mappingProperty.Value.Value<float>();
                        break;
                    case "vshift":
                        textureMapping.VShift = mappingProperty.Value.Value<float>();
                        break;
                    case "rotation":
                        textureMapping.Rotation = mappingProperty.Value.Value<float>();
                        break;
                    case "texturelocked":
                        textureMapping.TextureLocked = mappingProperty.Value.Value<bool>();
                        break;
                }
            }

            return textureMapping;
        }

        private List<SolidIndex> ExtractIndices(JArray array)
        {
            List<SolidIndex> solidIndices = new List<SolidIndex>();
            foreach (JObject jObject in array.Children<JObject>())
            {
                SolidIndex index = new SolidIndex();
                foreach (JProperty property in jObject.Children<JProperty>())
                {
                    switch (property.Name)
                    {
                        case "index":
                            index.Index = property.Value.Value<int>();
                            break;
                        case "uv":
                            index.DiffuseUv = ExtractUV(property);
                            break;
                    }
                }

                solidIndices.Add(index);
            }

            if (solidIndices.Count <= 0)
                throw new RunegearFileformatErrorException("Some solid indices could not be added to a solid face!");

            return solidIndices;
        }

        private Vector2 ExtractUV(JProperty property)
        {
            string[] values = property.Value.ToString().Split(';');

            if (values.Length < 2)
                return new Vector2();

            float u = float.Parse(values[0].Replace('.', ','));
            float v = float.Parse(values[1].Replace('.', ','));
            return new Vector2(u,v);
        }
    }
}