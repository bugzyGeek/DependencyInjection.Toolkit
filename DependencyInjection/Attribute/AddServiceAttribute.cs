using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionToolkit.DependencyInjection.Factory;

namespace DependencyInjectionToolkit.DependencyInjection.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddServiceAttribute : System.Attribute
    {
        public AddServiceAttribute(FactoryScope scope, params string[] tInterface )
        {

        }
    }
}
