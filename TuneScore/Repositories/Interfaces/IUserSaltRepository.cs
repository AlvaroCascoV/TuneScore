using TuneScore.Models;

namespace TuneScore.Repositories.Interfaces
{
    public interface IUserSaltRepository
    {
        Task CreateAsync(int userId, byte[] passwordHash, string salt);
    }
}

