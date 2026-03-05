using TuneScore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuneScore.Repositories.Interfaces
{
    public interface IRepositorySongs
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<IEnumerable<Song>> GetSongsByAlbumAsync(int albumId);
        Task<Song?> GetSongByIdAsync(int id);
        Task AddSongAsync(Song song);
        Task UpdateSongAsync(Song song);
        Task DeleteSongAsync(int id);
    }
}
