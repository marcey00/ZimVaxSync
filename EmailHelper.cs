using System.Net;
using System.Net.Mail;

public static class EmailHelper
{
    public static void SendVerificationEmail(string toEmail, string code)
    {
        string fromEmail = "muhuromarceline@gmail.com"; // Replace with your Gmail address
        string appPassword = "ocdc mqmu pdwc mcih"; // Replace with your App Password

        MailMessage message = new MailMessage(fromEmail, toEmail);
        message.Subject = "ZimVaxSync Verification Code";
        message.Body = $"Your verification code is: {code}";
        message.IsBodyHtml = false;

        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
        smtp.EnableSsl = true;

        smtp.Send(message);
    }
}
