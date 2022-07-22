using System;
using System.Collections.Generic;

public class ServicesContainer
{
    private static ServicesContainer _instance;
    private readonly IDictionary<Type, object> _services = new Dictionary<Type, object>();

    public static ServicesContainer Instance => _instance ??= new ServicesContainer();

    public void AddService<TType>(object serviceInstance)
    {
        _services.Add(typeof(TType), serviceInstance);
    }

    public TType GetService<TType>()
    {
        return (TType)_services[typeof(TType)];
    }
}