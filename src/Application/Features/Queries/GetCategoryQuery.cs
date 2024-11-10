namespace Application.Features.Queries;

public sealed record GetCategoryQuery(Guid Id) : IRequest<CategoryDTO>
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDTO> Handle(
            GetCategoryQuery request,
            CancellationToken cancellationToken
        )
        {
            var category =
                await _context
                    .Categories.AsNoTracking()
                    .Include(c => c.Expenses)
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                ?? throw new CategoryNotFoundException(request.Id);

            return new CategoryDTO(
                category.Id,
                category.Name,
                category.Description,
                category?.Expenses
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
