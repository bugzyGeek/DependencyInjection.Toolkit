using DependencyInjectionToolkit.DependencyInjection.Attribute;
using DependencyInjectionToolkit.DependencyInjection.Factory;

namespace Sample;

//[AddService(FactoryScope.Transient)]
//public class TestServiceA : ITestService
//{
//    public void DoSomething()
//    {
//        Console.WriteLine("TestServiceA did something");
//    }
//}

[AddService(FactoryScope.Transient, nameof(ITestServiceB))]
[AddService(FactoryScope.Transient, nameof(ITestServiceB))]
[AddService(FactoryScope.Transient, nameof(ITestServiceB))]
public class TestServiceC :  ITestServiceB
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceA did something");
    }
}

//[AddService(FactoryScope.Transient)]
//public class TestServiceD
//{
//    public void DoSomething()
//    {
//        Console.WriteLine("TestServiceA did something");
//    }
//}