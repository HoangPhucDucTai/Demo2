    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsWeb.Helpers;
using NewsWeb.Models;
using PagedList.Core;

namespace NewsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DocumentsController : Controller
    {
        private readonly NewsWebContext _context;

        public DocumentsController(NewsWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Documents
        public IActionResult Index(int? page)
        {
            //kt quyen truycap
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountId == int.Parse(taikhoanID));
            if (account == null) return NotFound();

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 2;

            var lsDocuments = _context.Documents
                .Include(p => p.Account).Include(p => p.File)
                .OrderByDescending(x => x.FileId);
            PagedList<Document> models = new PagedList<Document>(lsDocuments, pageNumber, pageSize);
            //ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Account)
                .Include(d => d.File)
                .FirstOrDefaultAsync(m => m.DocId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Admin/Documents/Create
        public IActionResult Create()
        {
            //kt quyen truycap
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["FileId"] = new SelectList(_context.Files, "FileId", "FileName");
            return View();
        }

        // POST: Admin/Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocId,Title,Contents,Scontents,Thumb,Alias,DateCreate,Published,Author,AccountId,FileId,Pin,Tags")] Document document, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            //kt quyen truycap
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountId == int.Parse(taikhoanID));
            if (account == null) return NotFound();

            if (ModelState.IsValid)
            {
                document.AccountId = account.AccountId;
                document.Author = account.FullName;
                if (document.FileId == null) document.FileId = 1;
                document.DateCreate = DateTime.Now;
                document.Alias = Utilities.SEOurl(document.Title);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string Newname = Utilities.SEOurl(document.Title) + extension;
                    document.Thumb = await Utilities.UploadFile(fThumb, @"post\", Newname.ToLower());
                }

                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", document.AccountId);
            ViewData["FileId"] = new SelectList(_context.Files, "FileId", "FileName", document.FileId);
            return View(document);
        }

        // GET: Admin/Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", document.AccountId);
            ViewData["FileId"] = new SelectList(_context.Files, "FileId", "FileName", document.FileId);

            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });

            return View(document);
        }

        // POST: Admin/Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocId,Title,Contents,Scontents,Thumb,Alias,DateCreate,Published,Author,AccountId,FileId,Pin,Tags")] Document document, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != document.DocId)
            {
                return NotFound();
            }
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null) return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            var account = _context.Accounts.AsNoTracking().FirstOrDefault(x => x.AccountId == int.Parse(taikhoanID));
            if (account == null) return NotFound();

            //if (account.RoleId != 1)
            //{
            //    if (document.AccountId != account.AccountId) return RedirectToAction(nameof(Index));
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string Newname = Utilities.SEOurl(document.Title) + extension;
                        document.Thumb = await Utilities.UploadFile(fThumb, @"posts\", Newname.ToLower());
                    }
                    document.Alias = Utilities.SEOurl(document.Title);
                    document.AccountId = account.AccountId;
                    document.Author = account.FullName;

                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", document.AccountId);
            ViewData["FileId"] = new SelectList(_context.Files, "FileId", "FileName", document.FileId);
            return View(document);
        }

        // GET: Admin/Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Account)
                .Include(d => d.File)
                .FirstOrDefaultAsync(m => m.DocId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Admin/Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocId == id);
        }
    }
}
