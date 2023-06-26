using Microsoft.CodeAnalysis.CSharp.Syntax;

public record struct AddClassServiceInfo(ClassDeclarationSyntax Class, string[] Interface, string Scope);

public record struct AddInterfaceServiceInfo(InterfaceDeclarationSyntax Interface, string Scope);