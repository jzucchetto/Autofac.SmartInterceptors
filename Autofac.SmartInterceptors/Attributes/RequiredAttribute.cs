using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.SmartInterceptors.Attributes
{
    /// <summary>
    /// Validates parameter is not empty or null
    /// <para>If list, it will validate list contain items</para>
    /// </summary>
    public class RequiredAttribute : ArgumentValidationAttribute
    {
        public override void ValidateArgument(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null");

            if (string.IsNullOrEmpty(value.ToString()))
                throw new ArgumentException($"{parameterName} cannot be empty", parameterName);

            var v = value as IEnumerable<object>;
            if (v != null && !v.Any())
                throw new ArgumentException($"{parameterName} cannot be empty", parameterName);
        }
    }
}
