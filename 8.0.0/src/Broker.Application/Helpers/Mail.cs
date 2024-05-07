using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Helpers
{
    public class MailerException : System.Exception
    {

        public MailerException()
            : base() { }

        public MailerException(string message)
            : base(message) { }

        public MailerException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public MailerException(string message, Exception innerException)
            : base(message, innerException) { }

        public MailerException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected MailerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    public class MailData
    {
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }

    public static class Mailer
    {
        public static void SendMail(MailMessage mail, MailData data)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = data.Host;
                smtpClient.Port = data.Port;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(data.Sender, data.Password);
                smtpClient.Timeout = 200000000;
                mail.From = new MailAddress(data.Sender);
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw new MailerException(ex.Message, ex.InnerException);
            }
        }

        public static void SendMail(MailMessage mail, MailData data, string body)
        {
            try
            {
                mail.IsBodyHtml = true;
                mail.Body = GetMailBody(body);
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = data.Host;
                smtpClient.Port = data.Port;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(data.Sender, data.Password);
                smtpClient.Timeout = 200000000;
                mail.From = new MailAddress(data.Sender);
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw new MailerException(ex.Message, ex.InnerException);
            }
        }

        public static string GetMailBody(string body)
        {
            StringBuilder mailBody = new StringBuilder(body);

            return mailBody.ToString();
        }

        public static void SendMailToCompany(MailMessage mail, MailData data, string header, string message, string footer, string appText)
        {
            mail.IsBodyHtml = true;
            mail.SubjectEncoding = Encoding.UTF8;
            var mailBuilder = new StringBuilder();
            mailBuilder.Append(
@"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>{TEXT_HTML_TITLE}</title>
    <style>
        body {
            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <h3>{TEXT_HEADER}</h3>
    <br /><br />
    <p>{Message}</p>
    <br /><br />
    <p>{Thank_Text}</p>
    <p><a href=""https://autoclick.ae/"">{App_Name}</a></p>
</body>
</html>");

            mailBuilder.Replace("{TEXT_HTML_TITLE}", mail.Subject);
            mailBuilder.Replace("{TEXT_HEADER}", header);
            mailBuilder.Replace("{Message}", message);
            mailBuilder.Replace("{Thank_Text}", footer);
            mailBuilder.Replace("{App_Name}", appText);

            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = data.Host;
            smtpClient.Port = Convert.ToInt32(data.Port);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(data.Sender, data.Password);
            smtpClient.Timeout = 200000000;
            mail.From = new MailAddress(data.Sender);
            smtpClient.Send(mail);
        }

        public static void SendRequestEmail(MailMessage mail, string Sender, string Host, string Port, string Password)
        {
            try
            {

                Sender = string.IsNullOrEmpty(Sender) ? "eslam.hamam@al7osamcompany.com" : Sender;
                Host = string.IsNullOrEmpty(Host) ? "smtpout.secureserver.net" : Host;
                Port = string.IsNullOrEmpty(Port) ? "80" : Port;
                Password = string.IsNullOrEmpty(Password) ? "123456" : Password;



                mail.IsBodyHtml = true;
                mail.SubjectEncoding = Encoding.UTF8;
                var mailBuilder = new StringBuilder();
                mailBuilder.Append(
    @"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>{TEXT_HTML_TITLE}</title>
    <style>
        body {
            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <h3>{TEXT_HEADER}</h3>
    <p>{TEXT_DESCRIPTION}</p>
    <p>&nbsp;</p>
    <p><a href=""http://IdealProten.com.com"">IdealProten</a></p>
</body>
</html>");

                mailBuilder.Replace("{TEXT_HTML_TITLE}", mail.Subject);
                mailBuilder.Replace("{TEXT_HEADER}", mail.Subject);
                mailBuilder.Replace("{TEXT_DESCRIPTION}", mail.Body);

                mail.Body = mailBuilder.ToString();
                mail.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = Host;
                smtpClient.Port = Convert.ToInt32(Port);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(Sender, Password);
                smtpClient.Timeout = 200000000;
                mail.From = new MailAddress(Sender);
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void SendRequestEmail(MailMessage mail, string Sender, string Host, string Port, string Password, string EnableSsl)
        {
            try
            {
                Sender = (Sender == "" || Sender == "admin@bahr.com" || Sender == "info@al7osamcompany.com") ? "eslam.hamam@al7osamcompany.com" : Sender;
                Host = Host == "" ? "al7osamcompany.com" : Host;
                Port = Port == "" ? "587" : Port;
                Password = Password == "" ? "123456" : Password;

                mail.IsBodyHtml = true;
                mail.SubjectEncoding = Encoding.UTF8;
                var mailBuilder = new StringBuilder();
                mailBuilder.Append(
    @"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>{TEXT_HTML_TITLE}</title>
    <style>
        body {
            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <h3>{TEXT_HEADER}</h3>
    <p>{TEXT_DESCRIPTION}</p>
    <p>&nbsp;</p>
    <p><a href=""http://bahr.mict.pro/"">Bahr Company</a></p>
</body>
</html>");

                mailBuilder.Replace("{TEXT_HTML_TITLE}", mail.Subject);
                mailBuilder.Replace("{TEXT_HEADER}", mail.Subject);
                mailBuilder.Replace("{TEXT_DESCRIPTION}", mail.Body);

                mail.Body = mailBuilder.ToString();
                mail.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = Host;
                smtpClient.Port = Convert.ToInt32(Port);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(Sender, Password);
                smtpClient.Timeout = 200000000;
                smtpClient.EnableSsl = Convert.ToBoolean(EnableSsl);
                mail.From = new MailAddress(Sender);
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public static Task SendEmailAsync(MailMessage mail, MailData data)
        {
            try
            {
                var credentialUserName = data.Sender;
                var sentFrom = data.Sender;
                var pwd = data.Password;
                SmtpClient client = new SmtpClient();
                client.Host = data.Host;
                client.Port = data.Port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = data.UseDefaultCredentials;
                NetworkCredential credentials = new NetworkCredential(credentialUserName, pwd);
                client.EnableSsl = data.EnableSsl;
                client.Credentials = credentials;
                return client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Task SendConfirmationBankMailAsync(MailMessage mail, MailData data)
        {
            try
            {
                var credentialUserName = data.Sender;
                var sentFrom = data.Sender;
                var pwd = data.Password;
                SmtpClient client = new SmtpClient();
                client.Host = data.Host;
                client.Port = data.Port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = data.UseDefaultCredentials;
                NetworkCredential credentials = new NetworkCredential(credentialUserName, pwd);
                client.EnableSsl = data.EnableSsl;
                client.Credentials = credentials;
                return client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}