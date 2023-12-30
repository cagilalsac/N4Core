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

        public ServiceMessages(Language language = Language.English) : base(language)
        {
            AddedSuccessfuly = language == Language.Turkish ? "Kayıt başarıyla eklendi." : "Record added successfuly.";
            UpdatedSuccessfuly = language == Language.Turkish ? "Kayıt başarıyla güncellendi." : "Record updated successfuly.";
            DeletedSuccessfuly = language == Language.Turkish ? "Kayıt başarıyla silindi." : "Record deleted successfuly.";
            OperationFailed = language == Language.Turkish ? "İşlem gerçekleştirilemedi!" : "Operation failed!";
            InvalidFileExtensionOrFileLength = language == Language.Turkish ? "Geçersiz dosya uzantısı veya boyutu!" : "Invalid file extension or length!";
            RelatedRecordsFound = language == Language.Turkish ? "İlişkili kayıtlar bulundu." : "Related records found.";
            RelatedRecordsDeletedSuccessfully = language == Language.Turkish ? "İlişkili kayıtlar başarıyla silindi." : "Related records deleted successfully.";
        }
    }
}
