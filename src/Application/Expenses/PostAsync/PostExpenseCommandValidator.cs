using Application.Messages;
using FluentValidation;

namespace Application.Expenses.PostAsync;

public sealed class PostExpenseCommandValidator : AbstractValidator<PostExpenseCommand>
{
    private readonly int NameMaxLength = 30;
    private readonly int DescriptionMaxLength = 256;

    public PostExpenseCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PostExpenseCommand.Name))
            )
            .MaximumLength(NameMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldNotBeLongerThan(
                    nameof(PostExpenseCommand.Name),
                    NameMaxLength
                )
            );

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PostExpenseCommand.Description))
            )
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldNotBeLongerThan(
                    nameof(PostExpenseCommand.Description),
                    DescriptionMaxLength
                )
            );

        RuleFor(c => c.Value)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PostExpenseCommand.Value))
            );

        RuleFor(c => c.CategoryId)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PostExpenseCommand.CategoryId))
            );
    }
}