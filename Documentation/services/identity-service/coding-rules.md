# Identity Service - Coding Rules

## Naming Conventions

```csharp
// Classes: PascalCase
public class AuthService : IAuthService

// Interfaces: IPascalCase
public interface IAuthService

// Methods: PascalCase
public async Task<AuthResponse> LoginAsync(...)

// Private fields: camelCase with _
private readonly UserManager<ApplicationUser> _userManager;
```

## Password Hashing

```csharp
// ✅ Use ASP.NET Core Identity built-in hashing
await _userManager.HashPasswordAsync(user, password);

// ❌ Don't use custom hashing without proper security review
SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)); // Should use Identity
```

## Error Handling

```csharp
// ✅ Proper exception handling
try
{
    var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: true);
    
    if (result.Succeeded)
    {
        return await GenerateTokensAsync(user);
    }
    
    if (result.IsLockedOut)
    {
        throw new AccountLockedException(user.Email);
    }
}
catch (Exception ex)
{
    _logger.LogError(ex, "Login failed for {Email}", email);
    throw new InvalidCredentialsException();
}

// ❌ Don't expose internal exceptions
catch (Exception ex) { throw ex; } // Loses stack trace!
```

## Email Validation

```csharp
// ✅ Use built-in validation
public async Task<bool> IsEmailValidAsync(string email)
{
    var user = await _userManager.FindByEmailAsync(email);
    return user == null; // Email not taken
}

// ❌ Don't use regex for email validation
public bool IsValidEmail(string email) => Regex.IsMatch(email, "^[a-zA-Z0-9]+@[a-zA-Z0-9]+\\.[a-zA-Z]{2,}$");
```

## JWT Security

```csharp
// ✅ Secure token configuration
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = _jwtSettings.Issuer,
    ValidAudience = _jwtSettings.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
    ClockSkew = TimeSpan.Zero  // Important: zero clock skew
};

// ❌ Don't use default clock skew (can cause tokens to be valid earlier than expected)
ClockSkew = TimeSpan.Default  // Wrong!
```

## Commit Messages

```
feat(auth): add password reset functionality
fix(jwt): fix token validation clock skew
docs(api): update auth API documentation
test(service): add unit tests for TokenService
refactor(identity): optimize user lookup
```