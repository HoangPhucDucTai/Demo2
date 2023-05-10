using NewsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.ModelViews
{
    public class HomeViewModel
    {
        public List<Document> Populars { get; set; }
        public List<Document> Inspriration { get; set; }
        public List<Document> Recents { get; set; }
        public List<Document> Tredings { get; set; }
        public List<Document> LatestDocs { get; set; }
        public Document Featured { get; set; }
    }
}
