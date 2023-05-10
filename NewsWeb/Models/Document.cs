using System;
using System.Collections.Generic;

#nullable disable

namespace NewsWeb.Models
{
    public partial class Document
    {
        public int DocId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Scontents { get; set; }
        public string Thumb { get; set; }
        public string Alias { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool Published { get; set; }
        public string Author { get; set; }
        public int? AccountId { get; set; }
        public int? FileId { get; set; }
        public bool? Pin { get; set; }
        public string Tags { get; set; }

        public virtual Account Account { get; set; }
        public virtual File File { get; set; }
    }
}
