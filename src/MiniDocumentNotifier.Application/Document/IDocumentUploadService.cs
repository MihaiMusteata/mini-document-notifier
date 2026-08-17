using MiniDocumentNotifier.Contracts.DocumentUploadContracts;

namespace MiniDocumentNotifier.Application.Document
{
    public interface IDocumentUploadService
    {
        int Upload(DocumentUploadRequest documentUploadModel);
    }
}