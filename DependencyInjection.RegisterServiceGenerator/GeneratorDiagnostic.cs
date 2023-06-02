using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal static class GeneratorDiagnostic
    {
        internal static List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();


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
            Diagnostics.Add(Diagnostic.Create(descriptor, location, messageArgs));
        }
    }
}
