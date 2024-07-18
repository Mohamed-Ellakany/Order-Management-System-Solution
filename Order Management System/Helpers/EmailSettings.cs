using Microsoft.AspNetCore.Identity;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Identity;
using Order_Management_System.Core.Entities.Order_Aggregate;
using System.Net;
using System.Net.Mail;

namespace Order_Management_System.Helpers
{
    public static class EmailSettings
    {
        

        public static  void SendEmail(Email email)
        {

            var Client = new SmtpClient("smtp.gmail.com" , 587);
          
            Client.EnableSsl = true ;

            Client.Credentials = new NetworkCredential("esamm612@gmail.com", "xuvlegrulkfcdeti");

            Client.Send("esamm612@gmail.com" , email.To , email.Subject , email.Body);

           


        }

    }
}
