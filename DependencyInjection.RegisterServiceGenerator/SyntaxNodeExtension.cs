using System.Linq;
using Microsoft.CodeAnalysis;

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
