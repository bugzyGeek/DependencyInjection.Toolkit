using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal static class GeneratorDiagnostic
    {
        internal static ImmutableList<Diagnostic> Diagnostics { get; private set; } = ImmutableList<Diagnostic>.Empty;

        internal static DiagnosticDescriptor GetDiagnosticDescriptor(string id, string title, string messageFormat, string category, DiagnosticSeverity severity)
        {
            return new DiagnosticDescriptor(
                             id: id,
                             title: title,
                             messageFormat: messageFormat,
                             category: category,
                             defaultSeverity: severity,
                             isEnabledByDefault: true);
        }

        internal static void Add(this DiagnosticDescriptor descriptor, Location location, params string[] messageArgs)
        {
            Diagnostics = Diagnostics.Add(Diagnostic.Create(descriptor, location, messageArgs));
        }
    }
}
