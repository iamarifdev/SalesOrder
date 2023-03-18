using FluentValidation;
using SalesOrder.Common.DTO.Order;

namespace SalesOrder.Common.Validators;

public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithName("Name is required");
        RuleFor(x => x.State).NotEmpty().WithName("State is required");
        RuleForEach(x => x.Windows).SetValidator(new WindowCreateDtoValidator());
    }
}

public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
{
    public OrderUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Order Id is required");
        RuleFor(x => x.Name).NotEmpty().WithName("Name is required");
        RuleFor(x => x.State).NotEmpty().WithName("State is required");
    }
}