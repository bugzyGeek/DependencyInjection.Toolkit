using System;
using System.Collections.Generic;
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
            AttributeSyntax attributeSyntax = GetAttributeSyntax(syntaxNode);
            if (attributeSyntax is null)
                return;

            ClassDeclarationSyntax classDeclaration = syntaxNode.GetParent<ClassDeclarationSyntax>();
            SeparatedSyntaxList<AttributeArgumentSyntax> attributeArguments = GetAttributeArguments(attributeSyntax);
            (int scope, string[] registerInterfaces) arguments = GetArguments(attributeArguments);
            Type[] interfaces = GetAllClassInterface(classDeclaration);
        }

        private AttributeSyntax GetAttributeSyntax(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "AddService" } } attribute))
                return null;
            return attribute;
        }

        private SeparatedSyntaxList<AttributeArgumentSyntax> GetAttributeArguments(AttributeSyntax attributeSyntax)
        {
            return attributeSyntax.ArgumentList.Arguments;
        }

        private (int scope, string[] interfaces) GetArguments(SeparatedSyntaxList<AttributeArgumentSyntax> arguments)
        {
            string[] interfaces = new string[arguments.Count - 1];
            var scope = int.Parse(arguments[0].ToString());

            for (int i = 1; i < arguments.Count; i++)
                interfaces[i] = arguments[i].ToString();

            return (scope, interfaces);
        }

        private Type[] GetAllClassInterface(ClassDeclarationSyntax classDeclaration)
        {
            Type classType = classDeclaration.GetType();

            return classType.GetInterfaces();
        }
    }
}