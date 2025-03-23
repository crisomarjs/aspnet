using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;

namespace AppStore.Repositories.Implementation;

public class UserAuthenticationService : IUserAuthenticationService
{

    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public UserAuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<Status> LoginAsync(LoginModel login)
    {
        var status = new Status();
        var user = await userManager.FindByNameAsync(login.UserName!);

        if(user == null)
        {
            status.StatusCode = 0;
            status.Message = "User not found";
            return status;
        }

        if(!await userManager.CheckPasswordAsync(user, login.Password!))
        {
            status.StatusCode = 0;
            status.Message = "Invalid password";
            return status;
        }

        var res = await signInManager.PasswordSignInAsync(user, login.Password!, true, false);
        if(!res.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "Login failed";
        } 

        status.StatusCode = 1;
        status.Message = "Login successful";
        return status;
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}