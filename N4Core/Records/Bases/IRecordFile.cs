#nullable disable


namespace N4Core.Records.Bases
{
    public interface IRecordFile
    {
        int Id { get; set; }
        byte[] FileData { get; set; }
        string FileContent { get; set; }
        string FilePath { get; set; }
    }
}
