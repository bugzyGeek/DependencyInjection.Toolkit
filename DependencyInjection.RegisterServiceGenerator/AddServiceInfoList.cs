using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal class AddServiceInfoList
    {
        internal ImmutableList<AddServiceInfo> AddServiceInfos { get; private set; } = ImmutableList<AddServiceInfo>.Empty;

        public void Add(Compilation compilation, ClassDeclarationSyntax classDeclaration, string[] attributeArgs, string scope)
        {
            // Get the semantic model and the class symbol
            var classSymbol = compilation.GetSemanticModel(classDeclaration.SyntaxTree).GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            // Get the fully qualified name of the class
            var fullyQualifiedClassName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            // Check if the attribute has the argument Interface.None
            if (attributeArgs.Contains("Interface.None"))
            {
                AddServiceInfos = AddServiceInfos.Add(new AddServiceInfo(fullyQualifiedClassName, Array.Empty<string>(), scope));
                return;
            }

            // Get all implemented interfaces
            var implementedInterfaces = classSymbol.AllInterfaces.ToList();

            // If no interface is passed to the attribute, register the class with all implemented interfaces
            if (attributeArgs.Length == 0)
            {
                var interfaces = implementedInterfaces.Select(i => i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)).ToArray();
                AddServiceInfos = AddServiceInfos.Add(new AddServiceInfo(fullyQualifiedClassName, interfaces, scope));
                return;
            }

            var interfacesToAdd = new List<string>();
            bool hasInvalidInterface = false;

            foreach (var arg in attributeArgs)
            {
                // Check if the interface passed to the attribute is implemented by the class
                var interfaceTypeSymbol = implementedInterfaces.Find(i => i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == arg || i.Name == arg);
                if (interfaceTypeSymbol == null)
                {
                    GeneratorDiagnostic.GetDiagnosticDescriptor("SG0003", "Invalid Interface Mapping", $"The interface {arg} passed to the attribute AddService for class {classDeclaration.Identifier.Text} is not implemented by the class", "DI Service Registration", DiagnosticSeverity.Error)
                        .Add(classDeclaration.GetLocation(), classDeclaration.Identifier.Text);
                    hasInvalidInterface = true;
                    continue;
                }

                // Get the fully qualified name of the interface
                var fullyQualifiedInterfaceName = interfaceTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                // Check if the interface is already registered
                bool serviceMapped = AddServiceInfos.Any(r => r.Class == fullyQualifiedClassName && r.Interface.Contains(fullyQualifiedInterfaceName) && r.Scope == scope);
                if (serviceMapped)
                {
                    GeneratorDiagnostic.GetDiagnosticDescriptor("SG0002", "Duplicate service registration", $"The interface {fullyQualifiedInterfaceName} with scope {scope} and implementation {classDeclaration.Identifier.Text} was already registered multiple times. The compiler ignored all and registered only one.", "DI Service Registration", DiagnosticSeverity.Warning)
                        .Add(classDeclaration.GetLocation(), fullyQualifiedInterfaceName, classDeclaration.Identifier.Text);
                    hasInvalidInterface = true;
                    continue;
                }

                interfacesToAdd.Add(fullyQualifiedInterfaceName);
            }

            if (!hasInvalidInterface)
            {
                AddServiceInfos = AddServiceInfos.Add(new AddServiceInfo(fullyQualifiedClassName, interfacesToAdd.ToArray(), scope));
            }
        }
    }
}