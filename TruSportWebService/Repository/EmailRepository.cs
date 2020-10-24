using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models.Shop;

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

        public async Task SendOrderConfirmation(string name, string email, Order order)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
                message.To.Add(new MailAddress(email));
                message.Subject = "Order Confirmation - " + order.OrderNumber;
                message.IsBodyHtml = true;

                string body = string.Empty;

                body = @"<html>
                    <body>
                        <p>Hi [Name].</p>
                        <br/>
                        <p>This is confirmation that we have received your order below. Details are below:</p>
                        <br/>
                        <br/>
                        <p>Once you have made payment, please reply here with a screentshot of the transafer for faster order completion.</p>
                        <br/>
                        <p>Regards,</p>
                        OnTrack Bermuda
                        <br/>
                        </ body >
                    </ html >
                    ";

                body = body.Replace("[Name]", name);
                

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
    }
}
