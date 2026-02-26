using Microsoft.AspNetCore.Http;
using TuneScore.Models;
using TuneScore.Repositories.Interfaces;
using TuneScore.Helpers;

namespace TuneScore.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSaltRepository _userSaltRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IUserSaltRepository userSaltRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _userSaltRepository = userSaltRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session?.Clear();
        }

        public async Task<V_UserLogin?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserForLoginAsync(username);
            if (user == null)
            {
                return null;
            }

            // Prefer hashed verification if hash + salt are available
            if (user.PasswordHash != null && !string.IsNullOrEmpty(user.Salt))
            {
                var computed = SecurityHelper.EncryptPassword(password, user.Salt);
                if (SecurityHelper.CompareArrays(computed, user.PasswordHash))
                {
                    return user;
                }
            }

            // Fallback for legacy plain-text data (or mismatched legacy hashes)
            if (user.PasswordPlain == password)
            {
                return user;
            }

            return null;
        }

        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            var user = await AuthenticateAsync(username, password);
            return user != null;
        }

        public async Task RegisterAsync(Register registerModel)
        {
            // 1. Create the user row (with plain password for now, to match your seed data)
            await _userRepository.RegisterUserAsync(registerModel);

            // 2. Create hash + salt in UserSalts using your salt generator
            string salt = SecurityHelper.GenerateSalt();
            byte[] hash = SecurityHelper.EncryptPassword(registerModel.PasswordPlain, salt);
            await _userSaltRepository.CreateAsync(registerModel.IdUser, hash, salt);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return user;
        }

        public async Task<bool> UpdateProfileAsync(int userId, string username, string email, string? newPasswordPlain = null)
        {
            return await _userRepository.UpdateUserAsync(userId, username, email, newPasswordPlain);
        }
    }
}
