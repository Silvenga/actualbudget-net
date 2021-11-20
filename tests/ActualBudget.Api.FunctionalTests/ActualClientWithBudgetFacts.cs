using System;
using System.Threading.Tasks;
using Xunit;

namespace ActualBudget.Api.FunctionalTests
{
    public class ActualClientWithBudgetFacts : IClassFixture<ActualClientWithBudgetFacts.ActualClientFixture>
    {
        private readonly ActualClientFixture _fixture;

        public IActualClient Client => _fixture.GetClient();

        public ActualClientWithBudgetFacts(ActualClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test1()
        {
            await using var clientWithBudget = await Client.OpenAsync("My-Finances-1-dde9556");
            var list = await clientWithBudget.GetBudgetMonthsAsync();
            foreach (var dateTime in list)
            {
                Console.WriteLine(dateTime);
            }
        }

        public class ActualClientFixture : IDisposable
        {
            private IActualClient? _client;

            public IActualClient GetClient()
            {
                return _client ??= ActualClientFactory.Create();
            }

            public void Dispose()
            {
                _client?.Dispose();
            }
        }
    }
}