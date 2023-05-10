using System;
using System.Collections.Generic;

#nullable disable

namespace NewsWeb.Models
{
    public partial class File
    {
        public File()
        {
            Documents = new HashSet<Document>();
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string MetaDe { get; set; }
        public string MetaKey { get; set; }
        public string Thumb { get; set; }
        public bool Published { get; set; }
        public int? Parents { get; set; }
        public string Description { get; set; }
        public int? Levels { get; set; }
        public int? Ordering { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
