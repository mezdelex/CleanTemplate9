namespace Application.Profiles;

public class ApplicationUsersProfile : Profile
{
    public ApplicationUsersProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDTO>();
    }
}
