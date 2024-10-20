Dependency Injection Toolkit

## Initializing

In order to use the Dependency Injection Toolkit you need to call the extension method on `IServiceCollection` interface as follows:

```csharp
// Add to your using statement
using DependencyInjectionToolkit.DependencyInjection;

// register Dependency Injection Toolkit on IServiceCollection
service.Initialize();
```
## Version Compatibility

DependencyInjectionToolkit is compatible with the following .NET versions:

- **.NET 6**: The minimum required version for using DependencyInjectionToolkit.
- **.NET 7**: Fully compatible.
- **.NET 8**: Fully compatible and backward compatible with .NET 6 and .NET 7.

This ensures that you can use DependencyInjectionToolkit in modern .NET applications while planning for future updates.


## Further information

For more information please visit:

- Our documentation site: https://github.com/bugzyGeek/DependencyInjection.Toolkit/wiki

- Our GitHub repository: https://github.com/bugzyGeek/DependencyInjection.Toolkit