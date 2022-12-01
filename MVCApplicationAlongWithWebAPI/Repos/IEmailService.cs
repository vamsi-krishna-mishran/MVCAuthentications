using System.Net.Mail;

namespace MVCApplicationAlongWithWebAPI.Repository
{
    public interface IEmailService
    {
        public int SendMail(string email, string heading, string body);
    }
    public class EmailService : IEmailService
    {
        public int SendMail(string email, string heading, string body)
        {
            Console.WriteLine(email);
            string mailBody = "<html>" +
                "<head>" +
                    "<style>" +
                        "h1{" +
                            "color:red;" +
                        "}" +
                    "</style>" +
                "</head>" +
                "<body>" +
                    "<h1>This is heading with color red</h1>" +
                    "<img src=\"~/Images/sunlight.jpg\"" +
                "</body>" +
                "</html>";
            Console.WriteLine(mailBody);
            try
            {
                MailMessage newMail = new MailMessage();

                SmtpClient client = new SmtpClient("smtp.gmail.com");


                newMail.From = new MailAddress("N170470@rguktn.ac.in", "vamsikrishna");

                newMail.To.Add(email);// declare the email subject

                newMail.Subject = heading; // use HTML for the email body


                newMail.Body = body;

                newMail.IsBodyHtml = true; //newMail.Body = mailBody;


                // enable SSL for encryption across channels
                client.EnableSsl = true;
                // Port 465 for SSL communication
                client.Port = 587;
                //client.UseDefaultCredentials = true;
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                client.Credentials = new System.Net.NetworkCredential("N170470@rguktn.ac.in", "Vickyvamsikrishna84");

                client.Send(newMail); // Send the constructed mail
                Console.WriteLine("Email Sent");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -" + ex);
                return -1;
            }

        }
    }
}

