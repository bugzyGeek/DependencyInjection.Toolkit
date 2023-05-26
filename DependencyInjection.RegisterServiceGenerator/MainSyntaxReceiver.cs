using System;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjection.Toolkit
{
    public class MainSyntaxReceiver : ISyntaxReceiver
    {
        public ImplementationSyntaxReceiver Implementation { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            var isServiceAttribute = syntaxNode is AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "AddService" } };

            if (!isServiceAttribute)
                return;

            Implementation.OnVisitSyntaxNode(syntaxNode);
        }
    }
}