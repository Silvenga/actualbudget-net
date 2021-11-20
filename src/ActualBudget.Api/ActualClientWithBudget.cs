using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActualBudget.Api.Internal;

namespace ActualBudget.Api
{
    public interface IActualClientWithBudget : IAsyncDisposable
    {
        string BudgetId { get; }
        Task OpenAsync();
        Task CloseAsync(bool hasError = false);
        Task<IReadOnlyList<DateTime>> GetBudgetMonthsAsync();
    }

    public class ActualClientWithBudget : IActualClientWithBudget
    {
        private readonly ActualClientBridge _bridge;

        public string BudgetId { get; }
        public bool IsOpen { get; private set; }

        internal ActualClientWithBudget(string budgetId, ActualClientBridge bridge)
        {
            BudgetId = budgetId;
            _bridge = bridge;
        }

        public async Task OpenAsync()
        {
            await _bridge.Execute("init");
            await _bridge.Execute("loadBudget", BudgetId);
            IsOpen = true;
        }

        public async Task<IReadOnlyList<DateTime>> GetBudgetMonthsAsync()
        {
            return await _bridge.Execute<IReadOnlyList<DateTime>>("getBudgetMonth")
                   ?? Array.Empty<DateTime>();
        }

        public async Task GetBudgetMonthAsync(DateTime dateTime)
        {
            await _bridge.Execute("getBudgetMonth", $"{dateTime:yyyy-MM}");
        }

        public async Task CloseAsync(bool hasError = false)
        {
            if (!IsOpen)
            {
                throw new ArgumentException("Client is already closed.");
            }

            await _bridge.Execute("send", "api/cleanup", new
            {
                hasError
            });
            await _bridge.Execute("disconnect");
            IsOpen = false;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsOpen)
            {
                await CloseAsync();
            }
        }
    }
}