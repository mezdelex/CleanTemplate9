using Application.Messages;
using FluentValidation;

namespace Application.Expenses.PatchAsync;

public sealed class PatchExpenseCommandValidator : AbstractValidator<PatchExpenseCommand>
{
    private readonly int NameMaxLength = 30;
    private readonly int DescriptionMaxLength = 256;

    public PatchExpenseCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Id)))
            .Must(id => id.GetType() == typeof(Guid))
            .WithMessage(GenericValidationMessages.ShouldBeAGuid(nameof(PatchExpenseCommand.Id)));

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Name))
            )
            .MaximumLength(NameMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldNotBeLongerThan(
                    nameof(PatchExpenseCommand.Name),
                    NameMaxLength
                )
            );

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Description))
            )
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldNotBeLongerThan(
                    nameof(PatchExpenseCommand.Description),
                    DescriptionMaxLength
                )
            );

        RuleFor(c => c.Value)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Value))
            );

        RuleFor(c => c.CategoryId)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.CategoryId))
            );
    }
}
