﻿namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
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
    }
}
