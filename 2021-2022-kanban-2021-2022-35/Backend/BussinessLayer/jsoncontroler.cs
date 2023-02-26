using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class jsoncontroler
    {

        protected const string _basePath = "persistance";
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public string SanitizeFilename(string filename)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitizedFileName = filename;
            foreach (char c in invalidChars)
            {
                sanitizedFileName = sanitizedFileName.Replace(c.ToString(), "");
            }
            return sanitizedFileName;
        }

        public System.Collections.Generic.List<T> LoadAll<T>(System.Type type)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), _basePath, type.Name);
            string[] filenames = Directory.GetFiles(path);
            return filenames
                .Where(n => n.EndsWith(".json"))
                .Select(n => Path.GetFileName(n))
                .Select(n => Load<T>(type, n))
                .ToList();
        }

        public T Load<T>(System.Type type, string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), _basePath, type.Name, SanitizeFilename(filename));
            string content = File.ReadAllText(path);
            return FromJson<T>(content);
        }
        public void Write(string filename, object obj)
        {
            string subDirectory = obj.GetType().Name;
            string content = ToJson(obj);
            string sanitizedFileName = SanitizeFilename(filename) + ".json";
            string path = Path.Combine(Directory.GetCurrentDirectory(), _basePath, subDirectory);
            string fullPath = Path.Combine(path, sanitizedFileName);
            Directory.CreateDirectory(path);
            File.WriteAllText(fullPath, content);
        }

        public static T FromJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string ToJson(object obj)
        {
            return JsonSerializer.Serialize(obj, obj.GetType(), options);
        }

    }
}
    

