using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsWeb.Helpers;
using NewsWeb.Models;
using PagedList.Core;
using File = NewsWeb.Models.File;

namespace NewsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FilesController : Controller
    {
        private readonly NewsWebContext _context;

        public FilesController(NewsWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Files
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Utilities.PAGE_SIZE;
            var pageSize = 2;
            var lsFiles = _context.Files
                .AsNoTracking()
                .OrderByDescending(x => x.FileId);
            PagedList<File> models = new PagedList<File>(lsFiles, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // GET: Admin/Files/Create
        public IActionResult Create()
        {
            ViewData["ThuMucGoc"] = new SelectList(_context.Files.Where(x => x.Levels == 1), "FileId", "FileName");
            return View();
        }

        // POST: Admin/Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileId,FileName,Title,Alias,MetaDe,MetaKey,Thumb,Published,Parents,Description,Levels,Ordering")] File file, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                file.Alias = Utilities.SEOurl(file.FileName);
                if (file.Parents == null)
                {
                    file.Levels = 1;
                }
                else
                {
                    file.Levels = file.Parents == 0 ? 1 : 2;
                }
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string Newname = Utilities.SEOurl(file.FileName) + "preview_" + extension;
                    file.Thumb = await Utilities.UploadFile(fThumb, @"file\", Newname.ToLower());
                }
                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(file);
        }

        // GET: Admin/Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }

        // POST: Admin/Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileId,FileName,Title,Alias,MetaDe,MetaKey,Thumb,Published,Parents,Description,Levels,Ordering")]File file, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != file.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    file.Alias = Utilities.SEOurl(file.FileName);
                    if (file.Parents == null)
                    {
                        file.Levels = 1;
                    }
                    else
                    {
                        file.Levels = file.Parents == 0 ? 1 : 2;
                    }
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string Newname = Utilities.SEOurl(file.FileName) + "preview_" + extension;
                        file.Thumb = await Utilities.UploadFile(fThumb, @"file\", Newname.ToLower());
                    }
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.FileId))
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
            return View(file);
        }

        // GET: Admin/Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // POST: Admin/Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.Files.FindAsync(id);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.Files.Any(e => e.FileId == id);
        }
    }
}
