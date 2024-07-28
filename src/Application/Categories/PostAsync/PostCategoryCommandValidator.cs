using Application.Messages;
using FluentValidation;

namespace Application.Categories.PostAsync;

public sealed class PostCategoryCommandValidator : AbstractValidator<PostCategoryCommand>
{
    private readonly int NameMaxLength = 30;
    private readonly int DescriptionMaxLength = 256;

    public PostCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PostCategoryCommand.Name))
            )
            .NotNull()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeNull(nameof(PostCategoryCommand.Name))
            )
            .MaximumLength(NameMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldBeThisCharactersLong(
                    nameof(PostCategoryCommand.Name),
                    NameMaxLength
                )
            );

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeEmpty(nameof(PostCategoryCommand.Description))
            )
            .NotNull()
            .WithMessage(
                GenericValidationMessages.ShouldNotBeNull(nameof(PostCategoryCommand.Description))
            )
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(
                GenericValidationMessages.ShouldBeThisCharactersLong(
                    nameof(PostCategoryCommand.Description),
                    DescriptionMaxLength
                )
            );
    }
}