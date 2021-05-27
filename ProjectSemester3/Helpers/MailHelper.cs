using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProjectSemester3.Helpers
{
    public class MailHelper
    {
        /*private IConfiguration configuration;
        public MailHelper(IConfiguration _configuration)
        {
            configuration = _configuration;
        }*/
        //nameFrom: The sender's name
        //form: sender's email address
        //password: sender's email password
        //nameto: recipient's name
        //to: recipient's email
        //subject: subject of email
        //body: content of email
        public bool Send(string nameFrom, string from, string password, string nameTo, string to, string subject, string body)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress fromOfMail = new MailboxAddress(nameFrom,
                from);
                message.From.Add(fromOfMail);

                MailboxAddress toOfMail = new MailboxAddress(nameTo, to);
                message.To.Add(toOfMail);

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;
                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(from, password);

                client.Send(message);
                Debug.WriteLine("Done");
                client.Disconnect(true);
                client.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }
        /*public bool Send(string from, string to, string subject, string body, string htmlAttachment)
        {
            try
            {
                var host = configuration["Gmail:Host"];
                var port = int.Parse(configuration["Gmail:Port"]);
                var username = configuration["Gmail:Username"];
                var password = configuration["Gmail:Password"];
                var enable = bool.Parse(configuration["Gmail:SMTP:starttls:enable"]);
                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };
                var mailMessage = new MailMessage(from, to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                if (!string.IsNullOrEmpty(htmlAttachment))
                {
                    using (var stream = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(htmlAttachment);
                            writer.Flush();
                            stream.Position = 0;
                            mailMessage.Attachments.Add(new Attachment(stream, "orderInfo.html", "text/html"));
                            smtpClient.Send(mailMessage);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }*/
    }
}
