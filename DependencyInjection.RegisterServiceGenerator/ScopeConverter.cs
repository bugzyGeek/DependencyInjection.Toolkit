using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    internal static class ScopeConverter
    {
        internal static int ConvertToInt(string scope)
        {
            if (int.TryParse(scope, out int result))
                return result;

            return scope switch
            {
                "FactoryScope.Transient" => 1,
                "FactoryScope.Scope" => 2,
                "FactoryScope.Singleton" => 3,
                _ => -1
            };

        }

        internal static string ConvertToString(int scope)
        {
            return scope switch
            {
                1 => "FactoryScope.Transient",
                2 => "FactoryScope.Scope",
                3 => "FactoryScope.Singleton",
                _ => ""
            };
        }

        internal static string ConvertToInterface(ExpressionSyntax expression)
        {
            if (int.TryParse(expression.ToString(), out int result))
                if (result == 0)
                    return "Interface.None";

            return expression switch
            {
                LiteralExpressionSyntax literalExpression => literalExpression.Token.ValueText,
                InvocationExpressionSyntax invocation when invocation.ArgumentList.Arguments.Count == 1 && invocation.Expression is IdentifierNameSyntax identifier && identifier.Identifier.Text == "nameof" => invocation.ArgumentList.Arguments[0].Expression.ToString(),
                ExpressionSyntax ex when ex.ToString().Equals("Interface.None") => "Interface.None",
                _ => ""
            };
        }
    }
}
