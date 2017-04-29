using System;

namespace Autofac.SmartInterceptors.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ArgumentValidationAttribute : Attribute
    {
        public abstract void ValidateArgument(object value, string parameterName);
    }
}
