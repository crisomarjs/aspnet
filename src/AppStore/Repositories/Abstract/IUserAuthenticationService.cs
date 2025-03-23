using AppStore.Models.DTO;

namespace AppStore.Repositories.Abstract;

public interface IUserAuthenticationService
{
    Task<Status> LoginAsync(LoginModel loginModel);
    Task LogoutAsync();
}