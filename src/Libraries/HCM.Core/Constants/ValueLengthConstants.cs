namespace HCM.Core.Constants;

public static class ValueLengthConstants
{
	public static class User
	{
		public const int UsernameMaxLength = 256;
		public const int RandomPasswordLength = 64;
		public const int PasswordMinLength = 8;
	}
	public static class Role
	{
		public const int NameMaxLength = 128;
		public const int DisplayNameMaxLength = 256;
	}

	public static class Department
	{
		public const int NameMaxLength = 100;
	}

	public static class Position
	{
		public const int NameMaxLength = 100;
	}

	public static class Person
	{
		public const int FirstNameMaxLength = 128;
		public const int LastNameMaxLength = 128;
		public const int EmailMaxLength = 255;
	}
}
