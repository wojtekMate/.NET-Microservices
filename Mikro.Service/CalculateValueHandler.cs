using System.Threading.Tasks;
using Mikro.Messages.Commands;
using Mikro.Messages.Events;
using RawRabbit;


namespace Mikro.Service
{
    public class CalculateValueHandler : ICommandHandler<CalculateValueCommand>
    {
        private IBusClient _client;
        private ICalculate _calculate;

        public CalculateValueHandler(IBusClient client,ICalculate calculate)
        {
            _client = client;
            _calculate = calculate;

        }
        public async Task HandleAsync(CalculateValueCommand command)
        {
            int result = _calculate.calculate(command.Number);

            await _client.PublishAsync(new ValueCalculatedEvent
            {
                Number = command.Number,
                Result = result
            });
        }

        private class BasicMessage
        {
            public string Prop { get; set; }
        }
    }
}