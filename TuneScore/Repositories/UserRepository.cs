using Microsoft.EntityFrameworkCore;
using TuneScore.Data;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;

namespace TuneScore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TuneScoreContext _context;

        public UserRepository(TuneScoreContext context)
        {
            _context = context;
        }

        public async Task<V_UserLogin?> GetUserForLoginAsync(string username)
        {
            return await _context.V_UserLogin
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserWithRatingsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Ratings)
                .ThenInclude(r => r.Song)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task RegisterUserAsync(Register registerModel)
        {
            var newUser = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                PasswordPlain = registerModel.PasswordPlain,
                Role = string.IsNullOrWhiteSpace(registerModel.Role) ? "User" : registerModel.Role,
                CreatedAt = registerModel.CreatedAt == default ? DateTime.Now : registerModel.CreatedAt
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Guardo la id autoincremental del usuario
            registerModel.IdUser = newUser.Id;
        }
    }
}