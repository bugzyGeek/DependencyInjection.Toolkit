using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjection.Toolkit
{
    public class MainSyntaxReceiver : ISyntaxReceiver
    {
        public AddServiceInfoList ServiceInfoList { get; } = new AddServiceInfoList();
        public static List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            AttributeSyntax attributeSyntax = GetAttributeSyntax(syntaxNode);
            if (attributeSyntax is null)
                return;

            ClassDeclarationSyntax classDeclaration = syntaxNode.GetParent<ClassDeclarationSyntax>();
            SeparatedSyntaxList<AttributeArgumentSyntax> attributeArguments = GetAttributeArguments(attributeSyntax);
            (int scope, string[] registerInterfaces) arguments = GetArguments(attributeArguments);
            Type classType = classDeclaration.GetType();
            Type[] interfaces = GetAllClassInterface(classType);
            ProcessDetailsToGenerate(interfaces, arguments.registerInterfaces, arguments.scope, classType.FullName, classDeclaration);
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
            var scope = ScopeConverter.ConvertToInt(arguments[0].ToString());

            for (int i = 1; i < arguments.Count; i++)
                interfaces[i - 1] = arguments[i].ToString();

            return (scope, interfaces);
        }

        private Type[] GetAllClassInterface(Type classType)
        {
            return classType.GetInterfaces();
        }

        private void ProcessDetailsToGenerate(Type[] interfaces, string[] registerationInterfaces, int scope, string className, SyntaxNode node)
        {
            bool implementsInterfaces = (interfaces != null && interfaces.Length > 0);
            bool specifiedInterfaces = (registerationInterfaces != null && registerationInterfaces.Length > 0);

            if (!implementsInterfaces && !specifiedInterfaces)
                ServiceInfoList.Add(className, string.Empty, scope, node);
            else if (!specifiedInterfaces && implementsInterfaces)
            {
                foreach (Type interfaceType in interfaces)
                {
                    ServiceInfoList.Add(className, interfaceType.FullName, scope, node);
                }
            }
            else if (specifiedInterfaces && implementsInterfaces)
            {
                var interfaceSet = new HashSet<string>(registerationInterfaces);
                List<string> matchedInterfaces = interfaces.Where(r => interfaceSet.Contains(r.Name) || interfaceSet.Contains(r.FullName)).Select(r => r.FullName).ToList();

                if (matchedInterfaces.Count != registerationInterfaces.Length)
                {
                    ;
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var i in interfaces)
                    {
                        if (!registerationInterfaces.Contains(i.Name) || !interfaceSet.Contains(i.FullName))
                        {
                            stringBuilder.Append($",{i} ");
                            stringBuilder.Remove(0, 1);
                        }
                    }

                    GeneratorDiagnostic.GetDiagnosticDescriptor("SG0001", $"Invalid Interface(s) registored for {className}", $"The interface(s) {0} are not implemented by the class {1}", "DI Service Registration", DiagnosticSeverity.Error)
                        .Add(node.GetLocation(), stringBuilder.ToString().Trim(), className);
                    return;
                }

                foreach (string interfaceType in matchedInterfaces)
                {
                    ServiceInfoList.Add(className, interfaceType, scope, node);
                }
            }

        }
    }
}