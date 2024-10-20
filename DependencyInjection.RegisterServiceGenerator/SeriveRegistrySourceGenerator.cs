using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DependencyInjection.Toolkit
{
    [Generator]
    public class SeriveRegistrySourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsClassDeclarationWithAddServiceAttribute(s),
                    transform: static (ctx, _) => GetClassDeclaration(ctx))
                .Where(static m => m is not null);

            // Combine the selected class declarations into a single collection
            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses =
                context.CompilationProvider.Combine(classDeclarations.Collect());

            // Register the source output
            context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        private static bool IsClassDeclarationWithAddServiceAttribute(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax classDeclaration &&
                   classDeclaration.AttributeLists
                       .SelectMany(al => al.Attributes)
                       .Any(a => a.Name.ToString() == "AddService");
        }

        private static ClassDeclarationSyntax GetClassDeclaration(GeneratorSyntaxContext context)
        {
            return (ClassDeclarationSyntax)context.Node;
        }

        private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
        {
            var serviceInfoList = new AddServiceInfoList();

            foreach (var classDeclaration in classes)
            {
                var attributeSyntax = classDeclaration.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .FirstOrDefault(a => a.Name.ToString() == "AddService");
                if (attributeSyntax != null)
                {
                    var argumentSyntaxes = attributeSyntax.ArgumentList.Arguments;
                    var (scope, attributeArgs) = GetArguments(argumentSyntaxes);



                    serviceInfoList.Add(compilation, classDeclaration, attributeArgs, scope);
                }
            }

            var source = new StringBuilder();
            source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            source.AppendLine("using DependencyInjectionToolkit.DependencyInjection;");
            source.AppendLine("namespace DependencyInjectionToolkit.DependencyInjection");
            source.AppendLine("{");
            source.AppendLine("\t/// <summary>");
            source.AppendLine("\t/// Auto generated class");
            source.AppendLine("\t/// </summary>");
            source.AppendLine("\tpublic static class ServiceRegistry");
            source.AppendLine("\t{");
            source.AppendLine("\t\t/// <summary>");
            source.AppendLine("\t\t/// Registers all dependency services");
            source.AppendLine("\t\t/// </summary>");
            source.AppendLine("\t\t/// <param name=\"service\">Specified <paramref name=\"IServiceCollection\"/> the types are to be registered to</param>");
            source.AppendLine("\t\t/// <exception cref=\"ArgumentNullException\"></exception>");
            source.AppendLine("\t\tpublic static void Initialize(this IServiceCollection service)");
            source.AppendLine("\t\t{");

            foreach (var info in serviceInfoList.AddServiceInfos)
            {
                if (info.Interface.Length > 0)
                {
                    foreach (var iface in info.Interface)
                    {
                        source.AppendLine($"\t\t\tservice.AddFactory<{iface}, {info.Class}>(global::DependencyInjectionToolkit.DependencyInjection.Register.Service.{info.Scope});");
                    }
                }
                else
                {
                    source.AppendLine($"\t\t\tservice.AddFactory<{info.Class}>(global::DependencyInjectionToolkit.DependencyInjection.Register.Service.{info.Scope});");
                }
            }

            source.AppendLine("\t\t}");
            source.AppendLine("\t}");
            source.AppendLine("}");

            foreach (var diagnostic in GeneratorDiagnostic.Diagnostics)
                context.ReportDiagnostic(diagnostic);

            _ = GeneratorDiagnostic.Diagnostics.Clear();

            context.AddSource("ServiceRegistry.g.cs", SourceText.From(source.ToString(), Encoding.UTF8));
        }

        private static (string scope, string[] interfaces) GetArguments(SeparatedSyntaxList<AttributeArgumentSyntax> arguments)
        {
            // create a string array to store attributeArgs
            string[] interfaces = new string[arguments.Count - 1];

            // convert the factory scope type to its respective string.
            var scope = ScopeConverter.ConvertToString(ScopeConverter.ConvertToInt(arguments[0].Expression.ToString()));

            for (int i = 1; i < arguments.Count; i++)
            {
                // get all the interface parameters passed to the attribute
                interfaces[i - 1] = ScopeConverter.ConvertToInterface(arguments[i].Expression);
            }

            return (scope, interfaces);
        }
    }
}
