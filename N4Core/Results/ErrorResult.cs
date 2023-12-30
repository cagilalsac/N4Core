#nullable disable

using N4Core.Results.Bases;

namespace N4Core.Results
{
	public class ErrorResult : ResultBase
	{
		public ErrorResult(string message) : base(false, message)
		{

		}

		public ErrorResult() : base(false, "")
		{

		}
	}

	public class ErrorResult<TResultType> : ResultBase<TResultType>
	{
		public ErrorResult(string message, TResultType data) : base(false, message, data)
		{

		}

		public ErrorResult(string message) : base(false, message, default)
		{

		}

		public ErrorResult(TResultType data) : base(false, "", data)
		{

		}

		public ErrorResult() : base(false, "", default)
		{

		}
	}
}
