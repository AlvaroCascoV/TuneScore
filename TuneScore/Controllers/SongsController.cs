using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuneScore.Data;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;
using TuneScore.Services;

namespace TuneScore.Controllers
{
    public class SongsController : Controller
    {
        private readonly TuneScoreContext _context;
        private readonly IRepositorySongs _songsRepository;
        private readonly UserService _userService;

        public SongsController(
            TuneScoreContext context,
            IRepositorySongs songsRepository,
            UserService userService)
        {
            _context = context;
            _songsRepository = songsRepository;
            _userService = userService;
        }

        // GET: Songs
        public async Task<IActionResult> Index()
        {
            var songs = await _songsRepository.GetAllSongsAsync();
            return View(songs);
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _songsRepository.GetSongByIdAsync(id.Value);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public async Task<IActionResult> Create(int? albumId)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para crear una canción.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");

            var model = new Song();
            if (albumId.HasValue)
            {
                model.AlbumId = albumId.Value;
            }
            return View(model);
        }

        // POST: Songs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,DurationSeconds,AlbumId,GenreId")] Song song)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para crear una canción.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                song.CreatedAt = DateTime.Now;
                await _songsRepository.AddSongAsync(song);
                return RedirectToAction(nameof(Index));
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title", song.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", song.GenreId);
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para editar una canción.";
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var song = await _songsRepository.GetSongByIdAsync(id.Value);
            if (song == null)
            {
                return NotFound();
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Title", song.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", song.GenreId);
            return View(song);
        }

        // POST: Songs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DurationSeconds,AlbumId,GenreId,CreatedAt")] Song song)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para editar una canción.";
                return RedirectToAction(nameof(Index));
            }
            if (id != song.Id)
            {
                return NotFound();
            }

            var songDb = await _songsRepository.GetSongByIdAsync(id);
            if (songDb == null)
            {
                return NotFound();
            }

            songDb.Title = song.Title;
            songDb.DurationSeconds = song.DurationSeconds;
            songDb.AlbumId = song.AlbumId;
            songDb.GenreId = song.GenreId;

            await _songsRepository.UpdateSongAsync(songDb);
            return RedirectToAction(nameof(Index));
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para eliminar una canción.";
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var song = await _songsRepository.GetSongByIdAsync(id.Value);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _userService.IsCurrentUserAdminAsync(HttpContext.Session))
            {
                TempData["Message"] = "Necesitas ser administrador para eliminar una canción.";
                return RedirectToAction(nameof(Index));
            }
            await _songsRepository.DeleteSongAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
