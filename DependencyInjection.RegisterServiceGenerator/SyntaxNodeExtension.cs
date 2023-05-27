using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public static class SyntaxNodeExtension
    {
        public static T GetParent<T>(this SyntaxNode node)
        {
            var parent = node.AncestorsAndSelf().OfType<T>().FirstOrDefault();
            // Throw an exception if parent is null
            if (parent == null)
                throw new NullReferenceException(nameof(parent));
            return parent;
        }
    }
}
