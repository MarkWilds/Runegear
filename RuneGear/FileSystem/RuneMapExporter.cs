using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using RuneGear.General;
using RuneGear.MapObjects;

namespace RuneGear.FileSystem
{
    public class RuneMapExporter : MapExporter
    {
        public string FileDescription => "";
        public string FileExtensionName => ".rmap";
        public string FileTypeName => "runemap";

        public void Export(FileStream stream, SceneDocument document)
        {
            StreamWriter streamWriter = new StreamWriter(stream);
            JsonWriter writer = new JsonTextWriter(streamWriter);
            writer.Formatting = Formatting.Indented;

            CustomOperation mapObjectJsonWriter = GetJsonOperation(writer);

            // Write map to Json file
            writer.WriteStartObject();

            // write map header
            writer.WritePropertyName("name");
            writer.WriteValue(document.MapName);
            writer.WritePropertyName("mapversion");
            writer.WriteValue(1);

            // write map objects
            writer.WritePropertyName("mapobjects");
            writer.WriteStartArray();
            foreach (MapObject mapObject in document)
            {
                mapObject.PerformOperation(mapObjectJsonWriter);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            writer.Close();
        }

        private CustomOperation GetJsonOperation(JsonWriter writer)
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo {NumberDecimalSeparator = "."};

            CustomOperation mapObjectJsonWriter = new CustomOperation();
            mapObjectJsonWriter.OnMapObjectGroup = (mapObjectGroup) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("group");
                writer.WriteStartArray();
                mapObjectGroup.MapObjectList.ForEach(
                    (mapObject) => { mapObject.PerformOperation(mapObjectJsonWriter); });
                writer.WriteEndArray();
                writer.WriteEndObject();
            };
            mapObjectJsonWriter.OnSolid = (solid) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("solid");
                writer.WriteStartObject();

                // color
                writer.WritePropertyName("color");
                writer.Formatting = Formatting.None;
                writer.WriteValue($"{solid.Color.R};{solid.Color.G};{solid.Color.B}");
                writer.Formatting = Formatting.Indented;

                // solid properties
                writer.WritePropertyName("detail");
                writer.WriteValue(solid.Detail);
                writer.WritePropertyName("hidden");
                writer.WriteValue(solid.Hidden);

                // vertex positions
                writer.WritePropertyName("positions");
                writer.WriteStartArray();
                solid.VertexPositions.ForEach(v =>
                {
                    writer.WriteValue(
                        $"{v.X.ToString(numberFormatInfo)};{v.Y.ToString(numberFormatInfo)};{v.Z.ToString(numberFormatInfo)}");
                });
                writer.WriteEndArray();

                // faces
                writer.WritePropertyName("faces");
                writer.WriteStartArray();
                solid.Faces.ForEach(f =>
                {
                    writer.WriteStartObject();
                    if (f.Texture != null)
                    {
                        writer.WritePropertyName("texture");
                        writer.WriteValue(f.Texture.Identifier);
                    }
                    writer.WritePropertyName("texturemapping");
                    writer.WriteStartObject();

                    writer.WritePropertyName("uscale");
                    writer.WriteValue(f.TextureMapping.UScale);
                    writer.WritePropertyName("vscale");
                    writer.WriteValue(f.TextureMapping.VScale);
                    writer.WritePropertyName("ushift");
                    writer.WriteValue(f.TextureMapping.UShift);
                    writer.WritePropertyName("vshift");
                    writer.WriteValue(f.TextureMapping.VScale);
                    writer.WritePropertyName("rotation");
                    writer.WriteValue(f.TextureMapping.Rotation);
                    writer.WritePropertyName("texturelocked");
                    writer.WriteValue(f.TextureMapping.TextureLocked);
                    writer.WriteEndObject();

                    writer.WritePropertyName("indices");
                    writer.WriteStartArray();
                    f.Indices.ForEach(i =>
                    {
                        writer.WriteStartObject();
                        writer.Formatting = Formatting.None;
                        writer.WritePropertyName("index");
                        writer.WriteValue(i.Index);
                        writer.WritePropertyName("uv");

                        string x = i.DiffuseUv.X.ToString(CultureInfo.InvariantCulture);
                        string y = i.DiffuseUv.Y.ToString(CultureInfo.InvariantCulture);
                        writer.WriteValue($"{x.ToString(numberFormatInfo)};{y.ToString(numberFormatInfo)}");

                        writer.WriteEndObject();
                        writer.Formatting = Formatting.Indented;
                    });
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                });
                writer.WriteEndArray();

                writer.WriteEndObject();
                writer.WriteEndObject();
            };

            return mapObjectJsonWriter;
        }
    }
}