namespace DependencyInjectionToolkit.DependencyInjection;

/// <summary>
/// A factory used to create a service type registered to the specified <typeparamref name="IServiceCollection"/> the <typeparamref name="Factory"/> is initialized
/// </summary>
/// <typeparam name="T">The service type to be created</typeparam>
public class Factory<T> : IFactory<T> where T : class
{
    private readonly Func<Type, T?> _func;
    public Factory(Func<Type, T?> func)
    {
        _func = func;
    }

    /// <summary>
    /// Create an instance of a service type of <typeparamref name="T"/> with a specific implementation type of <typeparamref name="I"/>
    /// </summary>
    /// <typeparam name="I">The implementation type of the service type</typeparam>
    /// <returns>Returns an instance of <typeparamref name="T"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual T Create<I>() where I : class, T
    {
        T service = _func(typeof(I)) ?? throw new InvalidOperationException();
        return service;
    }
}
