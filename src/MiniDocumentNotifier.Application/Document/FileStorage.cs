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

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public IFileMetadata GetInfo(string path)
        {
            return new FileMetadata(new FileInfo(path));
        }
    }

    public class FileMetadata : IFileMetadata
    {
        private readonly FileInfo _fileInfo;

        public FileMetadata(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public long Length => _fileInfo.Length;
    }
}