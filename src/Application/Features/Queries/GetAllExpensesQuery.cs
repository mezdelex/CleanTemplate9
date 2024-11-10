namespace Application.Features.Queries;

public record GetAllExpensesQuery(int Page, int PageSize) : IRequest<PagedList<ExpenseDTO>>
{
    public sealed class GetAllExpensesQueryHandler
        : IRequestHandler<GetAllExpensesQuery, PagedList<ExpenseDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IRedisCache _redisCache;

        public GetAllExpensesQueryHandler(IApplicationDbContext context, IRedisCache redisCache)
        {
            _context = context;
            _redisCache = redisCache;
        }

        public async Task<PagedList<ExpenseDTO>> Handle(
            GetAllExpensesQuery request,
            CancellationToken cancellationToken
        )
        {
            var redisKey = $"{nameof(GetAllExpensesQuery)}#{request.Page}#{request.PageSize}";
            var cachedGetAllExpensesQuery = await _redisCache.GetCachedData<PagedList<ExpenseDTO>>(
                redisKey
            );
            if (cachedGetAllExpensesQuery != null)
                return cachedGetAllExpensesQuery;

            var pagedExpenses = await _context
                .Expenses.AsNoTracking()
                .OrderBy(e => e.Name)
                .Select(e => new ExpenseDTO(e.Id, e.Name, e.Description, e.Value, e.CategoryId))
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
