using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;

namespace OnTrackWebService.Repository
{
    public class EmailRepository : IEmailRepository<string>
    {
        OnTrackContext _context;

        public EmailRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task SendSignUpEmail(string name, string email, string userRole, string userTeam)
        {
            try
            {
                MailMessage message = new MailMessage();

                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                //SmtpServer.EnableSsl = true;
                //SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("ontrackbda@gmail.com", "BermudaOnTrack2019");
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //        System.Security.Cryptography.X509Certificates.X509Chain chain,
                //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};

                message.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
                message.To.Add(new MailAddress(Constants.BCCAddress));
                message.Subject = "New Sign Up";
                message.IsBodyHtml = true;

                string body = string.Empty;

                if (userTeam != null)
                {
                    body = @"<html>
                        <body>
                            <p>There has been a new sign up by [Name].</p>
                            <br/>
                            <p>They have requested a [UserRole] role for [Team].</p>
                            <br/>
                            <br/>
                            <p>You may reply to [Name] at [Email].</p>
                           <br/>
                           <p>Regards,</p>
                            OnTrack Bermuda
                            <br/>
                         </ body >
                        </ html >
                     ";

                    body = body.Replace("[UserRole]", userRole);
                    body = body.Replace("[Team]", userTeam);
                    body = body.Replace("[Name]", name);
                    body = body.Replace("[Email]", email);
                }
                else
                {
                    body = @"<html>
                        <body>
                            <p>There has been a new sign up by [Name].</p>
                            <br/>
                            <p>They have requested a [UserRole] role.</p>
                            <br/>
                            <br/>
                            <p>You may reply to [Name] at [Email].</p>
                           <br/>
                           <p>Regards,</p>
                            OnTrack Bermuda
                            <br/>
                         </ body >
                        </ html >
                     ";

                    body = body.Replace("[UserRole]", userRole);
                    body = body.Replace("[Name]", name);
                    body = body.Replace("[Email]", email);
                }

                message.Body = body;

                try
                {
                    //SmtpServer.Send(message);

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("ontrackbda@gmail.com", "BermudaOnTrack2019");
                        smtp.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Email");
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }
        }

        public async Task SendPasswordResetEmail(string name, string email, string temporaryPassword)
        {
            try
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
                message.To.Add(new MailAddress(email));
                message.Bcc.Add(new MailAddress(Constants.BCCAddress));
                message.Subject = "Password Reset Request";
                message.IsBodyHtml = true;

                string body = string.Empty;

                body = @"<html>
                        <body>
                            <p>Hi [FirstName],</p>
                            <p>You have requested to reset your password.</p>
                            <p>Your temporary password is: [TemporaryPassword]</p>
                            <p>If you have any questions please contact us at [Email].</p>
                           <br/>
                           <p>Regards,</p>
                            OnTrack Bermuda
                            <br/>
                         </ body >
                        </ html >
                     ";

                body = body.Replace("[FirstName]", name);
                body = body.Replace("[TemporaryPassword]", temporaryPassword);
                body = body.Replace("[Email]", Constants.BCCAddress);
                
                message.Body = body;

                try
                {
                    //SmtpServer.Send(message);

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("ontrackbda@gmail.com", "BermudaOnTrack2019");
                        smtp.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Email");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }
        }

        //public async Task SendWelcomeEmail(string To, string Subject, string htmlString)
        //{
        //    try
        //    {
        //        MailMessage message = new MailMessage();

        //        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("ontrackbda@gmail.com", "BermudaOnTrack2019");

        //        message.From = new MailAddress(Constants.NoReplyFromAddress);
        //        message.To.Add(new MailAddress(Constants.BCCAddress));
        //        message.Subject = "Welcome to OnTrack";
        //        message.IsBodyHtml = true;

        //        string body = string.Empty;

        //        if (userTeam != null)
        //        {
        //            body = @"<html>
        //                <body>
        //                    <p>There has been a new sign up by [Name].</p>
        //                    <br/>
        //                    <p>They have requested a [UserRole] role for [Team].</p>
        //                    <br/>
        //                    <br/>
        //                    <p>You may reply to [Name] at [Email].</p>
        //                   <br/>
        //                   <p>Regards,</p>
        //                    OnTrack Bermuda
        //                    <br/>
        //                 </ body >
        //                </ html >
        //             ";

        //            body = body.Replace("[UserRole]", userRole);
        //            body = body.Replace("[Team]", userTeam);
        //            body = body.Replace("[Name]", name);
        //            body = body.Replace("[Email]", email);
        //        }
        //        else
        //        {
        //            body = @"<html>
        //                <body>
        //                    <p>There has been a new sign up by [Name].</p>
        //                    <br/>
        //                    <p>They have requested a [UserRole] role.</p>
        //                    <br/>
        //                    <br/>
        //                    <p>You may reply to [Name] at [Email].</p>
        //                   <br/>
        //                   <p>Regards,</p>
        //                    OnTrack Bermuda
        //                    <br/>
        //                 </ body >
        //                </ html >
        //             ";

        //            body = body.Replace("[UserRole]", userRole);
        //            body = body.Replace("[Name]", name);
        //            body = body.Replace("[Email]", email);
        //        }

        //        message.Body = body;

        //        try
        //        {
        //            SmtpServer.Send(message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine(ex.Message, "Email");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Email");
        //    }
        //}
    }
}
