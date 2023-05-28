﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjection.Toolkit
{
    public class MainSyntaxReceiver : ISyntaxReceiver
    {
        public AddServiceInfoList ServiceInfoList { get; } = new AddServiceInfoList();
        public List<AttributeSyntax> AttributeList { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            AttributeSyntax attributeSyntax = GetAttributeSyntax(syntaxNode);
            if (attributeSyntax is null)
                return;

            AttributeList.Add(attributeSyntax);

            AttributeArgumentSyntax[] argumentSyntaxes = GetAttributeArguments(attributeSyntax);
            ClassDeclarationSyntax classDeclaration = syntaxNode.GetParent<ClassDeclarationSyntax>();
            (int scope, string[] interfaces) arguments = GetArguments(argumentSyntaxes);

            if (arguments.scope < 1 || arguments.scope > 3)
                GeneratorDiagnostic.GetDiagnosticDescriptor("SG00001", "Invalid Scope", "Invalid scope for class {0}", "Service Registration", DiagnosticSeverity.Error).Add(classDeclaration.GetLocation(), classDeclaration.Identifier.Text);

            ServiceInfoList.Add(classDeclaration, arguments.interfaces, arguments.scope);
        }

        private AttributeSyntax GetAttributeSyntax(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "AddService" } } attribute))
                return null;
            return attribute;
        }

        private AttributeArgumentSyntax[] GetAttributeArguments(AttributeSyntax attributeSyntax)
        {
            return attributeSyntax.ArgumentList.Arguments.ToArray();
        }

        private (int scope, string[] interfaces) GetArguments(AttributeArgumentSyntax[] arguments)
        {
            string[] interfaces = new string[arguments.Length - 1];
            var scope = ScopeConverter.ConvertToInt(arguments[0].Expression.ToString());
            for (int i = 1; i < arguments.Length; i++)
            {
                interfaces[i - 1] = arguments[i].Expression switch
                {
                    LiteralExpressionSyntax literalExpression => literalExpression.Token.ValueText,
                    InvocationExpressionSyntax invocation when invocation.ArgumentList.Arguments.Count == 1 && invocation.Expression is IdentifierNameSyntax identifier && identifier.Identifier.Text == "nameof" => invocation.ArgumentList.Arguments[0].Expression.ToString(),
                    _ => "~"
                };
            };

            return (scope, interfaces);
        }
    }
}