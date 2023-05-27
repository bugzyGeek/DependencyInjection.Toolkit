using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace DependencyInjectionToolkit.DependencyInjection.RegisterServiceGenerator
{
    public class AddServiceInfoList
    {
        public List<AddServiceInfo> AddServiceInfos { get; } = new List<AddServiceInfo>();

        public void Add(string className, string interfaceName, int scope)
        {
            string factoryScope = scope switch
            {
                1 => "FactoryScope.Transient",
                2 => "FactoryScope.Scope",
                3 => "FactoryScope.Singleton"
            };

            bool serviceMapped = AddServiceInfos.Any(r => r.Class.Equals(className) && r.Interface.Equals(interfaceName) && r.Scope.Equals(factoryScope));
            if (serviceMapped)
                throw new InvalidOperationException($"The Implementation {className} and the Interface {interfaceName} have been added multiple times");

            AddServiceInfos.Add(new AddServiceInfo(className, interfaceName, factoryScope));

        }
    }
}