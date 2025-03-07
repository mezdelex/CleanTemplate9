namespace Application.Features.Queries;

public sealed record GetCategoryQuery(string Id) : IRequest<CategoryDTO>
{
    public sealed class GetCategoryQueryHandler(ICategoriesRepository repository, IMapper mapper)
        : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly ICategoriesRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

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

            return _mapper.Map<CategoryDTO>(category);
        }
    }

    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Id)))
                .MaximumLength(CategoryConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Id),
                        CategoryConstraints.IdMaxLength
                    )
                );
        }
    }
}
