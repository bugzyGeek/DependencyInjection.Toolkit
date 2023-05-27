using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjection.Toolkit
{
    public class MainSyntaxReceiver : ISyntaxReceiver
    {
        public AddServiceInfoList ServiceInfoList { get; } = new AddServiceInfoList();
        public static List<Diagnostic> Diagnostics {  get; } = new List<Diagnostic>();

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
            ProcessDetailsToGenerate(interfaces, arguments.registerInterfaces, arguments.scope, classType.FullName);
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

        private Type[] GetAllClassInterface(Type classType)
        {
            return classType.GetInterfaces();
        }

        private void ProcessDetailsToGenerate(Type[] interfaces, string[] registerationInterfaces, int scope, string className)
        {
            bool implementsInterfaces = (interfaces != null &&  interfaces.Length > 0);
            bool specifiedInterfaces = (registerationInterfaces != null && registerationInterfaces.Length > 0);

            if (!implementsInterfaces && !specifiedInterfaces)
                ServiceInfoList.Add(className, string.Empty, scope);
            else if(!specifiedInterfaces && implementsInterfaces)
            {
                foreach (Type interfaceType in interfaces)
                {
                    ServiceInfoList.Add(className, interfaceType.FullName, scope);
                }
            }
            else if(specifiedInterfaces && implementsInterfaces)
            {
                var interfaceSet = new HashSet<string>(registerationInterfaces);
                List<string> matchedInterfaces = interfaces.Where(r => interfaceSet.Contains(r.Name) || interfaceSet.Contains(r.FullName)).Select(r => r.FullName).ToList();

                if (matchedInterfaces.Count != registerationInterfaces.Length)
                    throw new Exception();

                foreach (string interfaceType in matchedInterfaces)
                {
                    ServiceInfoList.Add(className, interfaceType, scope);
                }
            }
            
        }
    }
}