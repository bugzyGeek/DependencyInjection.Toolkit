using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;

namespace DependencyInjection.Toolkit
{
    [Generator]
    public class SeriveRegistrySourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            ISyntaxReceiver reciever = (MainSyntaxReceiver) context.SyntaxReceiver;

            var filename = "ServiceRegistry.g.cs";

            StringBuilder source = new StringBuilder();
            source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
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

            // code will be generated here

            source.AppendLine("\t\t}");

            source.AppendLine("\t}");
            source.Append('}');

            context.AddSource(filename, source.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
        }
    }
}