using System.Reflection;

namespace Infrastructure.CQRS
{
    public class MethodsAssembly
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetAssembly(typeof(MethodsAssembly));
        }
    }
}
