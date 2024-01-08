#nullable disable

using N4Core.Enums;

namespace N4Core.Messages
{
    public class ServiceMessages : RecordMessages
    {
        public string AddedSuccessfuly { get; set; }
        public string UpdatedSuccessfuly { get; set; }
        public string DeletedSuccessfuly { get; set; }
        public string OperationFailed { get; set; }
        public string InvalidFileExtensionOrFileLength { get; set; }
        public string RelatedRecordsFound { get; set; }
        public string RelatedRecordsDeletedSuccessfully { get; set; }
        public string FileOperationsNotConfigured { get; set; }
        public string Report { get; set; }

        public ServiceMessages(Language language = Language.English) : base(language)
        {
            AddedSuccessfuly = language == Language.Turkish ? "Kayıt başarıyla eklendi." : language == Language.English ? "Record added successfuly." : "";
            UpdatedSuccessfuly = language == Language.Turkish ? "Kayıt başarıyla güncellendi." : language == Language.English ? "Record updated successfuly." : "";
            DeletedSuccessfuly = language == Language.Turkish ? "Kayıt başarıyla silindi." : language == Language.English ? "Record deleted successfuly." : "";
            OperationFailed = language == Language.Turkish ? "İşlem gerçekleştirilemedi!" : language == Language.English ? "Operation failed!" : "";
            InvalidFileExtensionOrFileLength = language == Language.Turkish ? "Geçersiz dosya uzantısı veya boyutu!" : language == Language.English ? "Invalid file extension or length!" : "";
            RelatedRecordsFound = language == Language.Turkish ? "İlişkili kayıtlar bulundu." : language == Language.English ? "Related records found." : "";
            RelatedRecordsDeletedSuccessfully = language == Language.Turkish ? "İlişkili kayıtlar başarıyla silindi." : language == Language.English ? "Related records deleted successfully." : "";
            FileOperationsNotConfigured = language == Language.Turkish ? "Dosya işlemleri konfigüre edilmemiştir!" : language == Language.English ? "File operations is not configured!" : "";
            Report = language == Language.Turkish ? "Rapor" : language == Language.English ? "Report" : "";
        }
    }
}
