using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DemoApp.Attributes;

public partial class PasswordComplexityAttribute() : ValidationAttribute("Password do not meet complexity rules.")
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string password)
        {
            if (password.Length >= 8 &&
                AtLeastOneLetterRegex().IsMatch(password) &&
                AtLeastOneNumberRegex().IsMatch(password) &&
                AtLeastOneSpecialCharacterRegex().IsMatch(password))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage ?? "Password do not meet complexity rules.");
    }

    [GeneratedRegex(@"[A-Za-z]")]
    private static partial Regex AtLeastOneLetterRegex();

    [GeneratedRegex(@"\d")]
    private static partial Regex AtLeastOneNumberRegex();

    [GeneratedRegex(@"[@$!%*?&]")]
    private static partial Regex AtLeastOneSpecialCharacterRegex();
}