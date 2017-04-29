using System;
using System.Linq;
using System.Reflection;
using Autofac.Builder;
using Autofac.Core;
using Castle.DynamicProxy;

// ReSharper disable once CheckNamespace
namespace Autofac
{
    public static class AutofacExtensions
    {
        /// <summary>
        /// This will mark this registration to be intercepted by interceptors registered with AttachInterceptorsToRegistrations
        /// </summary>
        /// <param name="builder"></param>
        public static void Intercept<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            if (builder.RegistrationData.Services.OfType<IServiceWithType>().Any(swt => !swt.ServiceType.GetTypeInfo().IsInterface || !swt.ServiceType.GetTypeInfo().IsVisible))
                throw new Exception("Only public interfaces intercept allowed");

            builder.WithMetadata("SmartInterceptors", true);
        }

        // DynamicProxy2 generator for creating proxies
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        /// <summary>
        /// Attached the interceptors to all registrations that are marked to be intercepted. ***Note this has to be the last registration before builder.Build()***
        /// </summary>
        /// <param name="builder">Contained builder to apply interceptions to.</param>
        /// <param name="interceptors">List of interceptors to apply.</param>
        public static void AttachInterceptorsToRegistrations(this ContainerBuilder builder, params IInterceptor[] interceptors)
        {
            builder.RegisterCallback((componentRegistry) =>
            {
                foreach (var registration in componentRegistry.Registrations.Where(x => x.Metadata.ContainsKey("SmartInterceptors")))
                {
                    InterceptRegistration(registration, interceptors);
                }
            });
        }

        /// <summary>
        /// Intercept a specific component registrations.
        /// </summary>
        /// <param name="registration">Component registration</param>
        /// <param name="interceptors">List of interceptors to apply.</param>
        private static void InterceptRegistration(IComponentRegistration registration, params IInterceptor[] interceptors)
        {
            registration.Activating += (sender, e) =>
            {
                var type = e.Instance.GetType();

                if (e.Component.Services.OfType<IServiceWithType>().Any(swt => !swt.ServiceType.GetTypeInfo().IsInterface || !swt.ServiceType.GetTypeInfo().IsVisible) ||
                    // prevent proxying the proxy 
                    type.Namespace == "Castle.Proxies")
                {
                    return;
                }

                var proxiedInterfaces = type.GetInterfaces().Where(i => i.GetTypeInfo().IsVisible).ToArray();

                if (!proxiedInterfaces.Any())
                {
                    return;
                }

                // intercept with all interceptors
                var theInterface = proxiedInterfaces.First();
                var interfaces = proxiedInterfaces.Skip(1).ToArray();

                e.Instance = Generator.CreateInterfaceProxyWithTarget(theInterface, interfaces, e.Instance, interceptors);
            };
        }
    }
}
