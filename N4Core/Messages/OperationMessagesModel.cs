using N4Core.Culture;
using N4Core.Messages.Bases;

namespace N4Core.Messages
{
    public class OperationMessagesModel : RecordMessagesModel
    {
        public string CreatedSuccessfuly { get; set; }
        public string UpdatedSuccessfuly { get; set; }
        public string DeletedSuccessfuly { get; set; }
        public string OperationFailed { get; set; }
        public string RecordWithSameNameExists { get; set; }
        public string InvalidFileExtensionOrFileLength { get; set; }
        public string RelatedRecordsFound { get; set; }
        public string RelatedRecordsDeletedSuccessfully { get; set; }
        public string FileDeletedSuccessfully { get; set; }
        public string FileOperationsNotConfigured { get; set; }
        public string RequestMethodNotConfigured { get; set; }
        public string Report { get; set; }

        public OperationMessagesModel(Languages language = Languages.English) : base(language)
        {
            CreatedSuccessfuly = language == Languages.Türkçe ? "Kayıt başarıyla eklendi." : "Record added successfuly.";
            UpdatedSuccessfuly = language == Languages.Türkçe ? "Kayıt başarıyla güncellendi." : "Record updated successfuly.";
            DeletedSuccessfuly = language == Languages.Türkçe ? "Kayıt başarıyla silindi." : "Record deleted successfuly.";
            OperationFailed = language == Languages.Türkçe ? "İşlem gerçekleştirilemedi!" : "Operation failed!";
            RecordWithSameNameExists = OperationFailed + " " + (language == Languages.Türkçe ? "Aynı ada sahip kayıt bulundu!" : "Record found with the same name!");
            InvalidFileExtensionOrFileLength = OperationFailed + " " + (language == Languages.Türkçe ? "Geçersiz dosya uzantısı veya boyutu!" : "Invalid file extension or length!");
            RelatedRecordsFound = OperationFailed + " " + (language == Languages.Türkçe ? "İlişkili kayıtlar bulundu!" : "Related records found!");
            RelatedRecordsDeletedSuccessfully = language == Languages.Türkçe ? "İlişkili kayıtlar başarıyla silindi." : "Related records deleted successfully.";
            FileDeletedSuccessfully = language == Languages.Türkçe ? "Dosya başarıyla silindi." : "File deleted successfully.";
            FileOperationsNotConfigured = OperationFailed + " " + (language == Languages.Türkçe ? "Dosya işlemleri konfigüre edilmemiştir!" : "File operations is not configured!");
            RequestMethodNotConfigured = OperationFailed + " " + (language == Languages.Türkçe ? "İstek methodu konfigüre edilmemiştir!" : "Request method is not configured!");
            Report = language == Languages.Türkçe ? "Rapor" : "Report";
        }
    }
}
