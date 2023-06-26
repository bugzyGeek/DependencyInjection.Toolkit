using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal class AddGeneratingInfoList
    {
        public List<AddGeneratingInfo> GeneratingInfos { get; } = new List<AddGeneratingInfo>();


        public void Add(string className, string interfaceName, string scope, SyntaxNode node)
        {
            bool serviceMapped = GeneratingInfos.Any(r => r.Class.Equals(className) && r.Interface.Equals(interfaceName) && r.Scope.Equals(scope));
            if (!serviceMapped)
                GeneratingInfos.Add(new AddGeneratingInfo(className, interfaceName, scope));
        }
    }
}
