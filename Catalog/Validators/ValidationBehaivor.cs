using Catalog.Commands;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.IO;
using System.Linq;

namespace Catalog.Validators;

// MediatR pipeline behavior
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

// Find item validator
public sealed class FindItemRequestValidator
    : AbstractValidator<FindItemRequestCommand>
{
    private const int MinPage = 1;
    private const int MaxPageSize = 100;
    private const int MaxRequestLength = 200;

    public FindItemRequestValidator()
    {
        RuleFor(x => x.request)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Search request is required")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Search request cannot contain only whitespace")
            .Must(x => x == x.Trim())
            .WithMessage("Search request cannot start or end with whitespace")
            .MaximumLength(MaxRequestLength)
            .WithMessage(
                $"Search request cannot exceed {MaxRequestLength} characters");

        RuleFor(x => x.page)
            .GreaterThanOrEqualTo(MinPage)
            .WithMessage($"Page must be at least {MinPage}");

        RuleFor(x => x.pageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage($"Page size must be between 1 and {MaxPageSize}");

        RuleFor(x => x.userId)
            .NotEqual(Guid.Empty)
            .WithMessage("User ID must not be empty");
    }
}

// Add item validator
public sealed class AddItemValidator : AbstractValidator<AddItemCommand>
{
    private const int MaxItemNameLength = 100;
    private const int MaxCategoryLength = 100;
    private const decimal MaxPrice = 1_000_000_000m;
    private const long MaxImageSize = 5 * 1024 * 1024; // 5 MB

    public AddItemValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("User ID must not be empty");

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
                $"Category cannot exceed {MaxCategoryLength} characters");

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
                        Path.GetExtension(image.FileName)
                            .ToLowerInvariant()))
            .WithMessage("Only JPG, PNG and WebP images are allowed");
    }
}

// Report item validator
public sealed class ReportItemRequestValidator
    : AbstractValidator<ReportItemRequest>
{
    private const int MaxTextLength = 2000;

    public ReportItemRequestValidator()
    {
        RuleFor(x => x.text)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Report text is required")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Report text cannot contain only whitespace")
            .MaximumLength(MaxTextLength)
            .WithMessage(
                $"Report text cannot exceed {MaxTextLength} characters");

        RuleFor(x => x.itemId)
            .NotEqual(Guid.Empty)
            .WithMessage("Item ID must not be empty");
    }
}