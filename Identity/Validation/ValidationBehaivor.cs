
using FluentValidation;
using FluentValidation.Results;
using Identity.Commands;
using MediatR;
using System.Net.Mail;

namespace Identity.Validators;


public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(
                context,
                cancellationToken);

            failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await next();
    }
}


public sealed class RegisterValidator
    : AbstractValidator<RegisterCommand>
{
    private const int UsernameMinLength = 3;
    private const int UsernameMaxLength = 50;
    private const int PasswordMinLength = 12;
    private const int PasswordMaxLength = 128;
    private const int EmailMaxLength = 254;

    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Username is required")
            .Length(UsernameMinLength, UsernameMaxLength)
                .WithMessage(
                    $"Username must be between {UsernameMinLength} and {UsernameMaxLength} characters")
            .Must(x => x == x.Trim())
                .WithMessage("Username cannot start or end with whitespace")
            .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage(
                    "Username can contain only English letters, numbers and underscores");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Password is required")
            .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Password cannot contain only whitespace")
            .MinimumLength(PasswordMinLength)
                .WithMessage(
                    $"Password must be at least {PasswordMinLength} characters long")
            .MaximumLength(PasswordMaxLength)
                .WithMessage(
                    $"Password cannot exceed {PasswordMaxLength} characters")
            .Must(x => !x.Contains('\0'))
                .WithMessage("Password contains an invalid character");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Email is required")
            .Must(x => x == x.Trim())
                .WithMessage("Email cannot start or end with whitespace")
            .MaximumLength(EmailMaxLength)
                .WithMessage(
                    $"Email cannot exceed {EmailMaxLength} characters")
            .Must(EmailValidation.IsValid)
                .WithMessage("Email address is invalid");
    }
}


public sealed class AuthValidator : AbstractValidator<AuthCommand>
{
    public AuthValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Email is required")
            .Must(x => x == x.Trim())
                .WithMessage("Email cannot start or end with whitespace")
            .MaximumLength(254)
                .WithMessage("Email cannot exceed 254 characters")
            .Must(EmailValidation.IsValid)
                .WithMessage("Email address is invalid");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Password is required")
            .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Password cannot contain only whitespace")
            .MaximumLength(128)
                .WithMessage("Password cannot exceed 128 characters")
            .Must(x => !x.Contains('\0'))
                .WithMessage("Password contains an invalid character");
    }
}


public sealed class EmailVerificationValidator
    : AbstractValidator<EmailVerificationCommand>
{
    public EmailVerificationValidator()
    {
        RuleFor(x => x.CodeId)
            .NotEmpty()
            .WithMessage("Verification code ID is required");

        RuleFor(x => x.code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Verification code is required")
            .Length(4)
                .WithMessage(
                    "Verification code must contain exactly 4 characters")
            .Matches(@"^[0-9]{4}$")
                .WithMessage(
                    "Verification code must contain only digits");
    }
}

public sealed class TwoFactorEnableValidator
    : AbstractValidator<TwoFactorEnableConfirmCommand>
{
    public TwoFactorEnableValidator()
    {
        RuleFor(x => x.CodeId)
            .NotEmpty()
            .WithMessage("Verification code ID is required");

        RuleFor(x => x.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Verification code is required")
            .Length(4)
            .WithMessage(
                "Verification code must contain exactly 4 characters")
            .Matches(@"^[0-9]{4}$")
            .WithMessage(
                "Verification code must contain only digits");
    }
}
public sealed class TwoFactorLoginVerifyValidator
    : AbstractValidator<TwoFactorLoginVerifyCommand>
{
    public TwoFactorLoginVerifyValidator()
    {
        RuleFor(x => x.CodeId)
            .NotEmpty()
            .WithMessage("Verification code ID is required");

        RuleFor(x => x.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Verification code is required")
            .Length(4)
            .WithMessage(
                "Verification code must contain exactly 4 characters")
            .Matches(@"^[0-9]{4}$")
            .WithMessage(
                "Verification code must contain only digits");
    }
}



public sealed class JWTRequestValidator
    : AbstractValidator<JWTRequestCommand>
{
    public JWTRequestValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("UserId is required")
            .Must(x => Guid.TryParse(x, out var id) && id != Guid.Empty)
                .WithMessage("UserId must be a valid, non-empty GUID");

        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Username is required")
            .Length(3, 50)
                .WithMessage("Username must be between 3 and 50 characters")
            .Must(x => x == x.Trim())
                .WithMessage("Username cannot start or end with whitespace");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Email is required")
            .MaximumLength(254)
                .WithMessage("Email cannot exceed 254 characters")
            .Must(EmailValidation.IsValid)
                .WithMessage("Email address is invalid");

        RuleFor(x => x.RegistrationDate)
            .NotEqual(default(DateTime))
                .WithMessage("RegistrationDate is required")
            .Must(date => date <= DateTime.UtcNow)
                .WithMessage("RegistrationDate cannot be in the future");
    }
}


internal static class EmailValidation
{
    public static bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var address = new MailAddress(email);

            return string.Equals(
                address.Address,
                email,
                StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
