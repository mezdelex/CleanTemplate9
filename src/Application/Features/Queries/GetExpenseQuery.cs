namespace Application.Features.Queries;

public sealed record GetExpenseQuery(Guid Id) : IRequest<ExpenseDTO>
{
    public sealed class GetExpenseQueryHandler : IRequestHandler<GetExpenseQuery, ExpenseDTO>
    {
        private readonly IApplicationDbContext _context;

        public GetExpenseQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseDTO> Handle(
            GetExpenseQuery request,
            CancellationToken cancellationToken
        )
        {
            var expense =
                await _context
                    .Expenses.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new ExpenseNotFoundException(request.Id);

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
