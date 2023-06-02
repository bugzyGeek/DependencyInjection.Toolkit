using System;
using System.Linq;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjection.Toolkit
{
    public class MainSyntaxReceiver : ISyntaxReceiver
    {
        public AddServiceInfoList ServiceInfoList { get; } = new AddServiceInfoList();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            AttributeSyntax attributeSyntax = GetAttributeSyntax(syntaxNode);
            if (attributeSyntax is null)
                return;

            AttributeArgumentSyntax[] argumentSyntaxes = GetAttributeArguments(attributeSyntax);
            ClassDeclarationSyntax classDeclaration = syntaxNode.GetParent<ClassDeclarationSyntax>();
            if (classDeclaration == null)
                return;

            (int scope, string[] interfaces) = GetArguments(argumentSyntaxes);

            var noInterface = interfaces.FirstOrDefault(r => r.Equals("AttInterface.None")) != null;

            if (noInterface && interfaces.Length > 0)
                GeneratorDiagnostic.GetDiagnosticDescriptor("SG00001", "Invalid Scope", "Invalid scope for class {0}", "Service Registration", DiagnosticSeverity.Error).Add(classDeclaration.GetLocation(), classDeclaration.Identifier.Text);

            if (scope < 1 || scope > 3)
                GeneratorDiagnostic.GetDiagnosticDescriptor("SG00001", "Invalid Scope", "Invalid scope for class {0}", "Service Registration", DiagnosticSeverity.Error).Add(classDeclaration.GetLocation(), classDeclaration.Identifier.Text);

            ServiceInfoList.Add(classDeclaration, interfaces, scope);
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