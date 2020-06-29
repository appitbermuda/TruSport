using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnTrackWebService.Interfaces
{
    public interface ISettingRepository<T>
    {
        Task<IEnumerable<T>> All();
        Task<T> Get(string id);
        Task Update(T item);
    }
}
