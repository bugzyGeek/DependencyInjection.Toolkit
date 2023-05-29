# DependencyInjectionToolkit

Dependency injection and factory pattern are two powerful techniques that can help you write clean, testable, and maintainable code in .NET. Dependency injection allows you to decouple your code from specific implementations of dependencies, making it easier to swap them or mock them for testing. Factory pattern allows you to create instances of a service type with different implementations at runtime, depending on some conditions.

However, implementing these techniques manually can be tedious and error-prone. You need to write a lot of boilerplate code to define the interfaces and classes for your services and factories, register them with the correct scope in the service collection, and resolve them from the service provider.

DependencyInjectionToolkit is a library that simplifies this process for you. With DependencyInjectionToolkit, you can:

- Use a simple attribute to mark the classes that you want to register as services
- Specify the scope and the interfaces of the services using the attribute parameters
- Use a generic factory class to create instances of the service type with a specific implementation type
- Use a static class to initialize the service collection with all the registered services

DependencyInjectionToolkit makes dependency injection and factory pattern easy and convenient in .NET.

## Installation

To install DependencyInjectionToolkit, run the following command in your project directory:

```bash
dotnet add package DependencyInjectionToolkit
```

Alternatively, you can use the NuGet Package Manager in Visual Studio to search and install DependencyInjectionToolkit.

## Usage

To use DependencyInjectionToolkit, follow these steps:

1. Mark the classes that you want to register as services with the `AddService` attribute. You can specify the scope (Transient, Scope, or Singleton) and the interfaces that the class implements as parameters. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Attribute;

// This will register the Service class as a singleton service that implements the IService interface
[AddService(FactoryScope.Singleton, nameof(IService))]
public class Service : IService
{
    // ...
}
```

The `AddService` attribute will automatically register the class as a service with the specified scope and interfaces. It will also register a factory for creating instances of this class.

If your class does not implement any interface or you want to register it as itself, you need to omit the interface types or use the `AttInterface.None` enum value in the attribute. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Attribute;

// This will register the Foo class as a transient service without any interface
[AddService(FactoryScope.Transient)]
public class Foo
{
    // ...
}
```

```csharp
using DependencyInjectionToolkit.DependencyInjection.Attribute;

// This will register the Bar class as a singleton service without any interface, even though it implements IBar
[AddService(FactoryScope.Singleton, AttInterface.None)]
public class Bar : IBar
{
    // ...
}
```

If your class implements one or more interfaces and you want to register it with specific interface types, you need to specify them in the attribute. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Attribute;

// This will register the Baz class as a scoped service that implements IBaz and IQux, but not IQuux
[AddService(FactoryScope.Scope, nameof(IBaz), nameof(IQux))]
public class Baz : IBaz, IQux, IQuux
{
    // ...
}
```

If your class implements one or more interfaces and you want to register it with all interface types, you can omit the interface types in the attribute. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Attribute;

// This will register the Qux class as a transient service that implements both IQux and IQuuz
[AddService(FactoryScope.Transient)]
public class Qux : IQux, IQuuz
{
    // ...
}
```

2. Initialize the service collection with the `ServiceRegistry` class. This will register all the services marked with `AddService` attribute and their corresponding factories. For example:

```csharp
using Microsoft.Extensions.DependencyInjection;
using DependencyInjectionToolkit.DependencyInjection.Factory;

var services = new ServiceCollection();
services.Initialize(); // This will register all the services marked with AddService attribute
```

The `Initialize` method will scan all the assemblies in your project and find all the classes marked with `AddService` attribute. It will then register them as services and factories in the service collection.

3. Use the `Factory<T>` class to create instances of the service type `T` with a specific implementation type. You can resolve the factory from the service provider and call its `Create<I>` method, where `I` is the implementation type. For example:

```csharp
using DependencyInjectionToolkit.DependencyInjection.Factory;

var serviceProvider = services.BuildServiceProvider();
var factory = serviceProvider.GetRequiredService<IFactory<IService>>();
var service = factory.Create<Service>(); // This will create an instance of Service class that implements IService interface
```

The `Factory<T>` class is a generic factory that can create instances of any service type `T` with any implementation type `I`. You just need to specify the implementation type as a generic parameter when calling the `Create<I>` method.

## Documentation

For more details and examples, please refer to the [documentation](https://github.com/DependencyInjectionToolkit/DependencyInjectionToolkit/wiki).

## Testing

To run tests for DependencyInjectionToolkit, use the following command:

```bash
dotnet test
```

This will run all the unit tests in the test project using xUnit.

## Contributing

We welcome contributions from anyone who is interested in improving DependencyInjectionToolkit. To contribute, please follow these steps:

1. Fork this repository and clone it to your local machine.
2. Create a new branch for your feature or bug fix.
3. Make your changes and commit them with descriptive messages.
4. Push your branch to your forked repository.
5. Create a pull request from your branch to this repository's main branch.
6. Wait for feedback or approval from the maintainers.

Please make sure to follow the coding style and conventions of this project. Also, please write unit tests for any new features or bug fixes.

## License

This library is licensed under the MIT License. See [LICENSE](LICENSE) for details.