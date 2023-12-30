#nullable disable

using N4Core.Enums;

namespace N4Core.Messages
{
	public class RecordMessages
	{
		public string RecordFound { get; set; }
		public string RecordsFound { get; set; }
		public string RecordNotFound { get; set; }
		public string AllRecords { get; set; }

		public RecordMessages(Language language = Language.English)
		{
			RecordFound = language == Language.Turkish ? "kayıt bulundu." : "record found.";
			RecordsFound = language == Language.Turkish ? "kayıt bulundu." : "records found.";
			RecordNotFound = language == Language.Turkish ? "Kayıt bulunamadı!" : "Record not found!";
			AllRecords = language == Language.Turkish ? "Tümü" : "All";
		}
	}
}
