using System;
using DependencyInjectionToolkit.DependencyInjection.Factory;
using Microsoft.Extensions.DependencyInjection;
using GeneratorTest;
using Xunit;

namespace DependencyInjection.Factory.UnitTest
{

    // A unit test class to verify the functionality of the factory pattern
    public class FactoryTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFactory<ITestService> _factory;

        public FactoryTests()
        {
            // Initialize the service collection and the factory
            var services = new ServiceCollection();
            services.InitializeFactory().RegistorServices();
            //services.AddFactory<ITestService, TestServiceA>(FactoryScope.Transient);
            //services.AddFactory<ITestService, TestServiceB>(FactoryScope.Transient);
            _serviceProvider = services.BuildServiceProvider();
            _factory = _serviceProvider.GetRequiredService<IFactory<ITestService>>();
        }

        [Fact]
        public void Factory_CanCreateTestServiceA()
        {
            // Arrange
            var expectedType = typeof(TestServiceA);

            // Act
            var service = _factory.Create<TestServiceA>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType(expectedType, service);
        }

        [Fact]
        public void Factory_CanCreateTestServiceB()
        {
            // Arrange
            var expectedType = typeof(TestServiceB);

            // Act
            var service = _factory.Create<TestServiceB>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType(expectedType, service);
        }
    }
}