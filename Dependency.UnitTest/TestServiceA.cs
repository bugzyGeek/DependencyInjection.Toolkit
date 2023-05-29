using System;
using DependencyInjectionToolkit.DependencyInjection.Attribute;
using DependencyInjectionToolkit.DependencyInjection.Factory;

namespace DependencyInjection.Factory.UnitTest;
[AddService(FactoryScope.Transient)]
public class TestServiceA : ITestService
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceA did something");
    }
}
