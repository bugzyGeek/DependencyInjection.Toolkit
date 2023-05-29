using System;
using DependencyInjectionToolkit.DependencyInjection.Attribute;
using DependencyInjectionToolkit.DependencyInjection.Factory;

namespace DependencyInjection.Factory.UnitTest;

[AddService(FactoryScope.Scope)]
public class TestServiceB : ITestService
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceB did something");
    }
}
