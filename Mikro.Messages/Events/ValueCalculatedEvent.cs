namespace Mikro.Messages.Events
{
    public class ValueCalculatedEvent : IEvent
    {
        public int Number { get; set; }
        public int Result { get; set; }
    }
}