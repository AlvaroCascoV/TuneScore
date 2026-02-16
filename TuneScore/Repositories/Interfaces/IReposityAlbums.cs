using TuneScore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuneScore.Repositories.Interfaces
{
    public interface IRepositoryAlbums
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album?> GetAlbumByIdAsync(int id);
        Task AddAlbumAsync(Album album);
        Task UpdateAlbumAsync(Album album);
        Task DeleteAlbumAsync(int id);
    }
}
