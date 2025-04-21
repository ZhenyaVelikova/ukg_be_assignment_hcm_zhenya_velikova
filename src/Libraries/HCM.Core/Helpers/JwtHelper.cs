using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HCM.Core.Helpers;

public static class JwtHelper
{
	public static string Generate(Guid userId, IEnumerable<string> roles, string secret, int expirationInSeconds)
	{
		var tokenHandler = new JsonWebTokenHandler();
		var key = Encoding.ASCII.GetBytes(secret);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Expires = DateTime.UtcNow.AddSeconds(expirationInSeconds),
			Subject = new ClaimsIdentity(new[]
			{
					new Claim("id", userId.ToString()),
					new Claim("roles", roles != null ? JsonConvert.SerializeObject(roles) : null)
				}),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return token;
	}

	public static async Task<Guid?> Validate(string? token, string secret)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			return null;
		}

		var tokenHandler = new JsonWebTokenHandler();
		var key = Encoding.ASCII.GetBytes(secret);

		try
		{
			var tokenValidation = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero
			});

			if (!(tokenValidation?.IsValid ?? false))
				return null;

			var idClaim = tokenValidation.Claims.FirstOrDefault(x => x.Key == "id");
			var isUserIdParsed = Guid.TryParse(idClaim.Value?.ToString(), out Guid userId);

			return isUserIdParsed ? userId : null;
		}
		catch
		{
			return null;
		}
	}
}

