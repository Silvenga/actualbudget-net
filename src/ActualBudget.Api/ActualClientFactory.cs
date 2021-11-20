using ActualBudget.Api.Internal;

namespace ActualBudget.Api
{
    public static class ActualClientFactory
    {
        public static IActualClient Create()
        {
            var serviceFactory = new NodeJsServiceFactory();
            var bridge = new ActualClientBridge(serviceFactory);

            return new ActualClient(bridge);
        }
    }
}