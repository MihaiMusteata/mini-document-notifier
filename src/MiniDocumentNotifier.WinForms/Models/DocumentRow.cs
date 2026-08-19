using System;

namespace MiniDocumentNotifier.WinForms.Models
{
    public class DocumentRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime UploadDate { get; set; }
    }
}