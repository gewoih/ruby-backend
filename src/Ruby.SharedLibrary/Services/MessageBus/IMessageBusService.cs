namespace Casino.SharedLibrary.Services.MessageBus
{
	public interface IMessageBusService
	{
		Task Publish<T>(T message);
	}
}