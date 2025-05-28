using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Services.UserService;
using Services.Services.UserService.Dto;
using SmartTrackingTransport.Extensions.ExceptionsHandler;
using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Twilio.Jwt.AccessToken;


namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		
		private readonly IUserService _userService;
		private readonly UserManager<AppUser> _userManager;
		private readonly Services.Services.IEmailService.IEmailService _emailService;

		public AccountController(IUserService userService , UserManager<AppUser> userManager,Services.Services.IEmailService.IEmailService emailService)
		{
			_userService = userService;
			_userManager = userManager;
			_emailService = emailService;
		}
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			var user = await _userService.Register(registerDto);
			if (user == null)
				return BadRequest(new CustomApiException(400, "Email Already Exist"));
			var appUser = await _userManager.FindByEmailAsync(user.Email);
			if (appUser == null) return BadRequest("User registration failed");
			return Ok(user);
		}
		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> LogIn(LogInDto logInDto)
		{
			var user = await _userService.LogIn(logInDto);
			if (user == null)
				return Unauthorized(new CustomApiException(401, "you are not Authorized!!"));
			return Ok(user);
		}
		[HttpGet("getCurrentUser")]
		[Authorize]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var user = await _userService.GetCurrentUser();
			if (user == null)
				return Unauthorized("User not found or token is invalid.");

			return Ok(user);
		}
		[HttpPut("UpdatecurrentUser")]
		[Authorize]
		public async Task<ActionResult<UserDto>> UpdateCurrentUser([FromQuery] string? displayName, [FromQuery] string? email)
		{
			var result = await _userService.UpdateCurrentUser(displayName, email);
			if (result == null)
				return BadRequest("Failed to update user");
			return Ok(result);
		}
		[HttpPost("reset-password-request")]
		public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null) return NotFound("User not found");

			// Generate a password reset token
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);



			// Create email content
			var resetLink = $"http://smarttrackingapp.runasp.net/redirect-to-app?email={dto.Email}&code={Uri.EscapeDataString(token)}";
			var emailBody = $@"
        <p>Hello,</p>
        <p>You requested a password reset. Click the link below to reset your password:</p>
        <a href='{resetLink}'>Reset Password</a>
        <p>If you did not request this, please ignore this email.</p>
    ";

			await _emailService.SendEmailAsync(dto.Email, "Password Reset Request", emailBody);

			return Ok("Password reset email sent");
		}
		[HttpGet("redirect-to-app")]
		public IActionResult RedirectToApp([FromQuery] string email, [FromQuery] string code)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
			{
				return BadRequest("Missing email or code");
			}

			var customUrl = $"myapp://reset?email={email}&code={Uri.EscapeDataString(code)}";

			return Redirect(customUrl);
		}
		/*
		[HttpGet("reset-password")]
		public IActionResult ResetPassword([FromQuery] string email, [FromQuery] string code)
		{
			var user = _userManager.FindByEmailAsync(email).Result;
			if (user == null)
			{
				return NotFound("User not found");
			}

			var isValidToken = _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", code).Result;
			if (!isValidToken)
			{
				return BadRequest("Invalid or expired token");
			}

			// Token is valid, send a response to navigate to reset password page in the app
			var resetUrl = $"http://smarttrackingapp.runasp.net/reset-password?email={email}&code={Uri.EscapeDataString(code)}";

			return Ok(new { message = "Token is valid, please proceed with setting a new password", resetUrl });
		}
		*/
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null) return NotFound("User not found");

			// Reset the password
			var result = await _userManager.ResetPasswordAsync(user, dto.Code, dto.NewPassword);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description);
				return BadRequest(new { Errors = errors });
			}

			return Ok("Password reset successfully");
		}
		[HttpPost("google-login")]
		public async Task<ActionResult<UserDto>> GoogleLogin([FromBody] string idToken)
		{
			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

			var user = await _userManager.FindByEmailAsync(payload.Email);
			if (user == null)
			{
				user = new AppUser
				{
					UserName = payload.Email,
					Email = payload.Email,
					DisplayName = payload.Name
				};

				var result = await _userManager.CreateAsync(user);
				if (!result.Succeeded)
					return BadRequest("User creation failed");
			}

			var userDto = await _userService.CreateToken(user);
			return Ok(userDto);
		}
		

	}
}