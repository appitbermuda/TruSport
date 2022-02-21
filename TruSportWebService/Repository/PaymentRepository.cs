using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;
using OnTrackWebService.Models.Ticket;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;

namespace OnTrackWebService.Repository
{
    public class PaymentRepository : IPaymentRepository<PaymentAuthorize>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;        

        public PaymentRepository(OnTrackContext context, IOptions<NotificationHubOptions> options, ILogger<PushNotificationRepository> logger)
        {
            _context = context;
            emailRepository = new EmailRepository(context);            
        }

        public async Task<TicketResponse> Scan(CustomerTicket scannedCustomerTicket, ClaimsPrincipal claimsUser)
        {
            TicketResponse ticketResponse = new TicketResponse();

            try
            {
                List<string> tickets = new List<string>();

                ////Get the current claims principal
                //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Get the claims values
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.UserName == username && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.IsActive)
                {
                    var eventTicket = await _context.EventTickets.Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == scannedCustomerTicket.EventTicketID);

                    var customerTicket = await _context.CustomerTickets
                        .Include(e => e.Order)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .FirstOrDefaultAsync(e => e.ID == scannedCustomerTicket.ID && e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID);

                    if (customerTicket != null)
                    {
                        if (!customerTicket.Validated)
                        {
                            customerTicket.Validated = true;
                            customerTicket.ValidatedTime = DateTime.Now.ToUniversalTime();

                            _context.CustomerTickets.Update(customerTicket);
                            await _context.SaveChangesAsync();

                            ticketResponse.Response = "Validated Successfully!";
                            ticketResponse.Ticket = customerTicket.EventTicket.Product.Age + " Ticket";
                            ticketResponse.IsValidated = true;

                            return ticketResponse;
                        }

                        ticketResponse.Response = "Ticket already validated!";
                        ticketResponse.IsValidated = false;

                        return ticketResponse;
                    }

                    ticketResponse.Response = "Invalid Ticket!";
                    ticketResponse.IsValidated = false;

                    return ticketResponse;

                }

                ticketResponse.Response = "You are not authorized to scan!";
                ticketResponse.IsValidated = false;

                return ticketResponse;
            }
            catch (Exception ex)
            {

            }

            ticketResponse.Response = "Error Validating Ticket!";
            ticketResponse.IsValidated = false;

            return ticketResponse;
        }

        public async Task<PaymentResponse> Authorize(PaymentAuthorize paymentAuthorization)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            try
            {
                var response = await request.AuhtorizePayment(paymentAuthorization);

                if (response.CreditCardTransactionResults.ResponseCode == "1")
                {
                    paymentResponse.Authorisation = response.CreditCardTransactionResults.AuthCode;
                    paymentResponse.OrderNumber = response.OrderNumber;

                    paymentResponse.IsApproved = true;                        

                    paymentResponse.Code = response.CreditCardTransactionResults.ResponseCode;
                    paymentResponse.Description = response.CreditCardTransactionResults.ReasonCodeDescription;

                    return paymentResponse;
                }


                paymentResponse.IsApproved = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Payment");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task<PaymentResponse> AuthorizeTest(PaymentAuthorize paymentAuthorization)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            try
            {
                paymentResponse.Authorisation = "TESTAUTHCODE";
                paymentResponse.OrderNumber = "TESTORDERNUM";

                paymentResponse.IsApproved = true;

                paymentResponse.Code = "1";
                paymentResponse.Description = "Success";

                return paymentResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Payment Test");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

//        public Stream CreateTicketPDF(Order order, CustomerTicket customerTicket, string TicketNumber)
//        {
//            try
//            {
//#if DEBUG
//                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
//#else
//                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
//#endif

//                //Create a new PDF document
//                PdfDocument doc = new PdfDocument();

//                //Add a page
//                PdfPage page = doc.Pages.Add();

//                //Create PDF graphics for the page
//                PdfGraphics graphics = page.Graphics;

//                //Loads the image as stream
//                FileStream imageStream = new FileStream("wwwroot/images/OnTrackBannerSmall.png", FileMode.Open, FileAccess.Read);
//                RectangleF bounds = new RectangleF(10, 0, 480, 189);
//                PdfImage image = PdfImage.FromStream(imageStream);
//                //Draws the image to the PDF page
//                page.Graphics.DrawImage(image, bounds);

//                bounds = new RectangleF(0, bounds.Bottom + 30, graphics.ClientSize.Width, 30);

//                //Set the standard font
//                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20, PdfFontStyle.Bold);
//                PdfFont subHeaderFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
//                PdfFont bodyFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
//                PdfFont bodyBoldFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 13, PdfFontStyle.Bold);
//                PdfFont lightHeaderFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
//                PdfFont smallInfoFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
//                PdfBrush lightHeaderBrush = new PdfSolidBrush(Color.Gray);
//                PdfStringFormat smallInfoFormat = new PdfStringFormat();
//                smallInfoFormat.WordWrap = PdfWordWrapType.Word;
//                smallInfoFormat.Alignment = PdfTextAlignment.Center;

//                PdfStringFormat headerFormat = new PdfStringFormat();
//                headerFormat.Alignment = PdfTextAlignment.Center;

//                //Draw the text
//                //Event
//                //PdfTextElement element = new PdfTextElement(customerTicket.EventTicket.SportEvent.Event, headerFont);
//                //PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));

//                graphics.DrawString(customerTicket.EventTicket.SportEvent.Event,
//                    headerFont, PdfBrushes.Black, new RectangleF(10, bounds.Top + 2, page.GetClientSize().Width, 30), headerFormat);

//                PdfTextElement element = new PdfTextElement(TicketNumber, bodyFont);
//                PdfLayoutResult result = element.Draw(page, new PointF(250, bounds.Bottom + 4));

//                //Ticket Type
//                element = new PdfTextElement("Ticket Type", lightHeaderFont, lightHeaderBrush);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 15));

//                element = new PdfTextElement(customerTicket.EventTicket.SportEvent.Title + " - " + customerTicket.EventTicket.Product.Age + (customerTicket.IsMemberTicket ? " (Member)" : string.Empty), subHeaderFont);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

//                //Event Date
//                element = new PdfTextElement("Event Date", lightHeaderFont, lightHeaderBrush);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

//                DateTime eventTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(customerTicket.EventTicket.SportEvent.EventTime, DateTimeKind.Utc), timeInfo);

//                element = new PdfTextElement(eventTime.AddHours(-1).ToString("MMM dd, yyyy - h:mm tt"), bodyFont);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

//                //Venue
//                element = new PdfTextElement("Venue", lightHeaderFont, lightHeaderBrush);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

//                element = new PdfTextElement(customerTicket.EventTicket.SportEvent.Field.Name, bodyFont);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));


//                //Order Info
//                element = new PdfTextElement("Order Information", bodyBoldFont);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 20));

//                //Order Number
//                element = new PdfTextElement("Order Number", lightHeaderFont, lightHeaderBrush);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

//                element = new PdfTextElement(order.OrderNumber, bodyFont);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

//                //Order Date
//                element = new PdfTextElement("Order Date", lightHeaderFont, lightHeaderBrush);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

//                DateTime orderDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(order.Date, DateTimeKind.Unspecified), timeInfo);

//                element = new PdfTextElement(orderDate.ToString("MMM dd, yyyy - h:mm tt"), bodyFont);
//                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

//                //graphics.DrawString("Contact Tracing", headerFont, PdfBrushes.Black, new PointF(0, 0));
//                //graphics.DrawString(contactTraces.FirstOrDefault().Order.Fixture, subHeaderFont, PdfBrushes.Black, new PointF(10, bounds.Top + 8));
//                //graphics.DrawString(contactTraces.FirstOrDefault().Order.FixtureDate.ToString("MMM dd, yyyy"), subHeaderFont, PdfBrushes.Black, new PointF(0, 0));


//                //Drawing QR barcode 
//                PdfQRBarcode qrBarcode = new PdfQRBarcode();

//                //Set Error Correction Level
//                qrBarcode.ErrorCorrectionLevel = PdfErrorCorrectionLevel.High;

//                //Set XDimension
//                qrBarcode.XDimension = 3;
//                qrBarcode.Text = JsonConvert.SerializeObject(customerTicket.CustomerMatchTicket);
//                ////Draw string
//                //page.Graphics.DrawString("Ticket QR", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(20, 180));
//                //Printing barcode on to the PDF
//                qrBarcode.Draw(page, new PointF(10, bounds.Top + 47));


//                //Information
//                graphics.DrawString("There are no refunds or exchanges. All terms and conditions related to this ticket and event can be found on www.ontrackbda.com/ticket/terms. Please print this ticket or display it on your smartphone for entry into the event. Do not duplicate ticket, as each ticket has a unique qr code. For more information, please contact us at tickets@ontrackbda.com.",
//                    smallInfoFont, PdfBrushes.Black, new RectangleF(10, result.Bounds.Bottom + qrBarcode.Size.Height + 15, page.GetClientSize().Width - 20, 50), smallInfoFormat);
                

//                //Save the PDF document to stream
//                MemoryStream stream = new MemoryStream();
//                doc.Save(stream);
//                //If the position is not set to '0' then the PDF will be empty.
//                stream.Position = 0;
//                //Close the document.
//                doc.Close(true);
//                //Defining the ContentType for pdf file.
//                //string contentType = "application/pdf";

//                //Creates a FileContentResult object by using the file contents, content type, and file name.
//                return stream;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex.Message, "PDF");
//            }

//            return null;
//        }
    }
}
