using System;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Autofac.SmartInterceptors.Attributes;
using Autofac.SmartInterceptors.Interceptors;
using Xunit;

namespace Autofac.SmartInterceptors.Tests.Attributes
{
    public class NotNullAttributeTests
    {
        private readonly ITestClass _testClass;

        public NotNullAttributeTests()
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
        public void ValidParameterPass()
        {
            var result = _testClass.TestMethod("test");
            Assert.NotNull(result);
        }


        public interface ITestClass
        {
            object TestMethod([NotNull] object obj);
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
