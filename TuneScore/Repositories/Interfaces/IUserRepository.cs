using TuneScore.Models;

namespace TuneScore.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task RegisterUserAsync(User user, byte[] hash, byte[] salt);
        Task<V_UserLogin?> GetUserForLoginAsync(string username);
        Task<User?> GetUserWithRatingsAsync(int userId);
    }
}
