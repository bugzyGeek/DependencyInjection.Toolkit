using DependencyInjectionToolkit.DependencyInjection.Factory;

namespace DependencyInjectionToolkit.DependencyInjection.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AddServiceAttribute : System.Attribute
    {
        public AddServiceAttribute(FactoryScope scope, params string[] tInterface )
        {

        }
    }
}
