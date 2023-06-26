using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public class AddServiceInfoList
    {
        public List<AddClassServiceInfo> AddServiceInfos { get; } = new List<AddClassServiceInfo>();
        public List<AddInterfaceServiceInfo> AddInterfaceServiceInfos { get; } = new List<AddInterfaceServiceInfo>();

        public void Add(ClassDeclarationSyntax className, string[] interfaceName, int scope)
        {
            string factoryScope = ScopeConverter.ConvertToString(scope);
            AddServiceInfos.Add(new AddClassServiceInfo(className, interfaceName, factoryScope));
        }

        public void Add(InterfaceDeclarationSyntax @interface, int scope)
        {
            string factoryScope = ScopeConverter.ConvertToString(scope);
            AddInterfaceServiceInfos.Add(new AddInterfaceServiceInfo(@interface, factoryScope));
        }
    }
}