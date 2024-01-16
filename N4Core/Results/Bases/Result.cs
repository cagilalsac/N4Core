namespace N4Core.Results.Bases
{
	public abstract class Result
	{
		public bool IsSuccessful { get; }
		public string Message { get; set; }

		protected Result(bool isSuccessful, string message)
		{
			IsSuccessful = isSuccessful;
			Message = message;
		}
	}

	public abstract class Result<TResultType> : Result, IResultData<TResultType>
	{
		public TResultType Data { get; }

		protected Result(bool isSuccessful, string message, TResultType data) : base(isSuccessful, message)
		{
			Data = data;
		}
	}
}
