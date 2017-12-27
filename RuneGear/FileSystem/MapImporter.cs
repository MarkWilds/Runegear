using System.IO;
using RuneGear.General;

namespace RuneGear.FileSystem
{
    public interface MapImporter
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

        void Import(FileStream stream, SceneDocument document);
    }
}
