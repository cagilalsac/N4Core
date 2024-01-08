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
			RecordFound = Language == Language.Turkish ? "kayıt bulundu." : Language == Language.English ? "record found." : "";
			RecordsFound = Language == Language.Turkish ? "kayıt bulundu." : Language == Language.English ? "records found." : "";
			RecordNotFound = Language == Language.Turkish ? "Kayıt bulunamadı!" : Language == Language.English ? "Record not found!" : "";
			AllRecords = Language == Language.Turkish ? "Tümü" : Language == Language.English ? "All" : "";
		}
	}
}
