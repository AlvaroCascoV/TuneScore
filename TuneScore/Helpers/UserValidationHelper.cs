using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TuneScore.Helpers
{
    public static class UserValidationHelper
    {
        public static bool ValidateLoginInput(ModelStateDictionary modelState, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                modelState.AddModelError(string.Empty, "Username and password are required.");
                return false;
            }

            return true;
        }

        public static bool ValidateRegisterPasswords(ModelStateDictionary modelState, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                modelState.AddModelError(string.Empty, "Passwords do not match.");
                return false;
            }

            return true;
        }
    }
}

