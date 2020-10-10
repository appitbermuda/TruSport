using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
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
                List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
                smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
                message.To.Add(new MailAddress(Constants.BCCAddress));
                message.Subject = "New Sign Up";
                message.IsBodyHtml = true;

                string body = string.Empty;

                if (userTeam != null)
                {
                    body = @"<html>
                        <head>
                        <style type='text/css'>
                            .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                        </style>
                        </head>
                        <body>
                        <center>
                            <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>New User Sign Up!</h1>

                        <p>[Name] has just signed up with a [Role] role for [Team].</p>

                        <p>If you have any issues with thier account, please reply to them at [Email]. Otherwise, click below to validate their account.</p>

                        <p><a href='[URL]/user/validate?email=[Email]'>Validate [Name]</a></p>

                        <br>
                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK Bermuda</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                        </center>
                         </ body >
                        </ html >
                     ";

                    body = body.Replace("[Role]", userRole);
                    body = body.Replace("[Team]", userTeam);
                    body = body.Replace("[Name]", name);
                    body = body.Replace("[URL]", Constants.OnTrackEndpoint);
                    body = body.Replace("[Email]", email);
                }
                else
                {
                    body = @"<html>
                        <head>
                        <style type='text/css'>
                            .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                        </style>
                        </head>
                        <body>
                        <center>
                            <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>New User Sign Up!</h1>

                        <p>[Name] has just signed up with a [Role] role.</p>

                        <p>If you have any issues with thier account, please reply to them at [Email]. Otherwise, click below to validate their account.</p>

                        <p><a href='[URL]/user/validate?email=[Email]'>Validate [FirstName]</a></p>

                        <br>
                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK Bermuda</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                        </center>
                         </ body >
                        </ html >
                     ";

                    body = body.Replace("[UserRole]", userRole);
                    body = body.Replace("[Name]", name);
                    body = body.Replace("[URL]", Constants.OnTrackEndpoint);
                    body = body.Replace("[Email]", email);
                }

                message.Body = body;

                try
                {
                    smtpClient.Send(message);
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

        public async Task SendCustomerSignUpEmail(Customer customer)
        {
            try
            {
                List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
                smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
                message.To.Add(new MailAddress(Constants.BCCAddress));
                message.Subject = "New Customer";
                message.IsBodyHtml = true;

                string body = string.Empty;

                body = @"<html>
                    <head>
                    <style type='text/css'>
                        .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                    </style>
                    </head>
                    <body>
                    <center>
                        <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                <tr>
               
                                    <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                        <div class='main'>
                    <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                        <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                    </p>

                    <h1>New Customer Sign Up!</h1>

                    <p>[Name] has just signed up for tickets.</p>

                    <p>If you have any issues with thier account, please reply to them at [Email].</p>

                    <br>
                    <br>
                    Thanks!
                    <br>

                    <strong>ONTRACK Bermuda</strong>

                    <br><br>
                    <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                    <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                        If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                    </p>
                    </div>
                    </td>
                </tr>
                </table>
                    </center>
                        </ body >
                    </ html >
                    ";

                body = body.Replace("[Name]", customer.Name);
                body = body.Replace("[URL]", Constants.OnTrackEndpoint);
                body = body.Replace("[Email]", customer.Email);
                

                message.Body = body;

                try
                {
                    smtpClient.Send(message);
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

        public async Task<bool> SendPasswordResetEmail(string name, string email, string temporaryPassword)
        {
            try
            {
                List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
                smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);


                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
                mailMessage.Bcc.Add(new MailAddress("sports@ontrackbda.com"));
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = "Password Reset Request";
                mailMessage.IsBodyHtml = true;

                string body = string.Empty;

                body = @"<html>
                <head>
                <style type='text/css'>
 
                    .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                </style>
                </head>
                <body>
                <center>
                    <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>Hi [FirstName],</h1>

                        <p>You have requested to reset your password.</p>
                            <p>Your temporary password is: [TemporaryPassword]</p>
                            <p>If you have any questions please contact us at [Email].</p>

                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                </center>
                </body>
                </html>";

                body = body.Replace("[FirstName]", name);
                body = body.Replace("[TemporaryPassword]", temporaryPassword);
                body = body.Replace("[Email]", Constants.FromAddress);

                mailMessage.Body = body;

                try
                {
                    smtpClient.Send(mailMessage);

                    return true;
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

            return false;
        }

        public async Task<bool> Welcome(string Email)
        {


            List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
            smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
            mailMessage.To.Add(new MailAddress(Email));
            mailMessage.Bcc.Add(new MailAddress("sports@ontrackbda.com"));

            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Welcome to ONTRACK!";

            string body = @"<html>
                <head>
                <style type='text/css'>
 
                    .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                </style>
                </head>
                <body>
                <center>
                    <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>Welcome to ONTRACK!</h1>

                        <p>Thank you for signing up. Your account is being reviewed, and you should hear back from us within 5 to 7 days. If we have any further queries in validating your account, we will reach out to you.</p>

                        <p>If you have any issues with your account please do not hesitate to contact us by replying to sports@ontrackbda.com</p>

                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                </center>
                </body>
                </html>";


            mailMessage.Body = body;

            try
            {
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return false;
        }

        public async Task<bool> WelcomeCustomer(string Email)
        {


            List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
            smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
            mailMessage.To.Add(new MailAddress(Email));
            mailMessage.Bcc.Add(new MailAddress("sports@ontrackbda.com"));

            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Welcome to ONTRACK!";

            string body = @"<html>
                <head>
                <style type='text/css'>
 
                    .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                </style>
                </head>
                <body>
                <center>
                    <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>Welcome to ONTRACK Tickets!</h1>

                        <p>Thank you for signing up. Please click the link below to validate your account.</p>

                        <p><a href='[URL]/customer/validate?email=[Email]'>Validate My Account</a></p>

                        <p>If you have any issues with your account please do not hesitate to contact us by replying to sports@ontrackbda.com</p>

                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                </center>
                </body>
                </html>";

            body = body.Replace("[URL]", Constants.OnTrackEndpoint);
            body = body.Replace("[Email]", Email);

            mailMessage.Body = body;

            try
            {
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return false;
        }

        public async Task<bool> UserValidated(string Email)
        {
            List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
            smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
            mailMessage.To.Add(new MailAddress(Email));
            mailMessage.Bcc.Add(new MailAddress("sports@ontrackbda.com"));

            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Account Validated!";

            string body = @"<html>
                <head>
                <style type='text/css'>
 
                    .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                </style>
                </head>
                <body>
                <center>
                    <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>Account Validated!</h1>

                        <p>Thank you for your patience. The team at ONTRACK has reviewed and accepted your request. You may now log into the app and access the features.</p>

                        <p>If you have any issues with your account please do not hesitate to contact us by replying to sports@ontrackbda.com </p>

                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                </center>
                </body>
                </html>";


            mailMessage.Body = body;

            try
            {
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return false;
        }

        public async Task SendOrderConfirmation(string name, string email, Order order)
        {
            try
            {
                List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
                smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

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
                    smtpClient.Send(message);

                    //return true;
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

        public async Task<bool> SendPaymentConfirmation(Customer customer, string authorisation, Order order, int quantity, Fixture fixture)
        {


            List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
            smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
            mailMessage.To.Add(new MailAddress(customer.Email));
            mailMessage.Bcc.Add(new MailAddress("sports@ontrackbda.com"));

            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "ONTRACK Match Ticket Confirmation";

            string body = @"<html>
                <head>
                <style type='text/css'>
 
                    .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                </style>
                </head>
                <body>
                <center>
                    <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>Hi [FirstName]</h1>

                        <p>Thank you for your purchase!</p>

                        <br/>
                        <h3>Order Summary</h3>
                        <hr/>

                        <table style='width: 100%; border:none; text-align:left;'>
                            <tr>
                             <th style='width:50%;'> Ticket </th>
                                       </tr>
                           <tr>
                             <td style='width:50%;'> [Product] </td>
                           </tr>
 
                        </table>
<br/>
                        <table style='width: 100%; border:none; text-align:left;'>
                            <tr>
                             <th style='width:50%;'> Qty </th>
                             <th style='width:50%;'> Total </th>
                           </tr>
                           <tr>
                             <td style='width:50%;'> [Quantity] </td>
                             <td style='width:50%;'> $[Total] </td>
                           </tr>
 
                        </table>
<br/>
                        <table style='width: 100%; border:none; text-align:left;'>
                            <tr>
                             <th style='width:50%;'> Authorisation </th>
                             <th style='width:50%;'> Transaction Date </th>
                           </tr>
                           <tr>
                             <td style='width:50%;'> [Authorisation] </td>
                             <td style='width:50%;'> [TrxDate] </td>
                           </tr>
 
                        </table>
<br/>

                        <p>Please plan to arrive to the match before half time.</p>

                         <p>If you have any issues with your purchase please do not hesitate to contact us by replying to sports@ontrackbda.com</p>

                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                </center>
                </body>
                </html>";

            body = body.Replace("[FirstName]", customer.FirstName);
            body = body.Replace("[Email]", customer.Email);
            body = body.Replace("[Product]", fixture.HomeTeam.Name + " v " + fixture.AwayTeam.Name);
            body = body.Replace("[Quantity]", quantity.ToString());
            body = body.Replace("[Total]", order.Total.ToString());
            body = body.Replace("[Authorisation]", authorisation);
            body = body.Replace("[TrxDate]", order.Date.ToLocalTime().ToString("MMM dd yyyy - H:mm:ss"));

            mailMessage.Body = body;

            try
            {
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return false;
        }

        public async Task<bool> SendPaymentConfirmationTest()
        {


            List<Setting> smtpSetting = await _context.Settings.Where(e => e.Key.Contains("SMTP")).ToListAsync();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);
            smtpClient.Host = smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPServer).Value;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPUsername).Value, smtpSetting.FirstOrDefault(e => e.Key == Constants.SMTPPassword).Value);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Constants.NoReplyFromAddress, "OnTrack Bermuda");
            mailMessage.To.Add(new MailAddress(Constants.BCCAddress));
            mailMessage.Bcc.Add(new MailAddress("sports@ontrackbda.com"));

            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "ONTRACK Match Ticket Confirmation";

            string body = @"<html>
                <head>
                <style type='text/css'>
 
                    .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{ line - height:100 %}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}
                </style>
                </head>
                <body>
                <center>
                    <table style='width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;' align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
               
                                    <tr>
               
                                        <td align='center' valign='top' id='bodyCell' style='-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;'>
                      
                                            <div class='main'>
                        <p style='text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;'>
                            <img src='https://ontrackimagestore.blob.core.windows.net/images/OnTrackBanner.png' width='100%' alt='ONTRACK' style='-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;'>
                        </p>

                        <h1>Hi [FirstName]</h1>

                        <p>Thank you for your purchase!</p>

                        <br/>
                        <h3>Order Summary</h3>
                        <hr/>

                        <table style='width: 100%; border:none;'>
                            <tr>
                             <th> Ticket </th>
                             <th> Qty </th>
                             <th> Total </th>
                           </tr>
                           <tr>
                             <td> [Product] </td>
                             <td> [Quantity] </td>
                             <td> [Total] </td>
                           </tr>
 
                        </table>

                        <table style='width: 100%; border:none;'>
                            <tr>
                             <th> Authorisation </th>
                             <th> Transaction Date </th>
                           </tr>
                           <tr>
                             <td> [Authorisation] </td>
                             <td> [TrxDate] </td>
                           </tr>
 
                        </table>
 
                         <p>If you have any issues with your purchase please do not hesitate to contact us by replying to sports@ontrackbda.com</p>

                        <br>
                        Thanks!
                        <br>

                        <strong>ONTRACK</strong>

                        <br><br>
                        <hr style='border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;'>
                        <p style='text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;'>
                            If you did not make this request, please contact us by replying to sports@ontrackbda.com.
                        </p>
                        </div>
                        </td>
                    </tr>
                    </table>
                </center>
                </body>
                </html>";

            //body = body.Replace("[FirstName]", customer.FirstName);
            //body = body.Replace("[Email]", customer.Email);
            //body = body.Replace("[Product]", fixture.HomeTeam.Name + " v " + fixture.AwayTeam.Name);
            //body = body.Replace("[Quantity]", quantity.ToString());
            //body = body.Replace("[Total]", order.Total.ToString("C"));
            //body = body.Replace("[Authorisation]", authorisation);
            //body = body.Replace("[TrxDate]", order.Date.ToString("MMM dd yyyy - H:mm:ss"));

            mailMessage.Body = body;

            try
            {
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return false;
        }
    }
}
