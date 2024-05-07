using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Abstract;
using System.Security.Claims;

namespace MovieStoreMvc.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthenticationService(RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager )
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;       
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
           var status = new Status();
            var user = await _userManager.FindByNameAsync( model.UserName );
            if(user== null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid Username";
                return status;
            }
            // We will mathc our password
            if(!await _userManager.CheckPasswordAsync(user , model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var signinResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signinResult.Succeeded)
            {
                // Add Role
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name)
                };
                foreach (var userRole in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in successfully";
                return status;
            }
            else if (signinResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on Loggin in";
                return status;
            }
        }




        public async Task<Status> LogoutAsync()
        {
             
            await _signInManager.SignOutAsync();

            return null;
        }




        public async Task<Status> RegistrationAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if(userExist != null)
            {
                status.StatusCode = 0;
                status.Message = "User Already Exist";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = true
            };
            var result = await  _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                status.StatusCode= 0;
                status.Message = "User Creation Failed";
                return status;
            }

            // Role Managment
            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));

            if(await _roleManager.RoleExistsAsync(model.Role))
            {
                await _userManager.AddToRoleAsync(user,model.Role);
            }

            status.StatusCode = 1;
            status.Message = "User has registered sussessfully";
            return status;
        }
    }
}
