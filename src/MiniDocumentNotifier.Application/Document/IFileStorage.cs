using System.IO;

namespace MiniDocumentNotifier.Application.Document
{
    public interface IFileStorage
    {
        void CreateDirectory(string path);
        void WriteAllBytes(string path, byte[] content);
        void WriteAllText(string path, string content);
        byte[] ReadAllBytes(string path);
        IFileMetadata GetInfo(string path);
    }

    public interface IFileMetadata
    {
        long Length { get; }
    }
}