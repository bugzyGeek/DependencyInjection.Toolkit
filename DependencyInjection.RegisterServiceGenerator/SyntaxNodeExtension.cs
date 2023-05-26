using System;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public static class SyntaxNodeExtension
    {
        public static T GetParent<T>(this SyntaxNode node)
        {
            var parent = node.Parent;
            while (true)
            {
                if (parent == null)
                    throw new NullReferenceException(nameof(parent));

                if (parent is T t)
                    return t;

                parent = parent.Parent;
            }
        }
    }
}
