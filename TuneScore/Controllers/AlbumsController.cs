using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuneScore.Data;
using TuneScore.Helpers;
using TuneScore.Models;

namespace TuneScore.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly TuneScoreContext _context;
        private HelperPathProvider helperPath;

        public AlbumsController(TuneScoreContext context, HelperPathProvider helperPath)
        {
            _context = context;
            this.helperPath = helperPath;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var tuneScoreContext = _context.Albums.Include(a => a.Artist);
            return View(await tuneScoreContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseYear,ArtistId,CreatedAt")] Album album, IFormFile fichero)
        {
            if (fichero == null || !fichero.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("", "Debe subir una imagen válida.");
                return View(album);
            }
            if (ModelState.IsValid)
            {
                if (fichero != null && fichero.Length > 0)
                {
                    // Generar nombre único
                    string extension = Path.GetExtension(fichero.FileName);
                    string fileName = Guid.NewGuid().ToString() + extension;

                    string path = this.helperPath.MapPath(fileName, Folders.Albums);

                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await fichero.CopyToAsync(stream);
                    }

                    // GUARDAMOS el nombre en la BD
                    album.ImageName = fileName;
                }

                album.CreatedAt = DateTime.Now;

                _context.Add(album);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", album.ArtistId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", album.ArtistId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Album album, IFormFile fichero)
        {
            if (id != album.Id) return NotFound();

            var albumDb = await _context.Albums.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (albumDb == null) return NotFound();

            // Manually update fields to avoid model binding issues
            albumDb.Title = album.Title;
            albumDb.ReleaseYear = album.ReleaseYear;
            albumDb.ArtistId = album.ArtistId;

            // File upload
            if (fichero != null && fichero.Length > 0)
            {
                if (!fichero.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("", "Solo se permiten imágenes.");
                    ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", albumDb.ArtistId);
                    return View(albumDb);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(albumDb.ImageName))
                {
                    string oldPath = helperPath.MapPath(albumDb.ImageName, Folders.Albums);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string extension = Path.GetExtension(fichero.FileName);
                // Remove invalid filename chars and spaces
                string safeTitle = string.Concat(album.Title.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
                safeTitle = safeTitle.Replace(" ", "");

                // Add "Icon" and the original extension
                string fileName = $"{safeTitle}Icon{extension}";

                string path = helperPath.MapPath(fileName, Folders.Albums);

                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await fichero.CopyToAsync(stream);
                }

                albumDb.ImageName = fileName;
            }

            _context.Update(albumDb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
