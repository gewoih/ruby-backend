namespace Casino.SharedLibrary.Models
{
	public sealed class ApiResponse<T> where T : class
	{
		public bool Succeeded { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }

		public static ApiResponse<T> Success(T data)
		{
			return new ApiResponse<T>
			{
				Succeeded = true,
				Data = data
			};
		}

		public static ApiResponse<T> Error(string message)
		{
			return new ApiResponse<T>
			{
				Succeeded = false,
				Message = message
			};
		}
	}
}
