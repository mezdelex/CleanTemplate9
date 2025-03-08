namespace Application.Features.Queries;

public sealed record GetAllExpensesQuery : BaseRequest, IRequest<PagedList<ExpenseDTO>>
{
    public string? Name { get; set; }
    public string? ContainedWord { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public string? CategoryId { get; set; }
    public string? ApplicationUserId { get; set; }

    public sealed class GetAllExpensesQueryHandler(
        IExpensesRepository repository,
        IMapper mapper,
        IRedisCache redisCache
    ) : IRequestHandler<GetAllExpensesQuery, PagedList<ExpenseDTO>>
    {
        private readonly IExpensesRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IRedisCache _redisCache = redisCache;

        public async Task<PagedList<ExpenseDTO>> Handle(
            GetAllExpensesQuery request,
            CancellationToken cancellationToken
        )
        {
            var redisKey =
                $"{nameof(Expense)}#{request.Name}#{request.ContainedWord}#{request.MinDate}#{request.MaxDate}#{request.CategoryId}#{request.ApplicationUserId}#{request.Page}#{request.PageSize}";
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
                        categoryId: request.CategoryId,
                        applicationUserId: request.ApplicationUserId
                    )
                )
                .Select(e => _mapper.Map<ExpenseDTO>(e))
                .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

            await _redisCache.SetCachedData(
                redisKey,
                pagedExpenses,
                DateTimeOffset.Now.AddMinutes(5)
            );

            return pagedExpenses;
        }
    }

    public class GetAllExpensesQueryValidator : AbstractValidator<GetAllExpensesQuery>
    {
        public GetAllExpensesQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Name),
                        ExpenseConstraints.NameMaxLength
                    )
                )
                .When(x => x.Name is not null);

            RuleFor(x => x.ContainedWord)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(ContainedWord),
                        ExpenseConstraints.DescriptionMaxLength
                    )
                )
                .When(x => x.Name is not null);
        }
    }
}
