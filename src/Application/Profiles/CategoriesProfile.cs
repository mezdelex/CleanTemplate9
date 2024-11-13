namespace Application.Profiles;

public class CategoriesProfile : Profile
{
    public CategoriesProfile()
    {
        CreateMap<Category, CategoryDTO>();
        CreateMap<Category, PatchedCategoryEvent>();
        CreateMap<Category, PostedCategoryEvent>();
        CreateMap<PatchCategoryCommand, Category>();
        CreateMap<PostCategoryCommand, Category>();
    }
}
