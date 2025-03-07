namespace Application.Features.Queries;

public sealed record GetExpenseQuery(string Id) : IRequest<ExpenseDTO>
{
    public sealed class GetExpenseQueryHandler(IExpensesRepository repository, IMapper mapper)
        : IRequestHandler<GetExpenseQuery, ExpenseDTO>
    {
        private readonly IExpensesRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

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
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Id)))
                .MaximumLength(ExpenseConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Id),
                        ExpenseConstraints.IdMaxLength
                    )
                );
        }
    }
}
