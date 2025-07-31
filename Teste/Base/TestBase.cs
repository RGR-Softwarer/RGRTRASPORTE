using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Teste.Base
{
    public abstract class TestBase : IDisposable
    {
        protected readonly IServiceCollection Services;
        protected readonly IServiceProvider ServiceProvider;
        protected readonly Mock<IServiceScope> ScopeMock;

        protected TestBase()
        {
            Services = new ServiceCollection();
            ConfigureServices(Services);
            ServiceProvider = Services.BuildServiceProvider();
            
            ScopeMock = new Mock<IServiceScope>();
            ScopeMock.Setup(x => x.ServiceProvider).Returns(ServiceProvider);
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        protected T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>() ?? throw new InvalidOperationException($"Service {typeof(T).Name} not registered");
        }

        protected Mock<T> GetMock<T>() where T : class
        {
            return Services.BuildServiceProvider().GetService<Mock<T>>() ?? throw new InvalidOperationException($"Mock {typeof(T).Name} not registered");
        }

        public virtual void Dispose()
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
} 
