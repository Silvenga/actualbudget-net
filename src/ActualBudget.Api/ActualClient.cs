using System;
using System.Threading.Tasks;
using ActualBudget.Api.Internal;

namespace ActualBudget.Api
{
    public interface IActualClient : IDisposable
    {
        Task<IActualClientWithBudget> OpenAsync(string budgetId);
    }

    public class ActualClient : IActualClient
    {
        private readonly ActualClientBridge _bridge;

        internal ActualClient(ActualClientBridge bridge)
        {
            _bridge = bridge;
        }

        public async Task<IActualClientWithBudget> OpenAsync(string budgetId)
        {
            var clientWithBudget = new ActualClientWithBudget(budgetId, _bridge);
            await clientWithBudget.OpenAsync();
            return clientWithBudget;
        }

        public Task RunWithBudget(string budgetId, Func<Task> func)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _bridge.Dispose();
        }
    }
}