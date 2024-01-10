#nullable disable

using N4Core.Enums;

namespace N4Core.Messages
{
	public class RecordMessages
	{
        public Language Language { get; private set; }
        public string RecordFound { get; set; }
		public string RecordsFound { get; set; }
		public string RecordNotFound { get; set; }
		public string AllRecords { get; set; }

		public RecordMessages(Language language = Language.English)
		{
			Language = language;
			RecordFound = Language == Language.Türkçe ? "kayıt bulundu." : Language == Language.English ? "record found." : "";
			RecordsFound = Language == Language.Türkçe ? "kayıt bulundu." : Language == Language.English ? "records found." : "";
			RecordNotFound = Language == Language.Türkçe ? "Kayıt bulunamadı!" : Language == Language.English ? "Record not found!" : "";
			AllRecords = Language == Language.Türkçe ? "Tümü" : Language == Language.English ? "All" : "";
		}
	}
}
