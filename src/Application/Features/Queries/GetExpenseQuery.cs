namespace Application.Features.Queries;

public sealed record GetExpenseQuery(Guid Id) : IRequest<ExpenseDTO>
{
    public sealed class GetExpenseQueryHandler : IRequestHandler<GetExpenseQuery, ExpenseDTO>
    {
        private readonly IExpensesRepository _repository;
        private readonly IMapper _mapper;

        public GetExpenseQueryHandler(IExpensesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExpenseDTO> Handle(
            GetExpenseQuery request,
            CancellationToken cancellationToken
        )
        {
            var expense =
                await _repository.GetBySpecAsync(
                    new ExpensesSpecification(id: request.Id),
                    cancellationToken
                ) ?? throw new NotFoundException(request.Id);

            return _mapper.Map<ExpenseDTO>(expense);
        }
    }

    public class GetExpenseQueryValidator : AbstractValidator<GetExpenseQuery>
    {
        public GetExpenseQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Id)));
        }
    }
}
