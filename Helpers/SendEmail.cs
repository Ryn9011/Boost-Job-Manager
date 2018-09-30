using System.Net.Mail;
using System.Text;

namespace JobTracker.API.Helpers
{
    public class SendEmail
    {

 public string SendEmails(string toAddress, string subject, string body)
   {
     string result = "Message Sent Successfully..!!";
     string senderID = "millie.crossman@boostpromotions.co.nz";
     const string senderPassword = "mc098)(*"; 

       SmtpClient smtp = new SmtpClient
       {
         Host = "mail.micloud.net.nz", // smtp server address here…
         Port = 587,
         // EnableSsl = true,
         DeliveryMethod = SmtpDeliveryMethod.Network,
         Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
         Timeout = 30000,
       };
       MailMessage message = new MailMessage(senderID, toAddress, subject, body);
       smtp.Send(message);

     return result;
   }

        }
    }
