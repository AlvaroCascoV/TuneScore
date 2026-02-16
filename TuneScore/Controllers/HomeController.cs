using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuneScore.Data;

public class HomeController : Controller
{
    private readonly TuneScoreContext _context;

    public HomeController(TuneScoreContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var songs = await _context.Songs
            .Include(s => s.Album)
            .ThenInclude(a => a.Artist)
            .ToListAsync();

        return View(songs);
    }
}
