using System;
using System.Threading.Tasks;
using OnTrackWebService.Data;
using ServiceReference;

namespace OnTrackWebService.Models.Shop
{
    public class Request
    {
        public async Task<AuthorizeResponse> Payment(PaymentAuthorize paymentAuthorize)
        {
            var client = new ServicesClient();
            var request = new AuthorizeRequest();
            var transactionRequest = new TransactionModificationRequest();

            request.CardDetails = new CardDetails();
            request.TransactionDetails = new TransactionDetails();


            //Amount
            paymentAuthorize.Amount = paymentAuthorize.Amount.PadLeft(12, '0');

            //CardDetails
            request.CardDetails.CardCVV2 = paymentAuthorize.CVV;
            request.CardDetails.CardExpiryDate = paymentAuthorize.Expiry;
            request.CardDetails.CardNumber = paymentAuthorize.CardNumber;
            //request.CardDetails.IssueNumber = cardDetails.IssueNumber;
            //request.CardDetails.StartDate = cardDetails.StartDate;

            //TransactionDetails
            request.TransactionDetails.AcquirerId = Constants.AcquirerId;
            request.TransactionDetails.Amount = paymentAuthorize.Amount;
            request.TransactionDetails.Currency = Constants.Currency;
            request.TransactionDetails.CurrencyExponent = 2;
            //request.TransactionDetails.IPAddress = "";
            request.TransactionDetails.MerchantId = Constants.MerchantId;
            request.TransactionDetails.OrderNumber = Constants.OrderNumberPrefix + DateTime.Now.Ticks.ToString("000000000000");

            //Compute Hash from required fields
            request.TransactionDetails.Signature = Helper.ComputeHash(String.Join("", Constants.ProcessingPW,
                request.TransactionDetails.MerchantId,
                request.TransactionDetails.AcquirerId,
                request.TransactionDetails.OrderNumber,
                request.TransactionDetails.Amount,
                request.TransactionDetails.Currency));
            request.TransactionDetails.SignatureMethod = "SHA1";
            request.TransactionDetails.TransactionCode = 8;


            transactionRequest.AcquirerId = Constants.AcquirerId;
            transactionRequest.Amount = paymentAuthorize.Amount;
            transactionRequest.CurrencyExponent = 2;
            transactionRequest.MerchantId = Constants.MerchantId;
            transactionRequest.OrderNumber = request.TransactionDetails.OrderNumber;
            transactionRequest.ModificationType = 1;

            await client.TransactionModificationAsync(transactionRequest);
            AuthorizeResponse response = await client.AuthorizeAsync(request);
            //CreditCardTransactionResults results = response.CreditCardTransactionResults;

            await client.CloseAsync();

            return response;

        }
    }
}
