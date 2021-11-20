using System;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;

namespace ActualBudget.Api.Internal
{
    internal class NodeJsServiceFactory : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public NodeJsServiceFactory()
        {
            var services = new ServiceCollection();
            services.AddNodeJS();

            services.Configure<NodeJSProcessOptions>(options => options.NodeAndV8Options = "--inspect-brk");
            services.Configure<OutOfProcessNodeJSServiceOptions>(options => options.TimeoutMS = -1);

            _serviceProvider = services.BuildServiceProvider();
        }

        public INodeJSService Create()
        {
            return _serviceProvider.GetRequiredService<INodeJSService>();
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}