using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Interfaces
{
    public interface IPaymentRepository<T>
    {
        Task<PaymentResponse> Authorize(PaymentAuthorize paymentAuthorize);
        Task<PaymentResponse> AuthorizeTest(PaymentAuthorize paymentAuthorize);
    }
}
