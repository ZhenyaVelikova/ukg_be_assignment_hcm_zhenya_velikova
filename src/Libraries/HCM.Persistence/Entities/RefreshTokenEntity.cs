namespace HCM.Persistence.Entities;
public class RefreshTokenEntity
{
	public int Id { get; set; }
	public string Token { get; set; } = null!;
	public DateTime ExpiresAt { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime CreatedAt { get; set; }

	public Guid UserId { get; set; }
	public UserEntity User { get; set; } = null!;
}