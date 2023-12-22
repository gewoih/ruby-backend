namespace Casino.SharedLibrary.Models
{
	public sealed class ApiResponse<T> where T : class
	{
		public bool Succeeded { get; private set; }
		public string Message { get; private set; }
		public T Data { get; private set; }

		public ApiResponse<T> Success(T data)
		{
			Succeeded = true;
			Data = data;

			return this;
		}

		public ApiResponse<T> Error(string message)
		{
			Succeeded = false;
			Message = message;

			return this;
		}
	}
}
