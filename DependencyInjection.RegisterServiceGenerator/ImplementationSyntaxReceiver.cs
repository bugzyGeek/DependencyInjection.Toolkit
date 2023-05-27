using System;
using System.Collections.Generic;
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

            List<string> scopes = new List<string>();
            var scope = int.Parse(arguments[0].ToString());

            if (scope < 1 || scope > 3)
                throw new ArgumentException($"The class {classDeclaration.Identifier.Text} recieved an invalid parameter for AddService Attribute", $"Parameter : FactoryScope, Value : {scope}");
            for (int i = 0; i < arguments.Count; i++)
            {
            }

            var interfaces = i.Split(',');
        }

        private void GetAddServiceInfo()
        {

        }
    }
}