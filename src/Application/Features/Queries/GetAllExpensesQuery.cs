namespace Application.Features.Queries;

public sealed record GetAllExpensesQuery : BaseRequest, IRequest<PagedList<ExpenseDTO>>
{
    public string? Name { get; set; }
    public string? ContainedWord { get; set; }
    public Guid? CategoryId { get; set; }

    public sealed class GetAllExpensesQueryHandler
        : IRequestHandler<GetAllExpensesQuery, PagedList<ExpenseDTO>>
    {
        private readonly IExpensesRepository _repository;
        private readonly IRedisCache _redisCache;

        public GetAllExpensesQueryHandler(IExpensesRepository repository, IRedisCache redisCache)
        {
            _repository = repository;
            _redisCache = redisCache;
        }

        public async Task<PagedList<ExpenseDTO>> Handle(
            GetAllExpensesQuery request,
            CancellationToken cancellationToken
        )
        {
            var redisKey = $"{nameof(GetAllExpensesQuery)}#{request.Page}#{request.PageSize}";
            var cachedPagedExpenses = await _redisCache.GetCachedData<PagedList<ExpenseDTO>>(
                redisKey
            );
            if (cachedPagedExpenses != null)
                return cachedPagedExpenses;

            var pagedExpenses = await _repository
                .ApplySpecification(
                    new ExpensesSpecification(
                        name: request.Name,
                        containedWord: request.ContainedWord,
                        categoryId: request.CategoryId
                    )
                )
                .Select(x => new ExpenseDTO(x.Id, x.Name, x.Description, x.Value, x.CategoryId))
                .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

            await _redisCache.SetCachedData<PagedList<ExpenseDTO>>(
                redisKey,
                pagedExpenses,
                DateTimeOffset.Now.AddMinutes(5)
            );

            return pagedExpenses;
        }
    }
}
