namespace Application.Features.Queries;

public sealed record GetCategoryQuery(Guid Id) : IRequest<CategoryDTO>
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly ICategoriesRepository _repository;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(ICategoriesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

            return _mapper.Map<CategoryDTO>(category);
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
