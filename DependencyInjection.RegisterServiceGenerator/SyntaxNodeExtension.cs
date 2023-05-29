using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public static class SyntaxNodeExtension
    {
        public static T GetParent<T>(this SyntaxNode node)
        {
            var parent = node.AncestorsAndSelf().OfType<T>().FirstOrDefault();
           
            return parent;
        }
    }
}
