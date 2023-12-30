#nullable disable

namespace N4Core.Records.Bases
{
	public interface ISoftDelete
	{
		bool? IsDeleted { get; set; }
	}
}
