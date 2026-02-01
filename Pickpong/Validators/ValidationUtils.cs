using System.Text.RegularExpressions;

namespace Pickpong.Validators
{
    public class ValidationUtils
    {
        public static bool IsValidEmailOrPhone(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            bool isEmail = input.Contains("@");
            bool isValid = isEmail
                ? Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                : Regex.IsMatch(input, @"^05\d{8}$");
            return isValid;
        }

    }
}