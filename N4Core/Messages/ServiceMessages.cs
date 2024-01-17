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

        public ServiceMessages(Languages language = Languages.English) : base(language)
        {
            AddedSuccessfuly = language == Languages.Türkçe ? "Kayıt başarıyla eklendi." : "Record added successfuly.";
            UpdatedSuccessfuly = language == Languages.Türkçe ? "Kayıt başarıyla güncellendi." : "Record updated successfuly.";
            DeletedSuccessfuly = language == Languages.Türkçe ? "Kayıt başarıyla silindi." : "Record deleted successfuly.";
            OperationFailed = language == Languages.Türkçe ? "İşlem gerçekleştirilemedi!" : "Operation failed!";
            InvalidFileExtensionOrFileLength = language == Languages.Türkçe ? "Geçersiz dosya uzantısı veya boyutu!" : "Invalid file extension or length!";
            RelatedRecordsFound = language == Languages.Türkçe ? "İlişkili kayıtlar bulundu." : "Related records found.";
            RelatedRecordsDeletedSuccessfully = language == Languages.Türkçe ? "İlişkili kayıtlar başarıyla silindi." : "Related records deleted successfully.";
            FileOperationsNotConfigured = language == Languages.Türkçe ? "Dosya işlemleri konfigüre edilmemiştir!" : "File operations is not configured!";
            Report = language == Languages.Türkçe ? "Rapor" : "Report";
        }
    }
}
