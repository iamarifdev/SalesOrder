using FluentValidation;
using SalesOrder.Common.DTO.Element;

namespace SalesOrder.Common.Validators;

public class SubElementCreateDtoValidator : AbstractValidator<SubElementCreateDto>
{
    public SubElementCreateDtoValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order Id is required");
        RuleFor(x => x.WindowId).NotEmpty().WithMessage("Window Id is required");
        RuleFor(x => x.Element).NotEmpty().WithMessage("Element is required");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
        RuleFor(x => x.Width).GreaterThan(0).WithMessage("Width must be greater than 0");
        RuleFor(x => x.Height).GreaterThan(0).WithMessage("Height must be greater than 0");
    }
}

public class SubElementUpdateDtoValidator : AbstractValidator<SubElementUpdateDto>
{
    public SubElementUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Sub Element Id is required");
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order Id is required");
        RuleFor(x => x.WindowId).NotEmpty().WithMessage("Window Id is required");
        RuleFor(x => x.Element).NotEmpty().WithMessage("Element is required");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
        RuleFor(x => x.Width).GreaterThan(0).WithMessage("Width must be greater than 0");
        RuleFor(x => x.Height).GreaterThan(0).WithMessage("Height must be greater than 0");
    }
}