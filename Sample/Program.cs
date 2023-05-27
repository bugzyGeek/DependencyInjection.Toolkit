// See https://aka.ms/new-console-template for more information

using DependencyInjectionToolkit.DependencyInjection.Factory;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Initialize the factory services
services.InitializeFactory();

Console.WriteLine((FactoryScope)5);

Console.WriteLine(nameof(FactoryScope));


