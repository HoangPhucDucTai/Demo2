using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsWeb.Helpers;
using NewsWeb.Models;
using PagedList.Core;

//layout chính
namespace NewsWeb.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly NewsWebContext _context;

        public DocumentsController(NewsWebContext context)
        {
            _context = context;
        }
        //get list
        [Route("{Alias}", Name ="ListDoc")]
        public IActionResult List(string Alias, int? page)
        {
            if (string.IsNullOrEmpty(Alias)) return RedirectToAction("Home", "Index");
            var thumuc = _context.Files.FirstOrDefault(x => x.Alias == Alias);
            if (thumuc == null) return RedirectToAction("Home", "Index");


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = 2;
            var pageSize = Utilities.PAGE_SIZE;
            List<Document> lsDocs = new List<Document>();

            if (!string.IsNullOrEmpty(Alias))
            {
                lsDocs = _context.Documents.
                    Include(x => x.File)
                    .Where(x => x.FileId == thumuc.FileId)
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreate)
                    .ToList();
            }
            else
            {
                lsDocs = _context.Documents.
                    Include(x => x.File)
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreate)
                    .ToList();
            } 
            PagedList<Document> models = new PagedList<Document>(lsDocs.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.ThuMuc = thumuc;

            return View(models);
        }
        // GET: Documents/Details/5
        [Route("/{Alias}.html", Name = "DocDetails")]
        public async Task<IActionResult> Details(string Alias)
        {
            if (string.IsNullOrEmpty(Alias))
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Account)
                .Include(d => d.File)
                .FirstOrDefaultAsync(m => m.Alias == Alias);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocId == id);
        }
    }
}
