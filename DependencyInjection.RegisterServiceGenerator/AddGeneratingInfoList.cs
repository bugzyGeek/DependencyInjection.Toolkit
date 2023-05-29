using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal class AddGeneratingInfoList
    {
        public List<AddGeneratingInfo> GeneratingInfos { get; } = new List<AddGeneratingInfo>();


        public void Add(string className, string interfaceName, string scope, SyntaxNode node)
        {
            bool serviceMapped = GeneratingInfos.Any(r => r.Class.Equals(className) && r.Interface.Equals(interfaceName) && r.Scope.Equals(scope));
            if (serviceMapped)
            {
                GeneratorDiagnostic.GetDiagnosticDescriptor("SG0002", "Duplicate service registration", $"The interface {{0}} with scope {scope} and implementation {{1}} was already registered multiple times the compile ignored all and registered only registered one", "DI Service Registration", DiagnosticSeverity.Warning)
                .Add(node.GetLocation(), interfaceName, className);
            }

            GeneratingInfos.Add(new AddGeneratingInfo(className, interfaceName, scope));
        }
    }
}
