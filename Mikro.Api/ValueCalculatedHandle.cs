using System.Threading.Tasks;
using Mikro.Api.Repositories;
using Mikro.Messages.Events;

public class ValueCalculatedHandler : IEventHandler<ValueCalculatedEvent>
    {
        private readonly IRepository _repository;

        public ValueCalculatedHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(ValueCalculatedEvent @event)
        {
            _repository.Insert(number: @event.Number, result: @event.Result);
            await Task.CompletedTask;
        }
    }