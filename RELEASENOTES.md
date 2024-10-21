# Release Notes for DependencyInjectionToolkit v2.1.0

## New Features
- **Incremental Source Generator**: Improved performance and incremental compilation support by refactoring the service registry source generator.

## Improvements
- **Immutable Collections**: Enhanced internal data structures to use immutable collections for better performance and thread safety.
- **Service Registration**: Improved handling of service registration with attributes for more flexible and powerful dependency injection configurations.
- **Documentation**: Added a "Version Compatibility" section to specify compatibility with .NET 6, 7, and 8. Enhanced documentation with detailed instructions on getting started, supported scopes, and real-world use cases for DependencyInjectionToolkit.

## Removals
- **Deprecated Code**: Removed obsolete internal code to streamline the codebase and improve maintainability.

## Project Configuration
- **Solution Update**: Updated project references and configurations to ensure compatibility and ease of use.

## NuGet Package Contents
- **README**: Included `PACKAGEREADME.md` in the NuGet package for detailed documentation.
- **Icon**: Included `DependencyInjectionToolkit.png` as the package icon.

## Summary
This release brings significant improvements to the DependencyInjectionToolkit, including performance enhancements, better immutability practices, and improved documentation. We recommend updating to this version to take advantage of the new features and improvements.

## How to Update
To update to the latest version, use the following command in your .NET project:

```bash
dotnet add package DependencyInjectionToolkit --version 2.1.0
```

Thank you for using DependencyInjectionToolkit! If you encounter any issues or have any feedback, please visit our [GitHub repository](https://github.com/bugzyGeek/DependencyInjection.Toolkit) to report them.
