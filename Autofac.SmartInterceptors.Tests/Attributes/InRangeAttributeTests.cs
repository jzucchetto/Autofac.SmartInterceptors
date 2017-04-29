using System;
using Autofac.SmartInterceptors.Attributes;
using Autofac.SmartInterceptors.Interceptors;
using Xunit;

namespace Autofac.SmartInterceptors.Tests.Attributes
{
    public class InRangeAttributeTests
    {
        private readonly ITestClass _testClass;

        public InRangeAttributeTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestClass>().As<ITestClass>().Intercept();
            builder.AttachInterceptorsToRegistrations(new ValidationInterceptor());
            var container = builder.Build();

            _testClass = container.Resolve<ITestClass>();
        }

        [Fact]
        public void ValidParameterPass()
        {
            var result = _testClass.TestMethod(2);
            Assert.Equal(result, 2);
        }

        [Fact]
        public void InvalidParameterThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _testClass.TestMethod(0));
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
