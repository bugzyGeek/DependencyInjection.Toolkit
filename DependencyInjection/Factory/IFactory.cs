namespace DependencyInjectionToolkit.DependencyInjection.Factory;

/// <summary>
/// Specifies a contract for a factory service descriptors
/// </summary>
/// <typeparam name="M">The service type registered in the specified <typeparamref name="IServiceCollection"/> to be created</typeparam>
public interface IFactory<M>
{
    M Create<I>() where I : class, M;
}