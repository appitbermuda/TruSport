using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnTrackWebService.Interfaces
{
    public interface ITicketRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(ClaimsPrincipal claimsPrincipal, string id);
        Task<bool> Insert(T item);
        Task<bool> Update(T item);
        Task<bool> Delete(string id);
    }
}
