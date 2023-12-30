namespace N4Core.Results.Bases
{
	public interface IResultData<out TResultType>
	{
		TResultType Data { get; }
	}
}
