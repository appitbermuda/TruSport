using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnTrackWebService.Interfaces
{
    public interface IOnTrackRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string id);
        Task Insert(T item);
        Task Update(T item);
        Task Delete(string id);
    }
}
