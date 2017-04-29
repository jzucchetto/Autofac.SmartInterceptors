using System;

namespace Autofac.SmartInterceptors.Attributes
{
    /// <summary>
    /// Validates parameter is between a range
    /// </summary>
    public class InRangeAttribute : ArgumentValidationAttribute
    {
        private readonly long _min;
        private readonly long _max;

        public InRangeAttribute(long min, long max)
        {
            _min = min;
            _max = max;
        }

        public override void ValidateArgument(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName, $"{parameterName} can not be null");

            if (!IsNumber(value))
                throw new ArgumentException($"{parameterName} is not a number", parameterName);

            var v = (long) value;
            if (v < _min || v > _max)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} not in range {_min} to {_max}");
        }

        private static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
