using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services;

public class EmailService : IEmailService
{
    public void Send(string to, string from, string subject, string message)
    {
        MailMessage mailMessage = new MailMessage(from, to);

        mailMessage.Subject = subject;
        mailMessage.Body = message;

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new System.Net.NetworkCredential("s.pagacz123@gmail.com", "placeholder");

        smtpClient.Send(mailMessage);
    }
}
