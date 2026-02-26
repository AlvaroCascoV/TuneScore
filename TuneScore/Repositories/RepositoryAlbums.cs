using Microsoft.EntityFrameworkCore;
using TuneScore.Data;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;

namespace TuneScore.Repositories
{
    public class RepositoryAlbums : IRepositoryAlbums
    {
        private readonly TuneScoreContext _context;

        public RepositoryAlbums(TuneScoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .ToListAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(int id)
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAlbumAsync(Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlbumAsync(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }
    }
}
