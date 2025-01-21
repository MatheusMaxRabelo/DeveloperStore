using FluentValidation;

namespace DeveloperStore.Sales.Application.Commands.Sales.Create;

public class CreateSaleValidation : AbstractValidator<CreateSaleCommand>
{
    private const string REQUIRED = "{PropertyName} is required!";
    private const string SHOULD_NOT_BE_EMPTY = "{PropertyName} should not be empty!";

    public CreateSaleValidation()
    {
        RuleFor(s => s.Branch)
            .NotEmpty().WithMessage(SHOULD_NOT_BE_EMPTY);

        RuleFor(p => p.Products)
            .NotNull().WithMessage(REQUIRED)
            .NotEmpty().WithMessage(SHOULD_NOT_BE_EMPTY);

        RuleForEach(p => p.Products).SetValidator(new ProductRequestValidator());
    }
}

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{

    private const string SHOULD_BE_GREATER_THAN_ZERO = "The {PropertyName} must be greater than 0.";
    private const string SHOULD_BE_LESS_THAN_TWENTY = "The {PropertyName} must be less than or equal to 20.";

    public ProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage(SHOULD_BE_GREATER_THAN_ZERO);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(SHOULD_BE_GREATER_THAN_ZERO)
            .LessThanOrEqualTo(20).WithMessage(SHOULD_BE_LESS_THAN_TWENTY);
    }
}