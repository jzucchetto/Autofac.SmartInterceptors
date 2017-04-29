using System;
using Autofac.SmartInterceptors.Attributes;
using Autofac.SmartInterceptors.Interceptors;
using Xunit;

namespace Autofac.SmartInterceptors.Tests.Attributes
{
    public class RequiredAttributeTests
    {
        private readonly ITestClass _testClass;

        public RequiredAttributeTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestClass>().As<ITestClass>().Intercept();
            builder.AttachInterceptorsToRegistrations(new ValidationInterceptor());
            var container = builder.Build();

            _testClass = container.Resolve<ITestClass>();
        }

        [Fact]
        public void NullParameterThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.TestMethod(null));
        }

        [Fact]
        public void EmptyParameterThrow()
        {
            Assert.Throws<ArgumentException>(() => _testClass.TestMethod(string.Empty));
        }

        [Fact]
        public void ValidReturnPass()
        {
            var result = _testClass.TestMethod("test");
            Assert.NotNull(result);
        }


        public interface ITestClass
        {
            object TestMethod([Required] object obj);
        }

        public class TestClass : ITestClass
        {
            public object TestMethod(object obj)
            {
                return obj;
            }
        }
    }
}
