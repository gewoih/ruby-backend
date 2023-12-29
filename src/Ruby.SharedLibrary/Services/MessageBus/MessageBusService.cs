using MassTransit;

namespace Casino.SharedLibrary.Services.MessageBus
{
	public sealed class MessageBusService : IMessageBusService
	{
		private readonly IBus _bus;

		public MessageBusService(IBus bus)
		{
			_bus = bus;
		}

		public async Task Publish<T>(T message)
		{
			await _bus.Publish(message);
		}
	}
}