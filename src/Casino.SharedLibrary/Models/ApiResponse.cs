namespace Casino.SharedLibrary.Models
{
	public sealed class ApiResponse<T> where T : class
	{
		public bool Succeeded { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }

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
