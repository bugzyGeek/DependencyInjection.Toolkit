using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public class ImplementationSyntaxReceiver : ISyntaxReceiver
    {
        public AddServiceInfoList ServiceInfoList { get; } = new AddServiceInfoList();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            ClassDeclarationSyntax classDeclaration = syntaxNode.GetParent<ClassDeclarationSyntax>();
            var arguments = (syntaxNode as AttributeSyntax).ArgumentList.Arguments;
            var scope = int.Parse(arguments[0].ToString());
            var i = arguments[0].ToString().Trim('"');

            if (scope < 1 || scope > 3)
                throw new ArgumentException($"The class {classDeclaration.Identifier.Text} recieved an invalid parameter for AddService Attribute", $"Parameter : FactoryScope, Value : {scope}");
        }
    }

    public class AddServiceInfoList
    {
        public List<AddServiceInfo> AddServiceInfos { get; } = new List<AddServiceInfo>();

        public void Add(string className, string interfaceName, int scope)
        {
            string factoryScope = scope switch
            {
                1 => "FactoryScope.Transient",
                2 => "FactoryScope.Scope",
                3 => "FactoryScope.Singleton"
            };

            bool serviceMapped = AddServiceInfos.Any(r => r.Class.Equals(className) && r.Interface.Equals(interfaceName) && r.Scope.Equals(factoryScope));
            if (serviceMapped)
                throw new InvalidOperationException($"The Implementation {className} and the Interface {interfaceName} have been added multiple times");

            AddServiceInfos.Add(new AddServiceInfo(className,interfaceName, factoryScope));

        }
    }
}