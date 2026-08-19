using System.IO;

namespace MiniDocumentNotifier.Application.Document
{
    public class FileStorage : IFileStorage
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void WriteAllBytes(string path, byte[] content)
        {
            File.WriteAllBytes(path, content);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}