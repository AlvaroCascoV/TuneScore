using Microsoft.AspNetCore.Http;

namespace TuneScore.Helpers
{
    /// <summary>
    /// Static helpers to read the current user from session (set on login).
    /// Use for access checks in controllers or views.
    /// </summary>
    public static class SessionHelper
    {
        public const string UserIdKey = "UserId";
        public const string UsernameKey = "Username";
        public const string EmailKey = "Email";
        public const string RoleKey = "Role";

        /// <summary>Role name that is allowed to create/edit/delete albums.</summary>
        public const string AdminRole = "Admin";

        public static bool IsLoggedIn(ISession session)
        {
            return session.GetInt32(UserIdKey).HasValue;
        }

        public static int? GetUserId(ISession session)
        {
            return session.GetInt32(UserIdKey);
        }

        public static string? GetUsername(ISession session)
        {
            return session.GetString(UsernameKey);
        }

        public static string? GetEmail(ISession session)
        {
            return session.GetString(EmailKey);
        }

        public static string? GetRole(ISession session)
        {
            return session.GetString(RoleKey);
        }

        /// <summary>
        /// Returns true if the current user has the Admin role. Use for album create/edit/delete access.
        /// </summary>
        public static bool IsAdmin(ISession session)
        {
            return string.Equals(GetRole(session), AdminRole, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Use in controllers: redirect to login if not logged in.
        /// </summary>
        public static bool RequireLogin(ISession session, out int userId)
        {
            var id = session.GetInt32(UserIdKey);
            if (id.HasValue)
            {
                userId = id.Value;
                return true;
            }
            userId = 0;
            return false;
        }
    }
}
