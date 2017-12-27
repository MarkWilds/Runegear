using System.IO;
using RuneGear.General;

namespace RuneGear.FileSystem
{
    public interface MapExporter
    {
        string FileExtensionName
        {
            get;
        }

        string FileDescription
        {
            get;
        }

        string FileTypeName
        {
            get;
        }

        void Export(FileStream stream, SceneDocument document);
    }
}
