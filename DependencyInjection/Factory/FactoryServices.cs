using DependencyInjectionToolkit.DependencyInjection.Register.Service;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionToolkit.DependencyInjection;

public static class FactoryServices
{

    /// <summary>
    /// Add a service of type specified in <typeparamref name="TInterface"/> with the Implementation type specified in <typeparamref name="TImplementation"/> to the specified <typeparamref name="IServiceCollection"/>
    /// </summary>
    /// <typeparam name="TInterface">The type of service to add</typeparam>
    /// <typeparam name="TImplementation">The type of implementation to use</typeparam>
    /// <param name="services">The specificed IServiceCollection the factory is registered to</param>
    /// <param name="factoryScope">The type of service to register</param>
    /// <returns>A reference of this instance after the operation is completed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddFactory<TInterface, TImplementation>(this IServiceCollection services, FactoryScope factoryScope)
where TInterface : class
where TImplementation : class, TInterface
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        switch (factoryScope)
        {
            case FactoryScope.Scope:
                services.AddScoped<TInterface, TImplementation>();
                break;
            case FactoryScope.Singleton:
                services.AddSingleton<TInterface, TImplementation>();
                break;
            case FactoryScope.Transient:
                services.AddTransient<TInterface, TImplementation>();
                break;
        }
        services.AddSingleton<IFactory<TInterface>, Factory<TInterface>>();
        services.AddSingleton<Func<Type, TInterface?>>(x => (t) =>
        {
            if (x is null)
                throw new ArgumentNullException(nameof(x));

            try
            {
                return x.GetServices<TInterface>().FirstOrDefault(x => x.GetType().Equals(t));
            }
            catch (InvalidOperationException)
            {
                using IServiceScope scope = x.CreateScope();
                return scope.ServiceProvider.GetServices<TInterface>().FirstOrDefault(x => x.GetType().Equals(t));
            }
        });


        return services;
    }

    /// <summary>
    /// Add a service of type specified in <typeparamref name="TImplementation"/> to the specified <typeparamref name="IServiceCollection"/>
    /// </summary>
    /// <typeparam name="TImplementation">The type of implementation to use</typeparam>
    /// <param name="services">The specifice IServiceCollection the factory is being generated on</param>
    /// <param name="factoryScope">The type of service to register</param>
    /// <returns>A reference of this instance after the operation is completed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddFactory<TImplementation>(this IServiceCollection services, FactoryScope factoryScope)
        where TImplementation : class
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        switch (factoryScope)
        {
            case FactoryScope.Scope:
                services.AddScoped<TImplementation>();
                break;
            case FactoryScope.Singleton:
                services.AddSingleton<TImplementation>();
                break;
            case FactoryScope.Transient:
                services.AddTransient<TImplementation>();
                break;
        }
        services.AddSingleton<IFactory<TImplementation>, Factory<TImplementation>>();

        return services;
    }
}