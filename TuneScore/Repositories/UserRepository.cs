using TuneScore.Data;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;

public class UserRepository : IUserRepository
{
    private readonly TuneScoreContext _context;

    public UserRepository(TuneScoreContext context)
    {
        _context = context;
    }

    public async Task RegisterUserAsync(User user, byte[] hash, byte[] salt)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userSalt = new UserSalt
        {
            UserId = user.Id,
            PasswordHash = hash,
            Salt = salt
        };

        _context.UserSalts.Add(userSalt);
        await _context.SaveChangesAsync();
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
}