using System;
using System.Collections.Generic;

namespace TruSport.Data
{
    public static class PushServiceContainer
    {
        static readonly Dictionary<Type, Lazy<object>> services
            = new Dictionary<Type, Lazy<object>>();

        public static void Register<T>(Func<T> function)
            => services[typeof(T)] = new Lazy<object>(() => function());

        public static T Resolve<T>()
            => (T)Resolve(typeof(T));

        public static object Resolve(Type type)
        {
            {
                if (services.TryGetValue(type, out var service))
                    return service.Value;

                throw new KeyNotFoundException($"Service not found for type '{type}'");
            }
        }
    }
}
