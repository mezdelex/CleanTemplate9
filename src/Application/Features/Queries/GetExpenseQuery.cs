namespace Application.Features.Queries;

public sealed record GetExpenseQuery(Guid Id) : IRequest<ExpenseDTO>
{
    public sealed class GetExpenseQueryHandler : IRequestHandler<GetExpenseQuery, ExpenseDTO>
    {
        private readonly IExpensesRepository _repository;

        public GetExpenseQueryHandler(IExpensesRepository repository)
        {
            _repository = repository;
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

            return new ExpenseDTO(
                expense.Id,
                expense.Name,
                expense.Description,
                expense.Value,
                expense.CategoryId
            );
        }
    }

    public class GetExpenseQueryValidator : AbstractValidator<GetExpenseQuery>
    {
        public GetExpenseQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty("Id"));
        }
    }
}
