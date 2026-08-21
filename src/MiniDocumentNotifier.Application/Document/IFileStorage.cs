using System;

namespace MiniDocumentNotifier.Application.Document
{
    public interface IFileStorage
    {
        void CreateDirectory(string path);
        void WriteAllBytes(string path, byte[] content);
        void WriteAllText(string path, string content);
        byte[] ReadAllBytes(string path);
        IFileMetadata GetInfo(string path);
        bool Exists(string path);
        bool DirectoryExists(string path);
        DateTime GetLastWriteTimeUtc(string path);
        string ReadAllText(string path);
        string GetDirectoryName(string path);
    }

    public interface IFileMetadata
    {
        long Length { get; }
    }
}