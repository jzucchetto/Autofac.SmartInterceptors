using System;
using System.Collections.Generic;
using Autofac.SmartInterceptors.Attributes;
using Autofac.SmartInterceptors.Interceptors;
using Xunit;

namespace Autofac.SmartInterceptors.Tests.Attributes
{
    public class NotEmptyAttributeTests
    {
        private readonly ITestClass _testClass;

        public NotEmptyAttributeTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestClass>().As<ITestClass>().Intercept();
            builder.AttachInterceptorsToRegistrations(new ValidationInterceptor());
            var container = builder.Build();

            _testClass = container.Resolve<ITestClass>();
        }

        [Fact]
        public void EmptyParameterThrow()
        {
            Assert.Throws<ArgumentException>(() => _testClass.TestMethod(new List<object>()));
        }

        [Fact]
        public void ValidParameterPass()
        {
            var param = new List<object>() {1, 2, 3};
            var result = _testClass.TestMethod(param);
            Assert.Equal(result, param);
        }

        [Fact]
        public void NullParameterThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.TestMethod(null));
        }

        [Fact]
        public void ValidReturnPass()
        {
            var result = _testClass.TestMethod(new List<object>() { 1, 2, 3 });
            Assert.NotNull(result);
        }

        public interface ITestClass
        {
            object TestMethod([NotEmpty] List<object> list);
        }

        public class TestClass : ITestClass
        {
            public object TestMethod(List<object> list)
            {
                return list;
            }
        }
    }
}
