using Newtonsoft.Json;

namespace Pi.Editor
{
    public interface IFileReader
    {
        string ReadText(string path);
    }

    public class DefaultFileReader : IFileReader
    {
        public string ReadText(string path)
        {
            return FileSys.ReadFile(path);
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

        public static T Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(FileReader.ReadText(path));
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
