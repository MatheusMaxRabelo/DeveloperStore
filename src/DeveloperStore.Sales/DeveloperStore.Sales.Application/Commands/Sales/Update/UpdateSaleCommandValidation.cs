using DeveloperStore.Sales.Application.DTOs;
using FluentValidation;

namespace DeveloperStore.Sales.Application.Commands.Sales.Update;

public class UpdateSaleCommandValidation : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidation()
    {
        RuleFor(x => x.SalesDate)
            .NotEmpty().WithMessage(Constants.Validations.REQUIRED)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(Constants.Validations.DATE_SHOULD_BE_LESS_OR_EQUAL_TO);

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_ZERO);

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_OR_EQUAL_TO_ZERO);

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage(Constants.Validations.REQUIRED);

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage(Constants.Validations.SHOULD_NOT_BE_EMPTY)
            .ForEach(product =>
            {
                product.SetValidator(new UpdateProductRequestValidator());
            });

        RuleFor(x => x.IsCancelled)
            .NotNull().WithMessage(Constants.Validations.REQUIRED);
    }
}

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_ZERO);

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_OR_EQUAL_TO_ZERO);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_ZERO);

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_OR_EQUAL_TO_ZERO)
            .LessThanOrEqualTo(0.2m).WithMessage(Constants.Validations.SHOULD_BE_LESS_THAN_TWENTY);

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage(Constants.Validations.SHOULD_BE_GREATER_THAN_OR_EQUAL_TO_ZERO);

        RuleFor(x => x.IsCancelled)
            .NotNull().WithMessage(Constants.Validations.REQUIRED);
    }
}