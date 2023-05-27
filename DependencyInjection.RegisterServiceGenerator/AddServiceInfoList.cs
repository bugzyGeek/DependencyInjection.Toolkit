using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public class AddServiceInfoList
    {
        public List<AddServiceInfo> AddServiceInfos { get; } = new List<AddServiceInfo>();

        public void Add(string className, string interfaceName, int scope, SyntaxNode node)
        {
            string factoryScope = scope switch
            {
                1 => "FactoryScope.Transient",
                2 => "FactoryScope.Scope",
                3 => "FactoryScope.Singleton",
                _ => throw new ArgumentException($"Invalid scope value: {scope}")
            };

            bool serviceMapped = AddServiceInfos.Any(r => r.Class.Equals(className) && r.Interface.Equals(interfaceName) && r.Scope.Equals(factoryScope));
            if (serviceMapped)
            {
                GeneratorDiagnostic.GetDiagnosticDescriptor("SG0002", "Duplicate service registration", $"The interface {0} and implementation {1} was already registered the compiple ignored only registered one", "DI Service Registration", DiagnosticSeverity.Warning)
                .Add(node.GetLocation(), interfaceName, className);
            }

            AddServiceInfos.Add(new AddServiceInfo(className, interfaceName, factoryScope));

        }
    }
}