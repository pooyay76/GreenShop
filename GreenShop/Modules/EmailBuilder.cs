using System;
using System.Net;
using System.Net.Mail;
using GreenShop.Models;
using Microsoft.Extensions.Configuration;

namespace GreenShop.Modules
{
    public class EmailBuilder : IDisposable  //didn't really know much about disposing, so i used resharper's
    {                                        //default implementation

        private MailAddress senderEmail { get; set; }
        private string host { get; set; }
        private int port { get; set; }
        private string subject { get; set; }
        private string body { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private bool isBodyHtml { get; set; }
        private bool useSsl { get; set; }
        private MailMessage mailMessage { get; set; }



        public EmailBuilder(Product product, string targetEmail) //acts as a director
        {                                                          //waits for .SendEMail()
            GetMailConfig();                                       //to send the email
            TemplateGenerator(product);
            BuildMessage(targetEmail);
        }

        private void GetMailConfig() //gets options from appsettings.development
        {
            IConfigurationRoot options = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            senderEmail = new MailAddress(options["Email:Username"]);
            host = options["Email:Host"];
            port = int.Parse(options["Email:Port"]);
            username = options["Email:Username"];
            password = options["Email:Password"];
            isBodyHtml = bool.Parse(options["Email:IsBodyHtml"]);
            useSsl = bool.Parse(options["Email:UseSsl"]);
        }

        private void TemplateGenerator(Product _product) //creates the template for subject and body
        {
            subject = $"Green Shop Product ({_product.Name})";
            body = $"<h1>{_product.Name}</h1>\n<img src={_product.ImageUrl}>\n{_product.Caption}\nPrice: {_product.Price}\nIn Stock: {_product.Stock}\n\n<a href=https://localhost:5001/ShowProduct/{ _product.Id})>Link</a>";
        }

        private void BuildMessage(string _targetEmail)
        {
            mailMessage = new MailMessage
            {
                From = senderEmail,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
            };
            mailMessage.To.Add(new MailAddress(_targetEmail));
        }

        public void SendEmail()  //throws an exception upon failure
        {
            SmtpClient smtpClient = new(host)
            {
                Port = port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = useSsl
            };
            smtpClient.Send(mailMessage);
            Dispose();
        }

        public void Dispose()
        {
            mailMessage?.Dispose();
        }
    }
}
