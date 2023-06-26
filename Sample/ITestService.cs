using DependencyInjectionToolkit.DependencyInjection.Register.Service;

namespace Sample;
public interface ITestService
{
    void DoSomething();
}

public interface ITestServiceA
{
    void DoSomething();
}

public interface ITestServiceB
{
    void DoSomething();
}

[AddService(FactoryScope.Transient)]
public interface ITestServiceC
{
    void DoSomething();
}
