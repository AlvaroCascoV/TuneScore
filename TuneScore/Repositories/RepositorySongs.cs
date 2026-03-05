using Microsoft.EntityFrameworkCore;
using TuneScore.Data;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;

namespace TuneScore.Repositories
{
    public class RepositorySongs : IRepositorySongs
    {
        private readonly TuneScoreContext _context;

        public RepositorySongs(TuneScoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByAlbumAsync(int albumId)
        {
            return await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .Where(s => s.AlbumId == albumId)
                .ToListAsync();
        }

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddSongAsync(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }
    }
}
