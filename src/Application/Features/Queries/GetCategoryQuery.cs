namespace Application.Features.Queries;

public sealed record GetCategoryQuery(Guid Id) : IRequest<CategoryDTO>
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly ICategoriesRepository _repository;

        public GetCategoryQueryHandler(ICategoriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDTO> Handle(
            GetCategoryQuery request,
            CancellationToken cancellationToken
        )
        {
            var category =
                await _repository.GetBySpecAsync(
                    new CategoriesSpecification(id: request.Id),
                    cancellationToken
                ) ?? throw new NotFoundException(request.Id);

            return new CategoryDTO(
                category.Id,
                category.Name,
                category.Description,
                category.Expenses
            );
        }
    }

    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty("Id"));
        }
    }
}
