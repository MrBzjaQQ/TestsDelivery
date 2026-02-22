# Notification Service - Coding Rules

## Naming Conventions

```csharp
// Classes: PascalCase
public class EmailNotificationService : IEmailNotificationService

// Interfaces: IPascalCase
public interface IEmailNotificationService

// Methods: PascalCase
public async Task SendEmailAsync(...)

// Private fields: camelCase with _
private readonly IEmailSender _emailSender;
```

## Error Handling

```csharp
// ✅ Correct
try
{
    await _smtpClient.SendAsync(message, ct);
}
catch (SmtpException ex)
{
    _logger.LogError(ex, "Failed to send email to {To}", message.To);
    throw new EmailSendFailedException(message.To, ex);
}

// ❌ Incorrect - don't swallow exceptions
try { await SendEmailAsync(...); } catch { } // Silent failure!
```

## Template Rendering

```csharp
// ✅ Safe template rendering
var template = File.ReadAllText(templatePath);
var handlebars = HandlebarsDotNet.Handlebars.RegisterHelper("if", ...);
var result = handlebars.Compile(template)(templateData);

// ❌ Don't use string interpolation directly - XSS risk!
var html = $"<p>Hello {userInput}</p>"; // XSS vulnerability!
```

## Email Validation

```csharp
// ✅ Proper email validation
public bool IsValidEmail(string email)
{
    try
    {
        var addr = new MailAddress(email);
        return addr.Host.Contains(".") && addr.Host.Length > 2;
    }
    catch
    {
        return false;
    }
}

// ❌ Don't use regex for email validation - too complex!
public bool IsValidEmail(string email) => Regex.IsMatch(email, "^[a-zA-Z0-9]+@[a-zA-Z0-9]+\\.[a-zA-Z]{2,}$");
```

## Commit Messages

```
feat(notification): add email template rendering
fix(smtp): handle connection timeout
docs(api): update notification API documentation
test(service): add unit tests for EmailNotificationService
refactor(template): optimize template rendering
```