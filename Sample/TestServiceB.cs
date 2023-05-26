using System;

namespace Sample;

public class TestServiceB : ITestService
{
    public void DoSomething()
    {
        Console.WriteLine("TestServiceB did something");
    }
}
