﻿using DependencyInjectionToolkit.DependencyInjection.Register.Service;

namespace ConsoleTest;

[AddService(FactoryScope.Transient)]
public class TestServiceA : ITestService, ITestServiceB, ITestServiceC
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceA did something");
    }
}

[AddService(FactoryScope.Transient, "ITestService", nameof(ITestServiceC))]
public class TestServiceB : ITestService, ITestServiceB, ITestServiceC
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceB did something");
    }
}

[AddService(FactoryScope.Transient, nameof(ITestServiceB))]
public class TestServiceC : ITestServiceB
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceC did something");
    }
}

[AddService(FactoryScope.Transient)]
public class TestServiceD
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceD did something");
    }
}

[AddService(FactoryScope.Singleton, nameof(ITestServiceA))]
public class TestServiceE : ITestServiceA
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceE did something");
    }
}

[AddService(FactoryScope.Singleton, Interface.None)]
public class TestServiceF : ITestServiceA
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceE did something");
    }
}

[AddService(FactoryScope.Singleton, 0)]
public class TestServiceG : ITestServiceA
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceE did something");
    }
}