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
            // Filter for Attribute  syntax node for AddServices
            AttributeSyntax attributeSyntax = GetAttributeSyntax(syntaxNode);
            if (attributeSyntax is null)
                return;

            // Get the arguments passed to the attribute
            AttributeArgumentSyntax[] argumentSyntaxes = GetAttributeArguments(attributeSyntax);

            // Get the class or interface the attribute is applied to
            ClassDeclarationSyntax classDeclaration = syntaxNode.GetParent<ClassDeclarationSyntax>();
            InterfaceDeclarationSyntax interfaceDeclaration = syntaxNode.GetParent<InterfaceDeclarationSyntax>();
            if (classDeclaration is null && interfaceDeclaration is null)
                return;

            // process the aguments passed to the attribute
            (int scope, string[] interfaces) = GetArguments(argumentSyntaxes);

            /// checks if the scope passed to the attribute is valid or not
            if (scope < 1 || scope > 3)
                GeneratorDiagnostic.GetDiagnosticDescriptor("SG00001", "Invalid Scope", "Invalid scope for class {0}", "Service Registration", DiagnosticSeverity.Error).Add(classDeclaration.GetLocation(), classDeclaration.Identifier.Text);

            if (classDeclaration is not null)
                ServiceInfoList.Add(classDeclaration, interfaces, scope);
            else
            {
                /// warns the user to remove the interface parameters passed to the Attribute if the attribute is set on an interface
                if(interfaces.Length > 0)
                    GeneratorDiagnostic.GetDiagnosticDescriptor("SG00004", "Invalid Attribute", "Remove interface name from the attribute parameters", "Service Registration", DiagnosticSeverity.Info).Add(classDeclaration.GetLocation());

                ServiceInfoList.Add(interfaceDeclaration, scope);
            }
        }

        /// <summary>
        /// Filter sysntax node for a AddServicen attribute type
        /// </summary>
        /// <param name="syntaxNode">Syntax node been inspected</param>
        /// <returns><paramref name="AttributeSyntax"/> is the syntax node is a type of AddService</returns>
        private AttributeSyntax GetAttributeSyntax(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "AddService" } } attribute))
                return null;
            return attribute;
        }

        /// <summary>
        /// Takes an <paramref name="AttributeSyntax"/> and retuns its arguments.
        /// </summary>
        /// <param name="attributeSyntax"><paramref name="AttributeSyntax"/> to get the arguments from</param>
        /// <returns>An array of  <paramref name="AttributeArgumentSyntax"/></returns>
        private AttributeArgumentSyntax[] GetAttributeArguments(AttributeSyntax attributeSyntax)
        {
            return attributeSyntax.ArgumentList.Arguments.ToArray();
        }

        /// <summary>
        /// Accepts a  <paramref name="AttributeSyntax"/> and process them to the respective types for the source generator to process
        /// </summary>
        /// <param name="arguments">An array <paramref name="AttributeArgumentSyntax"/></param>
        /// <returns></returns>
        private (int scope, string[] interfaces) GetArguments(AttributeArgumentSyntax[] arguments)
        {
            // create a string array to store interfaces
            string[] interfaces = new string[arguments.Length - 1];
            // convert the factory scope type to it's respective int.
            var scope = ScopeConverter.ConvertToInt(arguments[0].Expression.ToString());

            for (int i = 1; i < arguments.Length; i++)
            {
                // get all the interface parameters passed to the attribute
                interfaces[i - 1] = ScopeConverter.ConvertToInterface(arguments[i].Expression);
            };

            return (scope, interfaces);
        }
    }
}