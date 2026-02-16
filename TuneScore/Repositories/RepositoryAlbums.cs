using TuneScore.Models;
using TuneScore.Repositories.Interfaces;

namespace TuneScore.Repositories
{
    public class RepositoryAlbums : IRepositoryAlbums
    {
        public Task AddAlbumAsync(Album album)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAlbumAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Album?> GetAlbumByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAlbumAsync(Album album)
        {
            throw new NotImplementedException();
        }
    }
}
