using Azure;
using Azure.Communication.Email;
using static System.Net.WebRequestMethods;

namespace LeafLINQWebAPI.Services;

public class EmailService
{
    private readonly EmailClient _emailClient;

    public EmailService(string connectionString)
    {
        _emailClient = new EmailClient(connectionString);
    }

    public async Task SendUserLoginInfo(string email, string password)
    {

        var subject = "Welcome LeafLINQ";
        var htmlContent = $"<html>" +
            $"<body>" +
            $"<h1>Welcome To LeafLINQ!!</h1><br/>" +
            $"<p>Please Find Below your new login information</p></br>" +
            $"<p>Login Email: {email}</p>" +
            $"<p>Password: {password}</p>" +
            $"<p>https://leaflinq.azurewebsites.net/Account/Login</p>" +
            $"<p>It is recommended that you change your password on first login.</p>" +
            $"<p>Kind Regards,</p></br>" +
            $"<p>Your Admin Team</p>" +
            $"</body>" +
            $"</html>";
        var sender = "donotreply@9b12558b-8d6c-473d-9a9e-e9f4bc8f5385.azurecomm.net";
        var recipient = email;

        try
        {
            Console.WriteLine("Sending email...");
            EmailSendOperation emailSendOperation = await _emailClient.SendAsync(
                Azure.WaitUntil.Started,
                sender,
                recipient,
                subject,
                htmlContent);
        }
        catch (RequestFailedException ex)
        {
            /// OperationID is contained in the exception message and can be used for troubleshooting purposes
            Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
        }
    }

    public async Task RemoveUserEmail(string email)
    {

        var subject = "Account Deletion";
        var htmlContent = $"<html>" +
            $"<body>" +
            $"<h1>Your Account has been deleted</h1><br/>" +
            $"<p>An Admin has deleted your LeafLINQ account. Please contact a LeafLINQ admin to have it restored.</p>" +
            $"</body>" +
            $"</html>";
        var sender = "donotreply@9b12558b-8d6c-473d-9a9e-e9f4bc8f5385.azurecomm.net";
        var recipient = email;

        try
        {
            Console.WriteLine("Sending email...");
            EmailSendOperation emailSendOperation = await _emailClient.SendAsync(
                Azure.WaitUntil.Started,
                sender,
                recipient,
                subject,
                htmlContent);
        }
        catch (RequestFailedException ex)
        {
            /// OperationID is contained in the exception message and can be used for troubleshooting purposes
            Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
        }
    }
}
