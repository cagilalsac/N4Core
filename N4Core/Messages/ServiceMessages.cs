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
            AddedSuccessfuly = language == Language.Türkçe ? "Kayıt başarıyla eklendi." : "Record added successfuly.";
            UpdatedSuccessfuly = language == Language.Türkçe ? "Kayıt başarıyla güncellendi." : "Record updated successfuly.";
            DeletedSuccessfuly = language == Language.Türkçe ? "Kayıt başarıyla silindi." : "Record deleted successfuly.";
            OperationFailed = language == Language.Türkçe ? "İşlem gerçekleştirilemedi!" : "Operation failed!";
            InvalidFileExtensionOrFileLength = language == Language.Türkçe ? "Geçersiz dosya uzantısı veya boyutu!" : "Invalid file extension or length!";
            RelatedRecordsFound = language == Language.Türkçe ? "İlişkili kayıtlar bulundu." : "Related records found.";
            RelatedRecordsDeletedSuccessfully = language == Language.Türkçe ? "İlişkili kayıtlar başarıyla silindi." : "Related records deleted successfully.";
            FileOperationsNotConfigured = language == Language.Türkçe ? "Dosya işlemleri konfigüre edilmemiştir!" : "File operations is not configured!";
            Report = language == Language.Türkçe ? "Rapor" : "Report";
        }
    }
}
