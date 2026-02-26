using TuneScore.Models;

namespace TuneScore.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task RegisterUserAsync(Register registerModel);
        Task<V_UserLogin?> GetUserForLoginAsync(string username);
        Task<User?> GetUserWithRatingsAsync(int userId);
    }
}
