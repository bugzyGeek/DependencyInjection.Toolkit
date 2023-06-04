using System.Linq;
using DependencyInjection.Toolkit;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal class SyntaxtNodeProcessor
    {
        public AddGeneratingInfoList GeneratingInfoLists { get; } = new AddGeneratingInfoList();

        public void Process(GeneratorExecutionContext context, MainSyntaxReceiver receiver)
        {
            foreach (AddServiceInfo serviceInfo in receiver.ServiceInfoList.AddServiceInfos)
            {
                var semanticModel = context.Compilation.GetSemanticModel(serviceInfo.Class.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(serviceInfo.Class);
                var classSymbol = symbol as INamedTypeSymbol;
                var interfaces = classSymbol.AllInterfaces;

                // check if Interface.None is passed to the attribute
                var noInterface = serviceInfo.Interface.Any(r => r.Equals("Interface.None"));

                if (interfaces.Length == 0 && serviceInfo.Interface != null && serviceInfo.Interface.Length > 0 && !noInterface)
                {
                    GeneratorDiagnostic.GetDiagnosticDescriptor("SG0003", "Invalid Interface Mapping", "An interface(s) was passed to the attrubute AddService for class {0} is not extending any interface", "DI Service Registration", DiagnosticSeverity.Error)
                .Add(serviceInfo.Class.GetLocation(), serviceInfo.Class.Identifier.Text);
                    continue;
                }

                string clazz = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (interfaces.Length == 0 || noInterface)
                {
                    GeneratingInfoLists.Add(clazz, string.Empty, serviceInfo.Scope, serviceInfo.Class);
                }
                else if (interfaces.Length > 0 && (serviceInfo.Interface == null || serviceInfo.Interface.Length == 0))
                {
                    foreach (var i in interfaces)
                        GeneratingInfoLists.Add(clazz, i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), serviceInfo.Scope, serviceInfo.Class);
                }
                else if (serviceInfo.Interface != null && serviceInfo.Interface.Length > 0)
                {
                    foreach (var i in serviceInfo.Interface)
                    {
                        INamedTypeSymbol interfaceTypeSymbol = interfaces.FirstOrDefault(r => Func(r, i));

                        if (interfaceTypeSymbol == null)
                        {
                            GeneratorDiagnostic.GetDiagnosticDescriptor("SG0003", "Invalid Interface Mapping", "An interface {0} was passed to the attrubute AddService for class {1} is not extending any interface", "DI Service Registration", DiagnosticSeverity.Error)
                                .Add(serviceInfo.Class.GetLocation(), i, serviceInfo.Class.Identifier.Text);
                            continue;
                        }
                        else
                        {
                            GeneratingInfoLists.Add(clazz, interfaceTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), serviceInfo.Scope, serviceInfo.Class);
                        }
                    }
                }
            }
        }

        private bool Func(INamedTypeSymbol r, string s) => r.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Equals(s) || r.Name.Equals(s);
    }
}
