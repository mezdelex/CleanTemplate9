using Application.Messages;
using FluentValidation;

namespace Application.Categories.PatchAsync;

public sealed class PatchCategoryCommandValidator : AbstractValidator<PatchCategoryCommand>
{
    private readonly int NameMaxLength = 30;
    private readonly int DescriptionMaxLength = 256;

    public PatchCategoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchCategoryCommand.Id))
            )
            .NotNull()
            .WithMessage(GenericValidationMessages.ShouldNotBeNull(nameof(PatchCategoryCommand.Id)))
            .Must(id => id.GetType() == typeof(Guid))
            .WithMessage(GenericValidationMessages.ShouldBeAGuid(nameof(PatchCategoryCommand.Id)));


        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchCategoryCommand.Name))
            )
            .NotNull()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeNull(nameof(PatchCategoryCommand.Name))
            )
            .MaximumLength(NameMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldNotBeLongerThan(
                    nameof(PatchCategoryCommand.Name),
                    NameMaxLength
                )
            );

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchCategoryCommand.Description))
            )
            .NotNull()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeNull(nameof(PatchCategoryCommand.Description))
            )
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldNotBeLongerThan(
                    nameof(PatchCategoryCommand.Description),
                    DescriptionMaxLength
                )
            );
    }
}