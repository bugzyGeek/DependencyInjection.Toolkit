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

## Version Compatibility

DependencyInjectionToolkit is compatible with the following .NET versions:

- **.NET 6**: The minimum required version for using DependencyInjectionToolkit.
- **.NET 7**: Fully compatible.
- **.NET 8**: Fully compatible and backward compatible with .NET 6 and .NET 7.

This ensures that you can use DependencyInjectionToolkit in modern .NET applications while planning for future updates.


## Installation

To install DependencyInjectionToolkit, run the following command in your project directory:

```bash
dotnet add package DependencyInjectionToolkit
```

Alternatively, you can use the NuGet Package Manager in Visual Studio to search and install DependencyInjectionToolkit.

## Getting Started

To get started with DependencyInjectionToolkit, follow these steps:

1. **Install the Package**: Run the following command in your project directory:
    
```bash
dotnet add package DependencyInjectionToolkit
```

2. **Annotate Your Classes**: Use the `AddService` attribute to mark your classes for registration.

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

[AddService(FactoryScope.Singleton, nameof(IService))]
public class Service : IService
{
    // ...
}
```

3. **Initialize Services**: Use the `ServiceRegistry` class to scan and register services.
    
```csharp
using DependencyInjectionToolkit.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.Initialize();
```

4. **Resolve Services**: Use the service provider to resolve or inject services.

```csharp
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = services.BuildServiceProvider();
var service = serviceProvider.GetRequiredService<IService>();
```

## Supported Scopes

DependencyInjectionToolkit supports three scopes:

1. **Transient**: A new instance is created each time it is requested.
2. **Scoped**: A single instance is created per scope. Typically, a scope corresponds to a single request in web applications.
3. **Singleton**: A single instance is created and shared throughout the application's lifetime.

## Usage

To use DependencyInjectionToolkit, follow these steps:

1. Mark the classes that you want to register as services with the AddService attribute. You can specify the lifetime (Transient, Scoped, or Singleton) and the interfaces that the class implements as parameters. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Register.Service;

// This will register the Service class as a singleton service that implements the IService interface
[AddService(FactoryScope.Singleton, nameof(IService))]
public class Service : IService
{
    // ...
}
```

The AddService attribute will automatically register the class as a service with the specified lifetime and interfaces. It will also register a factory for creating instances of this class.

If your class does not implement any interface and you want to register it as itself, you can omit the interface types in the attribute. However, if your class does implement interfaces but you do not want to register any of them, you should use the Interface.None enum value in the attribute to explicitly indicate that no interfaces should be registered. For example:

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

The Initialize method initializes the dependencies and will scan all the assemblies in your project to find all the classes marked with the AddService attribute. It will then register them as services and factories in the service collection.

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

`AddService` attribute and `AddFactory` method are combined to register all implementations and interfaces needed in your application

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

## Real-World Use Cases

Here are some real-world use cases for DependencyInjectionToolkit:

1. **Web Applications**: Simplify the registration and resolution of services in ASP.NET Core applications.
2. **Microservices**: Manage dependencies in microservices architectures, ensuring each service has its own scope.
3. **Desktop Applications**: Use dependency injection in WPF or WinForms applications to manage services and their lifetimes.
4. **Testing**: Easily mock dependencies for unit testing by swapping out implementations.


## Where to find more information?

For more details and examples, please check out our documentation [here](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/wiki).

## How to contribute?

This project is open source and welcomes contributions from anyone who is interested. Please see our [CONTRIBUTING](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/blob/main/CONTRIBUTING.md) file for guidelines on how to contribute.

## What is the license?

This project is licensed under the MIT License - see our [LICENSE](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/blob/main/LICENSE) file for details.
