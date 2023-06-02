// See https://aka.ms/new-console-template for more information

using System;
using DependencyInjectionToolkit.DependencyInjection;
using DependencyInjectionToolkit.DependencyInjection.Factory;
using Microsoft.Extensions.DependencyInjection;
using Sample;

var services = new ServiceCollection();

// Initialize the factory services
services.Initialize();

Console.WriteLine("Start");


var s = services.BuildServiceProvider();

var f = s.GetRequiredService<IFactory<ITestService>>();

var a = s.GetRequiredService<ITestServiceA>();

var b = s.GetRequiredService<IFactory<ITestServiceB>>();

var c = s.GetRequiredService<IFactory<ITestServiceC>>();

var d = s.GetRequiredService<TestServiceD>();

f.Create<TestServiceA>().DoSomething();
a.DoSomething();
b.Create<TestServiceC>().DoSomething();
c.Create<TestServiceB>().DoSomething();
d.DoSomething();
