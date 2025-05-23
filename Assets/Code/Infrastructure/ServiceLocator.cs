using System;
using System.Collections.Generic;

namespace PMT
{
    internal class ServiceLocator
    {
        private static Dictionary<Type, IService> _services = new();
        public static T Register<T>(T service) where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
                _services[typeof(T)] = service;
            else
                _services.Add(typeof(T), service);
            return service;
        }

        public static T Resolve<T>() where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
                return (T)_services[typeof(T)];
            else
                throw new Exception("Сервис не найден.");
        }
    }
}