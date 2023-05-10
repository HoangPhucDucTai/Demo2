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
    public class FilesViewComponent : ViewComponent
    {
        private readonly NewsWebContext _context;
        private IMemoryCache _memoryCache;
        public FilesViewComponent(NewsWebContext context, IMemoryCache memoryCache) 
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public IViewComponentResult Invoke()
        {
            var _lsThuMuc = _memoryCache.GetOrCreate(CacheKeys.Files, entry =>
            {
                entry.SlidingExpiration = TimeSpan.MaxValue;
                return GetlsFiles();
            });
            return View(_lsThuMuc);
        }
        public List<File> GetlsFiles()
        {
            List<File> lstins = new List<File>();
            lstins = _context.Files
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderBy(x => x.Ordering)
                .ToList();
            return lstins;
        }
    }
}
