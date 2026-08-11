using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MiniDocumentNotifier.Domain.Enums;

namespace MiniDocumentNotifier.Contracts.DocumentContracts
{
    [DataContract]
    public class DocumentDto
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public DocumentType Type { get; set; }
        [DataMember] public DocumentStatus Status { get; set; }
        [DataMember] public DateTime UploadDate { get; set; }
        [DataMember] public int InstitutionId { get; set; }
    }

    public static class DocumentLabels
    {
        public static Dictionary<DocumentType, string> DocumentTypeLabels =
            new Dictionary<DocumentType, string>
            {
                { DocumentType.Notification, "Statement" },
                { DocumentType.Statement, "Statement" },
                { DocumentType.Contract, "Contract" },
                { DocumentType.Circular, "Circular" }
            };

        public static Dictionary<DocumentStatus, string> DocumentStatusLabels =
            new Dictionary<DocumentStatus, string>
            {
                { DocumentStatus.New, "New" },
                { DocumentStatus.Read, "Read" },
                { DocumentStatus.Archived, "Archived" }
            };
    }
}