using System.Reflection;
using Autofac.SmartInterceptors.Attributes;
using Castle.DynamicProxy;

namespace Autofac.SmartInterceptors.Interceptors
{
    /// <summary>
    /// Interceptor used for parameter validation attributes.
    /// <para>Add this interception to <c>AttachInterceptorsToRegistrations</c> autofac extionsions if using parameter validation attributes</para>
    /// </summary>
    public class ValidationInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var parameters = invocation.Method.GetParameters();
            for (var index = 0; index < parameters.Length; index++)
                foreach (ArgumentValidationAttribute attr in parameters[index].GetCustomAttributes<ArgumentValidationAttribute>(true))
                    attr.ValidateArgument(invocation.Arguments[index], parameters[index].Name);

            invocation.Proceed();
        }
    }
}
