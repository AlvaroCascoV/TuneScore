using TuneScore.Data;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;

namespace TuneScore.Repositories
{
    public class UserSaltRepository : IUserSaltRepository
    {
        private readonly TuneScoreContext _context;

        public UserSaltRepository(TuneScoreContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(int userId, byte[] passwordHash, string salt)
        {
            var userSalt = new UserSalt
            {
                UserId = userId,
                PasswordHash = passwordHash,
                Salt = salt
            };

            _context.UserSalts.Add(userSalt);
            await _context.SaveChangesAsync();
        }
    }
}

