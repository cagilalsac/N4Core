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
            AddedSuccessfuly = language == Language.Türkçe ? "Kayıt başarıyla eklendi." : language == Language.English ? "Record added successfuly." : "";
            UpdatedSuccessfuly = language == Language.Türkçe ? "Kayıt başarıyla güncellendi." : language == Language.English ? "Record updated successfuly." : "";
            DeletedSuccessfuly = language == Language.Türkçe ? "Kayıt başarıyla silindi." : language == Language.English ? "Record deleted successfuly." : "";
            OperationFailed = language == Language.Türkçe ? "İşlem gerçekleştirilemedi!" : language == Language.English ? "Operation failed!" : "";
            InvalidFileExtensionOrFileLength = language == Language.Türkçe ? "Geçersiz dosya uzantısı veya boyutu!" : language == Language.English ? "Invalid file extension or length!" : "";
            RelatedRecordsFound = language == Language.Türkçe ? "İlişkili kayıtlar bulundu." : language == Language.English ? "Related records found." : "";
            RelatedRecordsDeletedSuccessfully = language == Language.Türkçe ? "İlişkili kayıtlar başarıyla silindi." : language == Language.English ? "Related records deleted successfully." : "";
            FileOperationsNotConfigured = language == Language.Türkçe ? "Dosya işlemleri konfigüre edilmemiştir!" : language == Language.English ? "File operations is not configured!" : "";
            Report = language == Language.Türkçe ? "Rapor" : language == Language.English ? "Report" : "";
        }
    }
}
