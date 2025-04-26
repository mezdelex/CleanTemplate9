namespace Infrastructure.Repositories;

public class ApplicationUsersRepository(
    ApplicationDbContext context,
    ISpecificationEvaluator evaluator
) : BaseRepository<ApplicationUser>(context, evaluator), IApplicationUsersRepository { }
