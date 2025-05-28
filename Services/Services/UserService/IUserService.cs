using Infrastucture.IdentityEntities;
using Services.Services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService
{
	public interface IUserService
	{
		Task<UserDto> Register(RegisterDto registerDto);
		Task<UserDto> LogIn(LogInDto logInDto);

		Task<UserDto> CreateToken(AppUser appUser);
		Task<UserDto> GetCurrentUser();
		Task<UserDto> UpdateCurrentUser(string? displayName, string? email);
		Task<bool> AddUserToRole(string email, string roleName);
		Task<bool> RemoveUserFromRole(string email, string roleName);
		Task<IList<string>> GetUserRoles(string email);
		Task<bool> IsInRole(string email, string roleName);
		Task<bool> AssignDefaultRole(AppUser user);
	}
}
