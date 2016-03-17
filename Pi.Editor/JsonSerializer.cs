using System.IO;
using Newtonsoft.Json;

namespace Pi.Editor
{
    public interface IFileReader
    {
        string ReadText(string path);
        byte[] ReadBytes(string path);
    }

    public class DefaultFileReader : IFileReader
    {
        public string ReadText(string path)
        {
            return File.ReadAllText(path);
        }

        public byte[] ReadBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }

    public static class JsonSerializer
    {
        private static IFileReader _fileReader;
        public static IFileReader FileReader
        {
            get { return _fileReader ?? new DefaultFileReader(); }
            set { _fileReader = value; }
        }

        public static ComponentDef DeserializeComponent(string path)
        {
            return JsonConvert.DeserializeObject<ComponentDef>(FileReader.ReadText(path));
        }

        public static PropertyDef DeserializeProperty(string path)
        {
            return JsonConvert.DeserializeObject<PropertyDef>(FileReader.ReadText(path));
        }

        public static EntityDef DeserializeEntity(string path)
        {
            return JsonConvert.DeserializeObject<EntityDef>(FileReader.ReadText(path));
        }
    }
}
