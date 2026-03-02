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
using TuneScore.Repositories.Interfaces;
using TuneScore.Services;
using TuneScore.Services.Interfaces;

namespace TuneScore.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly TuneScoreContext _context;
        private readonly IRepositoryAlbums _albumsRepository;
        private readonly IAlbumImageService _albumImageService;
        private readonly UserService _userService;
        private HelperPathProvider helperPath;

        public AlbumsController(
            TuneScoreContext context,
            IRepositoryAlbums albumsRepository,
            IAlbumImageService albumImageService,
            UserService userService,
            HelperPathProvider helperPath)
        {
            _context = context;
            _albumsRepository = albumsRepository;
            _albumImageService = albumImageService;
            _userService = userService;
            this.helperPath = helperPath;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var albums = await _albumsRepository.GetAllAlbumsAsync();
            return View(albums);
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumsRepository.GetAlbumByIdAsync(id.Value);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public async Task<IActionResult> Create()
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para crear un álbum.";
                return RedirectToAction(nameof(Index));
            }
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
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para crear un álbum.";
                return RedirectToAction(nameof(Index));
            }
            if (!AlbumHelper.ValidateCreateImage(ModelState, fichero))
            {
                return View(album);
            }
            if (ModelState.IsValid)
            {
                if (fichero != null && fichero.Length > 0)
                {
                    album.ImageName = await _albumImageService.SaveNewImageAsync(fichero);
                }

                album.CreatedAt = DateTime.Now;

                await _albumsRepository.AddAlbumAsync(album);

                return RedirectToAction(nameof(Index));
            }

            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", album.ArtistId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para editar un álbum.";
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumsRepository.GetAlbumByIdAsync(id.Value);
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
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para editar un álbum.";
                return RedirectToAction(nameof(Index));
            }
            if (id != album.Id) return NotFound();

            var albumDb = await _albumsRepository.GetAlbumByIdAsync(id);
            if (albumDb == null) return NotFound();

            // Manually update fields to avoid model binding issues
            albumDb.Title = album.Title;
            albumDb.ReleaseYear = album.ReleaseYear;
            albumDb.ArtistId = album.ArtistId;

            // File upload
            if (fichero != null && fichero.Length > 0)
            {
                if (!AlbumHelper.ValidateEditImage(ModelState, fichero))
                {
                    ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", albumDb.ArtistId);
                    return View(albumDb);
                }

                albumDb.ImageName = await _albumImageService.ReplaceImageAsync(fichero, albumDb.ImageName, album.Title);
            }

            await _albumsRepository.UpdateAlbumAsync(albumDb);

            return RedirectToAction(nameof(Index));
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para eliminar un álbum.";
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumsRepository.GetAlbumByIdAsync(id.Value);
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
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para eliminar un álbum.";
                return RedirectToAction(nameof(Index));
            }
            await _albumsRepository.DeleteAlbumAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
