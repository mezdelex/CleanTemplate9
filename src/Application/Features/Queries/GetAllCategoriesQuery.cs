namespace Application.Features.Queries;

public sealed record GetAllCategoriesQuery : BaseRequest, IRequest<PagedList<CategoryDTO>>
{
    public string? Name { get; set; }
    public string? ContainedWord { get; set; }

    public sealed class GetAllCategoriesQueryHandler(
        ICategoriesRepository repository,
        IMapper mapper,
        IRedisCache redisCache
    ) : IRequestHandler<GetAllCategoriesQuery, PagedList<CategoryDTO>>
    {
        private readonly ICategoriesRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IRedisCache _redisCache = redisCache;

        public async Task<PagedList<CategoryDTO>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            var redisKey =
                $"{nameof(GetAllCategoriesQuery)}#{request.Name}#{request.ContainedWord}#{request.Page}#{request.PageSize}";
            var cachedPagedCategories = await _redisCache.GetCachedData<PagedList<CategoryDTO>>(
                redisKey
            );
            if (cachedPagedCategories != null)
                return cachedPagedCategories;

            var pagedCategories = await _repository
                .ApplySpecification(
                    new CategoriesSpecification(
                        name: request.Name,
                        containedWord: request.ContainedWord
                    )
                )
                .Select(c => _mapper.Map<CategoryDTO>(c))
                .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

            await _redisCache.SetCachedData(
                redisKey,
                pagedCategories,
                DateTimeOffset.Now.AddMinutes(5)
            );

            return pagedCategories;
        }
    }
}
