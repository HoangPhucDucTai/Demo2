using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NewsWeb.Enums;
using NewsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Controllers.Components
{
    public class PopularViewComponent: ViewComponent
    {
        private readonly NewsWebContext _context;
        private IMemoryCache _memoryCache;
        public PopularViewComponent(NewsWebContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public IViewComponentResult Invoke()
        {
            var _tailieu = _memoryCache.GetOrCreate(CacheKeys.Popular, entry =>
            {
                entry.SlidingExpiration = TimeSpan.MaxValue;
                return GetlsDocuments();
            });
            return View(_tailieu);
        }
        public List<Document> GetlsDocuments()
        {
            List<Document> lstins = new List<Document>();
            lstins = _context.Documents
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderByDescending(x => x.DateCreate)
                .Take(6)
                .ToList();
            return lstins;
        }
    }
}
