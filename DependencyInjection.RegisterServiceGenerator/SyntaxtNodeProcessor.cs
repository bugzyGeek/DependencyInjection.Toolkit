using System.Linq;
using DependencyInjection.Toolkit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal class SyntaxtNodeProcessor
    {
        public AddGeneratingInfoList GeneratingInfoLists { get; } = new AddGeneratingInfoList();
        MainSyntaxReceiver _receiver;
        Compilation _compilation;

        public void Process(GeneratorExecutionContext context, MainSyntaxReceiver receiver)
        {
            _receiver = receiver;
            _compilation = context.Compilation;
            ProcessInterfaceRegister();
            ProcessClassRegistry();
        }

        /// <summary>
        /// This method process all the classes that were maked by the AddService attribute
        /// </summary>
        private void ProcessClassRegistry()
        {
            foreach (AddClassServiceInfo serviceInfo in _receiver.ServiceInfoList.AddServiceInfos)
            {
                var semanticModel = _compilation.GetSemanticModel(serviceInfo.Class.SyntaxTree);
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

        /// <summary>
        /// Process all Interfaces that where marked with the AddService attribute.
        /// </summary>
        private void ProcessInterfaceRegister()
        {
            foreach (AddInterfaceServiceInfo serviceInfo in _receiver.ServiceInfoList.AddInterfaceServiceInfos)
            {
                var semanticModel = _compilation.GetSemanticModel(serviceInfo.Interface.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(serviceInfo.Interface);
                var interfaceSymbol = symbol as INamedTypeSymbol;
                var classes = _compilation.SyntaxTrees
                    .SelectMany(x => x.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                    .Where(x => x.BaseList?.Types.Any(y => y.Type.ToString() == interfaceSymbol.Name) ?? false);

                foreach(ClassDeclarationSyntax clazz in classes)
                {
                    var classSemanticModel = _compilation.GetSemanticModel(clazz.SyntaxTree);
                    var classSymbol = classSemanticModel.GetDeclaredSymbol(clazz);
                    GeneratingInfoLists.Add((classSymbol as INamedTypeSymbol).ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), serviceInfo.Scope, clazz);
                }
            }
        }

        private bool Func(INamedTypeSymbol r, string s) => r.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Equals(s) || r.Name.Equals(s);
    }
}
