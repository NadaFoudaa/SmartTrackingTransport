namespace SmartTrackingTransport.Extensions.ExceptionsHandler
{
	public class CustomApiException : Exception
	{
		public int StatusCode { get; }
		public string Content { get; }

		public CustomApiException(int statusCode, string message, string content = null)
			: base(message)
		{
			StatusCode = statusCode;
			Content = content;
		}
	}

}
