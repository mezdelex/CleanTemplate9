namespace Infrastructure.Repositories;

public class ApplicationUsersRepository(
    ApplicationDbContext context,
    ISpecificationEvaluator evaluator
)
    : BaseRepository<IBaseEntity, ApplicationUser>(context, evaluator),
        IApplicationUsersRepository
{ }
