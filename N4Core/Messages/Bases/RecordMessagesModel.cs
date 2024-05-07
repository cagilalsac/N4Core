using N4Core.Culture;

namespace N4Core.Messages.Bases
{
    public class RecordMessagesModel
    {
        public Languages Language { get; private set; }
        public string RecordFound { get; set; }
        public string RecordsFound { get; set; }
        public string RecordNotFound { get; set; }
        public string AllRecords { get; set; }

        public RecordMessagesModel(Languages language = Languages.English)
        {
            Language = language;
            RecordFound = Language == Languages.Türkçe ? "kayıt bulundu." : "record found.";
            RecordsFound = Language == Languages.Türkçe ? "kayıt bulundu." : "records found.";
            RecordNotFound = Language == Languages.Türkçe ? "Kayıt bulunamadı!" : "Record not found!";
            AllRecords = Language == Languages.Türkçe ? "Tümü" : "All";
        }
    }
}
