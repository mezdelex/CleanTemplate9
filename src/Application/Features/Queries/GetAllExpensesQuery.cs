namespace Application.Features.Queries;

public sealed record GetAllExpensesQuery : BaseRequest, IRequest<PagedList<ExpenseDTO>>
{
    public string? Name { get; set; }
    public string? ContainedWord { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public Guid? CategoryId { get; set; }

    public sealed class GetAllExpensesQueryHandler
        : IRequestHandler<GetAllExpensesQuery, PagedList<ExpenseDTO>>
    {
        private readonly IExpensesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IRedisCache _redisCache;

        public GetAllExpensesQueryHandler(
            IExpensesRepository repository,
            IMapper mapper,
            IRedisCache redisCache
        )
        {
            _repository = repository;
            _mapper = mapper;
            _redisCache = redisCache;
        }

        public async Task<PagedList<ExpenseDTO>> Handle(
            GetAllExpensesQuery request,
            CancellationToken cancellationToken
        )
        {
            var redisKey =
                $"{nameof(GetAllExpensesQuery)}#{request.Name}#{request.ContainedWord}#{request.MinDate}#{request.MaxDate}#{request.CategoryId}#{request.Page}#{request.PageSize}";
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
                        minDate: request.MinDate,
                        maxDate: request.MaxDate,
                        categoryId: request.CategoryId
                    )
                )
                .Select(e => _mapper.Map<ExpenseDTO>(e))
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
