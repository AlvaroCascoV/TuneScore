using TuneScore.Models;

namespace TuneScore.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task RegisterUserAsync(Register registerModel);
        Task<V_UserLogin?> GetUserForLoginAsync(string username);
        Task<User?> GetUserWithRatingsAsync(int userId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(int userId, string username, string email, string? passwordPlain = null);
    }
}
