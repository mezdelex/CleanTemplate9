namespace Application.Profiles;

public class ExpensesProfile : Profile
{
    public ExpensesProfile()
    {
        CreateMap<Expense, ExpenseDTO>();
        CreateMap<Expense, PatchedExpenseEvent>();
        CreateMap<Expense, PostedExpenseEvent>();
        CreateMap<PatchExpenseCommand, Expense>();
        CreateMap<PostExpenseCommand, Expense>();
    }
}
