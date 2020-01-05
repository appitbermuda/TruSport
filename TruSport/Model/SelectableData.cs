using System;
namespace TruSport.Model
{
    public class SelectableData<T>
    {

        public T Data { get; set; }

        public bool StarterSelected { get; set; }

        public bool SubSelected { get; set; }
    }
}
