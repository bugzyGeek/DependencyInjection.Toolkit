# DependencyInjectionToolkit

DependencyInjectionToolkit is a C# library that simplifies the usage of dependency injection and factory patterns in your .NET 6.0 applications. It provides attributes and extensions methods to register services and create them using factories. It also allows you to use the registered services without using factories if you don't need to create instances with different implementations at runtime.

## Why use DependencyInjectionToolkit?

Dependency injection is a design pattern that helps you write loosely coupled, testable, and maintainable code. It allows you to inject dependencies (such as services, repositories, or configurations) into your classes, instead of creating them directly. This way, you can easily swap or mock your dependencies for different environments or scenarios.

Factory patterns are a design pattern that helps you create objects without exposing the creation logic to the client. It allows you to encapsulate the object creation process in a separate class or method, and return an interface or abstract class instead of a concrete type. This way, you can easily change or extend the object creation logic without affecting the client code.

DependencyInjectionToolkit combines these two patterns and provides a simple and elegant way to register and create your services using attributes and extension methods. It integrates with the built-in dependency injection framework of .NET 6.0, and supports three scopes: transient, scoped, and singleton.

![GitHub release (latest by date)](https://img.shields.io/github/v/release/Bugzygeek/DependencyInjection.Toolkit)
![Nuget](https://img.shields.io/nuget/v/DependencyInjectionToolkit)
![License](https://img.shields.io/github/license/Bugzygeek/DependencyInjection.Toolkit)

## Features

- AddService attribute: Allows you to annotate your classes with the desired scope and interface(s) to register them as services in the IServiceCollection. You can use the nameof operator to specify the interface name, or use Interface.None to register the class without an interface.
- Factory interface: Allows you to create instances of your services using a generic factory that resolves the implementation type from the service type. You can use the Create\<I>() method to specify the implementation type, or use Create\<T>() to use the default implementation type registered for the service type.
- ServiceRegistry class: A static class that scans your assembly for classes with the AddService attribute and registers them as services using the FactoryServices extension methods. You can use the Initialize() method to register all your services in one line of code.
- AddFactory method: This is an extension method of `IServiceCollection` used to register classes with the desired scope and interface(s) as services in the IServiceCollection. This is the same as adding the `AddService` attribute on a class declaration. This is used when registering a class from an external library and you do not have access to the class declaration to add an attribute to it.

## Installation

To install DependencyInjectionToolkit, run the following command in your project directory:

```bash
dotnet add package DependencyInjectionToolkit
```

Alternatively, you can use the NuGet Package Manager in Visual Studio to search and install DependencyInjectionToolkit.

## Usage

To use DependencyInjectionToolkit, follow these steps:

1. Mark the classes that you want to register as services with the AddService attribute. You can specify the scope (Transient, Scope, or Singleton) and the interfaces that the class implements as parameters. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

// This will register the Service class as a singleton service that implements the IService interface
[AddService(FactoryScope.Singleton, nameof(IService))]
public class Service : IService
{
    // ...
}
```

The AddService attribute will automatically register the class as a service with the specified scope and interfaces. It will also register a factory for creating instances of this class.

If your class does not implement any interface or you want to register it as itself, you need to omit the interface types in the attribute. Alternatively, you can use the Interface.None enum value in the attribute if you want to explicitly indicate that your class does not implement any interface. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

// This will register the Foo class as a transient service without any interface
[AddService(FactoryScope.Transient)]
public class Foo
{
    // ...
}
```

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

// This will register the Bar class as a singleton service without any interface, even though it implements IBar
[AddService(FactoryScope.Singleton, Interface.None)]
public class Bar : IBar
{
    // ...
}
```

If your class implements one or more interfaces and you want to register it with specific interface types, you need to specify them in the attribute. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

// This will register the Baz class as a scoped service that implements IBaz and IQux, but not IQuux
[AddService(FactoryScope.Scope, nameof(IBaz), nameof(IQux))]
public class Baz : IBaz, IQux, IQuux
{
    // ...
}
```

If your class implements one or more interfaces and you want to register it with all interface types, you can omit the interface types in the attribute. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

// This will register the Qux class as a transient service that implements both IQux and IQuuz
[AddService(FactoryScope.Transient)]
public class Qux : IQux, IQuuz
{
    // ...
}
```

2. Use the ServiceRegistry class to scan your assembly for classes with the AddService attribute and register them as services in the IServiceCollection. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.Initialize(); // This is an extension method that will register all the services marked with AddService attribute
```

The Initialize method initializes the dependency and also will scan all the assemblies in your project and find all the classes marked with AddService attribute. It will then register them as services and factories in the service collection.

3. Registering services using `AddFactory` method.

 ```csharp
// Create an IServiceCollection
var services = new ServiceCollection();

// Initialize the factory services
services.Initialize();

// Add a service of type SomeInterface with the implementation type SomeImplementation using a scoped lifetime
services.AddFactory<SomeInterface, SomeImplementation>(FactoryScope.Scope);

// Add another service of type SomeInterface with the implementation type AnotherImplementation using a scoped lifetime
services.AddFactory<SomeInterface, AnotherImplementation>(FactoryScope.Scope);

// Add a service of type AnotherClass using a transient lifetime
services.AddFactory<AnotherClass>(FactoryScope.Transient);
```

`AddService` attribute and `AddFactory` method are compined to register all implementations and Interfaces needed in your application

4. Use the Factory<T> class to create instances of the service type T with a specific implementation type. You can resolve the factory from the service provider and call its Create<I> method, where I is the implementation type. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Factory;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = services.BuildServiceProvider();
var factory = serviceProvider.GetRequiredService<IFactory<IService>>();
var service = factory.Create<Service>(); // This will create an instance of Service class that implements IService interface
```

The Factory<T> class is a generic factory that can create instances of any service type T with any implementation type I. You just need to specify the implementation type as a generic parameter when calling the Create<I> method.

Note that you can also use the registered services without using factories if you don't need to create instances with different implementations at runtime. You can simply resolve or inject them using their interface types. For example:

```csharp
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = services.BuildServiceProvider();
var service = serviceProvider.GetRequiredService<IService>(); // This will resolve an instance of Service class that implements IService interface
```

Or you can inject an instance of Service class into your classes using constructor injection. For example:

```csharp
public class Test
{
    private readonly IService _service;
    
    public Test(IService service)
    {
        _service = service; // This will inject an instance of Service class that implements IService interface
    }
    
    // ...
}
```

## Where to find more information?

For more details and examples, please check out our documentation [here](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/wiki).

## How to contribute?

This project is open source and welcomes contributions from anyone who is interested. Please see our [CONTRIBUTING](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/blob/main/CONTRIBUTING.md) file for guidelines on how to contribute.

## What is the license?

This project is licensed under the MIT License - see our [LICENSE](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/blob/main/LICENSE) file for details.
