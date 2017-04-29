using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac.SmartInterceptors.Attributes;
using Autofac.SmartInterceptors.Interceptors;
using Xunit;

namespace Autofac.SmartInterceptors.Tests.Interceptors
{
    public class LogInterceptorTests
    {
        private ITestClass _testClass;


        [Fact]
        public void TestLogInterceptor()
        {
            var output = new StringWriter();

            var builder = new ContainerBuilder();
            builder.RegisterType<TestClass>().As<ITestClass>().Intercept();
            builder.AttachInterceptorsToRegistrations(new LogInterceptor(output));
            var container = builder.Build();

            _testClass = container.Resolve<ITestClass>();
            _testClass.TestMethod(1);

            var result = output.GetStringBuilder().ToString();
            output.Dispose();

            Assert.True(result.Contains("[INFO] Autofac.SmartInterceptors.Tests.Interceptors.LogInterceptorTests+ITestClass.TestMethod(val: 1)"));
            Assert.True(result.Contains("[INFO] Autofac.SmartInterceptors.Tests.Interceptors.LogInterceptorTests+ITestClass.TestMethod Returned: 1"));
        }

        public interface ITestClass
        {
            long TestMethod([InRange(1, 5)] long val);
        }

        public class TestClass : ITestClass
        {
            public long TestMethod(long val)
            {
                return val;
            }
        }
    }
}
