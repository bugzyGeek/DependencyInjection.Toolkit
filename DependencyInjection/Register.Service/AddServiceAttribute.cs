namespace DependencyInjectionToolkit.DependencyInjection.Register.Service;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AddServiceAttribute : Attribute
{
    public AddServiceAttribute(FactoryScope scope, params string[] tInterface)
    {

    }

    public AddServiceAttribute(FactoryScope scope, Interface tInterface)
    {
            
    }
}
