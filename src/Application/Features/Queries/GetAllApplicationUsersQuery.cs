namespace Application.Features.Queries;

public sealed record GetAllApplicationUsersQuery
    : BaseRequest,
        IRequest<PagedList<ApplicationUserDTO>>
{
    public string? Email { get; set; }
    public string? ContainedWord { get; set; }

    public sealed class GetAllApplicationUsersQueryHandler(
        IApplicationUsersRepository repository,
        IMapper mapper,
        IRedisCache redisCache
    ) : IRequestHandler<GetAllApplicationUsersQuery, PagedList<ApplicationUserDTO>>
    {
        private readonly IApplicationUsersRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IRedisCache _redisCache = redisCache;

        public async Task<PagedList<ApplicationUserDTO>> Handle(
            GetAllApplicationUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var redisKey =
                $"{nameof(GetAllApplicationUsersQuery)}#{request.Email}#{request.ContainedWord}#{request.Page}#{request.PageSize}";
            var cachedPagedApplicationUsers = await _redisCache.GetCachedData<
                PagedList<ApplicationUserDTO>
            >(redisKey);
            if (cachedPagedApplicationUsers != null)
                return cachedPagedApplicationUsers;

            var pagedApplicationUsers = await _repository
                .ApplySpecification(
                    new ApplicationUsersSpecification(
                        email: request.Email,
                        containedWord: request.ContainedWord
                    )
                )
                .Select(c => _mapper.Map<ApplicationUserDTO>(c))
                .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

            await _redisCache.SetCachedData(
                redisKey,
                pagedApplicationUsers,
                DateTimeOffset.Now.AddMinutes(5)
            );

            return pagedApplicationUsers;
        }
    }
}
