namespace N4Core.Results.Bases
{
	public abstract class ResultBase
	{
		public bool IsSuccessful { get; }
		public string Message { get; set; }

		protected ResultBase(bool isSuccessful, string message)
		{
			IsSuccessful = isSuccessful;
			Message = message;
		}
	}

	public abstract class ResultBase<TResultType> : ResultBase, IResultData<TResultType>
	{
		public TResultType Data { get; }

		protected ResultBase(bool isSuccessful, string message, TResultType data) : base(isSuccessful, message)
		{
			Data = data;
		}
	}
}
