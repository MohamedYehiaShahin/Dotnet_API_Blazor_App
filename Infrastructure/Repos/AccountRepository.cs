using Application.Contracts;
using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Domain.Entity.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Mapster;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Application.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class AccountRepository
        (RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, 
        IConfiguration config, SignInManager<ApplicationUser> signInManager,AppDbContext context) : IAccount
    {
        private async Task<ApplicationUser> FindUserByEmailAsync(string email)
            => await userManager.FindByEmailAsync(email);
        private async Task<IdentityRole> FindRoleByNameAsync(string roleName)
            => await roleManager.FindByNameAsync(roleName);
        private static string GenerateRefreshToken()=>Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role,(await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("Fullname",user.Name),
                };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience:config["Jwt:Audience"],
                    claims:userClaims,
                    expires:DateTime.Now.AddMinutes(20),
                    signingCredentials:credentials
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return "Unable To generate Token";
            }
        }
        private async Task<GeneralResponse> AssignUserToRole(ApplicationUser user,IdentityRole role)
        {
            if(user is null || role is null) return new GeneralResponse(false,"Model state cannot be empty");

            if (await FindRoleByNameAsync(role.Name) == null)
                await CreateRoleAsync(role.Adapt(new CreateRoleDTO()));

            IdentityResult result = await userManager.AddToRoleAsync(user,role.Name);
            string error = checkResponse(result);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false,error);
            else
                return new GeneralResponse(true,$"{user.Name} assigned to {role.Name} role");
        }
        private string checkResponse(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                OperatingSystem os = Environment.OSVersion;
                PlatformID platformID = os.Platform;
                switch (platformID)
                {
                    case PlatformID.Unix:
                        return string.Join("\n", errors);
                        break;
                }
                return string.Join(Environment.NewLine, errors);
            }
            return null!;
        }

        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            if (await FindRoleByNameAsync(model.RoleName) is null) return new GeneralResponse(false, "Role not found");
            if (await FindUserByEmailAsync(model.UserEmail) is null) return new GeneralResponse(false, "User not found");

            var user = await FindUserByEmailAsync(model.UserEmail);
            var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);
            var error = checkResponse(removeOldRole);
            if(!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, error);

            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            var response = checkResponse(result);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, response);
            else 
                return new GeneralResponse(true, "Role Changed");
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if(await FindUserByEmailAsync(model.EmailAddress)  != null)
                    return new GeneralResponse(false,"Sorry, user is already created");
                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    PasswordHash = model.Password
                };
                var result = await userManager.CreateAsync(user,model.Password);
                string error = checkResponse(result);
                if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, error);

                var(flag,message) = await AssignUserToRole(user,new IdentityRole() { Name = model.Role });
                return new GeneralResponse(flag, message);
            }
            catch (Exception ex) 
            {
                return new GeneralResponse(false,ex.Message);
            }
        }

        public async Task<GeneralResponse> RemoveAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if (await FindUserByEmailAsync(model.EmailAddress) == null)

                    return new GeneralResponse(false, "Sorry, this user already does not exist");
                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    PasswordHash = model.Password
                };
                var result = await userManager.DeleteAsync(user);
                string error = checkResponse(result);
                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse(false, error);

                var (flag, message) = await AssignUserToRole(user, new IdentityRole() { Name = model.Role });
                return new GeneralResponse(flag, message);
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, ex.Message);
            }
        }

        public async Task CreateAdmin()
        {
            try
            {
                if((await FindRoleByNameAsync(Constant.Role.Admin)) != null) return;
                var admin = new CreateAccountDTO()
                {
                    Name = "Admin",
                    Password = "Admin@123",
                    EmailAddress = "admin@admin.com",
                    Role = Constant.Role.Admin,
                };
                await CreateAccountAsync(admin);
            }
            catch (Exception e) { }
        }

        public async Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model)
        {
            try
            {
                if((await FindRoleByNameAsync(model.Name)) == null)
                {
                    var response = await roleManager.CreateAsync(new IdentityRole(model.Name));
                    var error = checkResponse(response);
                    if(!string.IsNullOrEmpty(error)) 
                        throw new Exception(error);
                    else
                        return new GeneralResponse(true,$"{model.Name} created");
                }
                return new GeneralResponse(false,$"{model.Name} already created");
            }catch (Exception ex) {throw new Exception(ex.Message); }
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
        =>(await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();

        public async Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync()
        {
            var allusers = await userManager.Users.ToListAsync();
            if (allusers is null) return null;

            var List = new List<GetUsersWithRolesResponseDTO>();
            foreach (var user in allusers) 
            {
                var getUserRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(r=>r.Name.ToLower() == getUserRole.ToLower());
                List.Add(new GetUsersWithRolesResponseDTO()
                {
                    Name = user.Name,
                    Email = user.Email,
                    RoleId = getRoleInfo?.Id,
                    RoleName = getRoleInfo.Name
                });
            }
            return List;
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.EmailAddress);
                if (user is null)
                    return new LoginResponse(false, "User not found");

                SignInResult result;
                try
                {
                    result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch
                {
                    return new LoginResponse(false, "Invalid credentials");
                }
                if (!result.Succeeded)
                    return new LoginResponse(false, "Invalid credentials");

                string jwtToken = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();
                if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                    return new LoginResponse(false, "error occured while logging in account, Please contact the administrator");
                else
                    return new LoginResponse(true, $"{user.Name} successfully logged in", jwtToken, refreshToken);
            }
            catch (Exception ex)
            {
                return new LoginResponse(false , ex.Message);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            var token = await context.RefreshTokens.FirstOrDefaultAsync(t=>t.Token == model.Token);
            if (token == null) return new LoginResponse();
            var user = await userManager.FindByIdAsync(token.UserID);
            string newToken = await GenerateToken(user);
            string newRefreshToken = GenerateRefreshToken();
            var saveResult = await SaveRefreshToken(user.Id, newRefreshToken);
            if(saveResult.Flag) 
                return new LoginResponse(true,$"{user.Name} successfully relogged in",newToken,newRefreshToken);
            else
                return new LoginResponse();
        }

        private async Task<GeneralResponse> SaveRefreshToken(string userId, string token)
        {
            try
            {
                var user = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserID == userId);
                if (user == null)
                    context.RefreshTokens.Add(new RefreshToken() { UserID = userId, Token = token });
                else
                    user.Token = token;

                await context.SaveChangesAsync();
                return new GeneralResponse(true, null!);
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false , ex.Message);
            }
            
        }
    }
}
