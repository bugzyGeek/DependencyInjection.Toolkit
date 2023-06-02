using Microsoft.CodeAnalysis.CSharp.Syntax;

public record struct AddServiceInfo(ClassDeclarationSyntax Class, string[] Interface, string Scope);
