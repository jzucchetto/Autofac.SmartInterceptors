using System;
using System.IO;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Autofac.SmartInterceptors.Interceptors
{
    /// <summary>
    /// Interceptor that will log method calls.
    /// <para>This is a good interceptor for logging</para>
    /// </summary>
    public class LogInterceptor : IInterceptor
    {
        private readonly TextWriter _output;

        public LogInterceptor(TextWriter output)
        {
            _output = output;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType}.{invocation.Method.Name}";
            var sb = new StringBuilder();

            sb.Append($"[{DateTime.Now:HH:mm:ss}] [INFO] {methodName}");
            if (invocation.Arguments.Length > 0)
            {
                var parameters = invocation.Method.GetParameters();
                for (var index = 0; index < invocation.Arguments.Length; index++)
                {
                    if (index == 0)
                        sb.Append("(");

                    var arg = invocation.Arguments[index];
                    var val = arg is string ? $"\"{arg}\"" : arg.ToString();
                    sb.Append($"{parameters[index].Name}: {val}");

                    if (index < invocation.Arguments.Length - 1)
                        sb.Append(", ");
                    else
                        sb.Append(")");
                }
            }
            _output.WriteLine(sb.ToString());

            invocation.Proceed();
            
            sb = new StringBuilder();
            sb.Append($"[{DateTime.Now:HH:mm:ss}] [INFO] {methodName}");
            if (invocation.ReturnValue != null)
                sb.Append($" Returned: {invocation.ReturnValue}");
            else
                sb.Append(" Returned: void");
            _output.WriteLine(sb.ToString());
        }
    }
}