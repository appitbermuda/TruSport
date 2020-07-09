using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnTrackWebService.Models;

namespace OnTrackWebService.Interfaces
{
    public interface INewsRepository<T>
    {
        Task<List<T>> Feed();
    }
}
