namespace Application.Features.Queries;

public sealed record GetApplicationUserQuery(string Id) : BaseRequest, IRequest<ApplicationUserDTO>
{
    public sealed class GetApplicationUserQueryHandler(
        IApplicationUsersRepository repository,
        IMapper mapper
    ) : IRequestHandler<GetApplicationUserQuery, ApplicationUserDTO>
    {
        private readonly IApplicationUsersRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApplicationUserDTO> Handle(
            GetApplicationUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var applicationUser =
                await _repository.GetBySpecAsync(
                    new ApplicationUsersSpecification(id: request.Id),
                    cancellationToken
                ) ?? throw new NotFoundException(request.Id);

            return _mapper.Map<ApplicationUserDTO>(applicationUser);
        }
    }
}
