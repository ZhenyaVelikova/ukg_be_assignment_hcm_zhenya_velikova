using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Authorization;
using HCM.Core.Constants;
using HCM.Core.Helpers;
using HCM.Persistence;
using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace HCM.BusinessLogic.Services;

public class AuthorizationService : IAuthorizationService
{
	private readonly HCMDbContext _context;
	private readonly IUserRepository _userRepository;
	private readonly IOptions<JwtOptions> _jwtOptions;
	private readonly ILogger<AuthorizationService> _logger;

	public AuthorizationService(HCMDbContext context, IUserRepository userRepository, IOptions<JwtOptions> jwtOptions, ILogger<AuthorizationService> logger)
	{
		_context = context;
		_jwtOptions = jwtOptions;
		_userRepository = userRepository;
		_logger = logger;
	}

	public async Task<TokenResponseModel> SignIn(SignInRequestModel model)
	{
		var user = await _userRepository
		.GetQueryable()
		.Include(u => u.UserRoles)  
		.ThenInclude(ur => ur.Role)
		.FirstOrDefaultAsync(u => u.UserName == model.Username);

		if (user == null || !VerifyPassword(model.Password!, user.Password))
			throw new UnauthorizedAccessException(MessageConstants.InvalidUserOrPassword);

		var accessToken = GenerateJwtToken(user);
		var refreshToken = await GenerateRefreshToken(user);

		return new TokenResponseModel
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken,
			Type = "Bearer"
		};
	}

	public async Task<TokenResponseModel> RefreshToken(RefreshTokenRequestModel model)
	{
		var token = await _context.RefreshTokens
			.Include(r => r.User)
			.FirstOrDefaultAsync(r => r.Token == model.RefreshToken && !r.IsRevoked);

		if (token == null || token.ExpiresAt < DateTime.UtcNow)
			throw new UnauthorizedAccessException(MessageConstants.InvalidOrExpiredToken);

		token.IsRevoked = true;
		await _context.SaveChangesAsync();

		var newAccessToken = GenerateJwtToken(token.User);
		var newRefreshToken = await GenerateRefreshToken(token.User);

		return new TokenResponseModel
		{
			AccessToken = newAccessToken,
			RefreshToken = newRefreshToken,
			Type = "Bearer"
		};
	}

	private string GenerateJwtToken(UserEntity user)
	{
		var opts = _jwtOptions.Value;
		var userRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim("user", user.UserName),
		};
		claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(opts.Secret));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

		var token = new JwtSecurityToken(
			issuer: opts.Issuer,
			audience: opts.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(opts.AccessTokenExpirationMinutes),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private async Task<string> GenerateRefreshToken(UserEntity user)
	{
		var refreshToken = new RefreshTokenEntity
		{
			Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
			CreatedAt = DateTime.UtcNow,
			ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpirationDays),
			UserId = user.Id
		};

		_context.RefreshTokens.Add(refreshToken);
		await _context.SaveChangesAsync();

		return refreshToken.Token;
	}

	private bool VerifyPassword(string inputPassword, string storedHash)
	{
		return PasswordHasherHelper.Verify(inputPassword, storedHash);
	}
}
