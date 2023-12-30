#nullable disable

using N4Core.Results.Bases;

namespace N4Core.Results
{
	public class SuccessResult : ResultBase
	{
		public SuccessResult(string message) : base(true, message)
		{

		}

		public SuccessResult() : base(true, "")
		{

		}
	}

	public class SuccessResult<TResultType> : ResultBase<TResultType>
	{
		public SuccessResult(string message, TResultType data) : base(true, message, data)
		{

		}

		public SuccessResult(string message) : base(true, message, default)
		{

		}

		public SuccessResult(TResultType data) : base(true, "", data)
		{

		}

		public SuccessResult() : base(true, "", default)
		{

		}
	}
}
