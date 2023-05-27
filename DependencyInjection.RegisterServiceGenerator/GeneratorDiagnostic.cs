using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal static class GeneratorDiagnostic
    {
        internal static List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();


        internal static void Add(string id, string title, string messageFormat, string category, DiagnosticSeverity severity)
        {
            var descriptor = new DiagnosticDescriptor(
                             id: id,
                             title: title,
                             messageFormat: messageFormat,
                             category: category,
                             defaultSeverity: severity,
                             isEnabledByDefault: true);
        }
    }
}
