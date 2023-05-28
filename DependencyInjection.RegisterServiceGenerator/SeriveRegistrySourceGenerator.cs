using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.ObjectPool;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjection.Toolkit
{
    [Generator]
    public class SeriveRegistrySourceGenerator : ISourceGenerator
    {
        private SyntaxtNodeProcessor Implementation { get; set; } = new SyntaxtNodeProcessor();
        public void Execute(GeneratorExecutionContext context)
        {
            var reciever = (MainSyntaxReceiver)context.SyntaxReceiver;
            Implementation.Process(context, reciever);
            
            var filename = "ServiceRegistry.g.cs";

            var source = new StringBuilder();
            source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            source.AppendLine("using DependencyInjectionToolkit.DependencyInjection.Factory;");
            source.AppendLine("namespace GeneratorTest");
            source.AppendLine("{\n");
            source.AppendLine("\t/// <summary>");
            source.AppendLine("\t/// Auto generatered class");
            source.AppendLine("\t/// </summary>");
            source.AppendLine("\tpublic static class SeriveRegistry");
            source.AppendLine("\t{");

            // Method code
            source.AppendLine("\t\t/// <summary>");
            source.AppendLine("\t\t/// Registers all dependency services");
            source.AppendLine("\t\t/// </summary>");
            source.AppendLine("\t\t/// <param name=\"service\">Specified <paramref name=\"IServiceCollection\"/> the types are to be registered to</param>");
            source.AppendLine("\t\t/// <exception cref=\"ArgumentNullException\"></exception>");
            source.AppendLine("\t\tpublic static void RegistorServices(this IServiceCollection service)");
            source.AppendLine("\t\t{");

            foreach (var info in Implementation.GeneratingInfoLists.GeneratingInfos)
            {
                if (!string.IsNullOrEmpty(info.Interface))
                    source.AppendLine($"\t\t\tservice.AddFactory<{info.Interface}, {info.Class}>(global::DependencyInjectionToolkit.DependencyInjection.Factory.{info.Scope});");
                else
                    source.AppendLine($"\t\t\tservice.AddFactory<{info.Class}>(global::DependencyInjectionToolkit.DependencyInjection.Factory.{info.Scope});");
            }

            source.AppendLine("\t\t}");

            source.AppendLine("\t}");
            source.Append('}');

            foreach (var diagnostic in GeneratorDiagnostic.Diagnostics)
                context.ReportDiagnostic(diagnostic);

            context.AddSource(filename, source.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
        }
    }
}
