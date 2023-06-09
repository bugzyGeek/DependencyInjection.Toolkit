﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public class AddServiceInfoList
    {
        public List<AddServiceInfo> AddServiceInfos { get; } = new List<AddServiceInfo>();

        public void Add(ClassDeclarationSyntax className, string[] interfaceName, int scope)
        {
            string factoryScope = ScopeConverter.ConvertToString(scope);

            AddServiceInfos.Add(new AddServiceInfo(className, interfaceName, factoryScope));
        }
    }
}