using DependencyInjectionToolkit.DependencyInjection.Attribute;
using DependencyInjectionToolkit.DependencyInjection.Factory;

namespace Sample;

[AddService(0,"ITestService","iui")]
public class TestServiceA : ITestService
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceA did something");
    }
}

[AddService(FactoryScope.Transient, nameof(ITestService))]
public class TestServiceC : ITestService
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceA did something");
    }
}