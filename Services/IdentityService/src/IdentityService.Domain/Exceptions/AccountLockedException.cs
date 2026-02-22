namespace IdentityService.Domain.Exceptions;

public class AccountLockedException : Exception
{
    public AccountLockedException(string email)
        : base($"Account for '{email}' is locked out")
    {
        Email = email;
    }

    public string Email { get; }
}
