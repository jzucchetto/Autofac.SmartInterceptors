using System;

namespace Autofac.SmartInterceptors.Attributes
{
    public class NotNullAttribute : ArgumentValidationAttribute
    {
        public override void ValidateArgument(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null");
        }
    }
}
