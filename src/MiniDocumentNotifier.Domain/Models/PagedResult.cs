using System.Collections.Generic;

namespace MiniDocumentNotifier.Domain.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
    }
}