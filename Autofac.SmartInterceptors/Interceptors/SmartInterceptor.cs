using System;
using Castle.DynamicProxy;

namespace Autofac.SmartInterceptors.Interceptors
{
    /// <summary>
    /// Interceptor that will call <c>beforeMethodAction</c> before method is executed and call <c>afterMethodAction</c> after method is executed.
    /// <para>This is a good interceptor for logging</para>
    /// </summary>
    public class SmartInterceptor : IInterceptor
    {
        private readonly Action<IInvocation> _beforeMethodAction;
        private readonly Action<IInvocation> _afterMethodAction;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="beforeMethodAction">If specified, it will be called before method execution</param>
        /// <param name="afterMethodAction">If specified, it will be called after method execution</param>
        public SmartInterceptor(Action<IInvocation> beforeMethodAction = null, Action<IInvocation> afterMethodAction = null)
        {
            _beforeMethodAction = beforeMethodAction;
            _afterMethodAction = afterMethodAction;
        }

        public void Intercept(IInvocation invocation)
        {
            _beforeMethodAction?.Invoke(invocation);

            invocation.Proceed();

            _afterMethodAction?.Invoke(invocation);
        }
    }
}