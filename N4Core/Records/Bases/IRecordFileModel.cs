using Microsoft.AspNetCore.Http;

namespace N4Core.Records.Bases
{
    public interface IRecordFileModel
	{
		IFormFile FormFileInput { get; set; }
		string ImgSrcOutput { get; set; }
	}
}
