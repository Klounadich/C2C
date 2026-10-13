
using Catalog.Commands;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.IO;
using System.Linq;

namespace Profile.Validators;

public sealed class ValidationBehaivor<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var context = new ValidationContext<TRequest>(request);

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

public sealed class ChangeItemCommandValidator
    : AbstractValidator<ChangeItemCommand>
{
    private const int MaxItemNameLength = 100;
    private const int MaxCategoryLength = 100;
    private const decimal MaxPrice = 1_000_000_000m;
    private const long MaxImageSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedCategories =
    [
        "electronics",
        "clothes",
        "home"
    ];

    public ChangeItemCommandValidator()
    {
        RuleFor(x => x.userId)
            .NotEqual(Guid.Empty)
            .WithMessage("User ID must not be empty");

        RuleFor(x => x.ItemId)
            .NotEqual(Guid.Empty)
            .WithMessage("Item ID must not be empty");

        RuleFor(x => x.itemName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Item name is required")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Item name cannot contain only whitespace")
            .MaximumLength(MaxItemNameLength)
            .WithMessage(
                $"Item name cannot exceed {MaxItemNameLength} characters");

        RuleFor(x => x.category)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Category is required")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Category cannot contain only whitespace")
            .MaximumLength(MaxCategoryLength)
            .WithMessage(
                $"Category cannot exceed {MaxCategoryLength} characters")
            .Must(category => AllowedCategories.Contains(
                category,
                StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                "Category must be electronics, clothes or home");

        RuleFor(x => x.price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero")
            .LessThanOrEqualTo(MaxPrice)
            .WithMessage($"Price cannot exceed {MaxPrice}");

        RuleFor(x => x.city)
            .NotEqual(Guid.Empty)
            .WithMessage("City ID must not be empty");

        RuleFor(x => x.image)
            .Must(image => image == null || image.Length > 0)
            .WithMessage("Image cannot be empty")
            .Must(image => image == null || image.Length <= MaxImageSize)
            .WithMessage("Image size cannot exceed 5 MB")
            .Must(image =>
                image == null ||
                new[] { ".jpg", ".jpeg", ".png", ".webp" }
                    .Contains(
                        Path.GetExtension(image.FileName),
                        StringComparer.OrdinalIgnoreCase))
            .WithMessage("Only JPG, PNG and WebP images are allowed");
    }
}
