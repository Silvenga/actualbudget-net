using System;
using System.IO;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;

namespace ActualBudget.Api.Internal
{
    internal class ActualClientBridge : IDisposable
    {
        private readonly NodeJsServiceFactory _serviceFactory;
        private readonly INodeJSService _service;

        public ActualClientBridge(NodeJsServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _service = serviceFactory.Create();
        }

        public async Task Execute(string exportName, params object[] arguments)
        {
            const string cacheKey = nameof(ActualClientBridge);

            var isCached = await _service.TryInvokeFromCacheAsync(cacheKey, exportName, arguments);
            if (!isCached)
            {
                using var module = GetResourceStream("ActualBudget.Api.Internal.Bridge.Module.js");
                await _service.InvokeFromStreamAsync(module, cacheKey, exportName, arguments);
            }
        }

        public async Task<T?> Execute<T>(string exportName, params object[] arguments)
        {
            const string cacheKey = nameof(ActualClientBridge);

            var (isCached, result) = await _service.TryInvokeFromCacheAsync<T>(cacheKey, exportName, arguments);
            if (isCached)
            {
                return result;
            }

            using var module = GetResourceStream("ActualBudget.Api.Internal.Bridge.Module.js");
            return await _service.InvokeFromStreamAsync<T>(module, cacheKey, exportName, arguments);
        }

        private static Stream GetResourceStream(string name)
        {
            var assembly = typeof(ActualClientBridge).Assembly;
            var stream = assembly.GetManifestResourceStream(name)
                         ?? throw new ArgumentException(nameof(name), $"The resource '{name}' was not found.");
            return stream;
        }

        public void Dispose()
        {
            _service.Dispose();
            _serviceFactory.Dispose();
        }
    }
}