#nullable disable

using N4Core.Enums;

namespace N4Core.Texts
{
    public class ViewTexts
    {
        public Language Language { get; private set; }
        public string List { get; set; }
        public string Create { get; set; }
        public string Save { get; set; }
        public string Clear { get; set; }
        public string BackToList { get; set; }
        public string Details { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string DeleteYes { get; set; }
        public string DeleteNo { get; set; }
        public string DeleteFile { get; set; }
        public string DeleteQuestion { get; set; }
        public string Export { get; set; }
        public string Select { get; set; }
        public string DownloadFile { get; set; }
        public string Warning { get; set; }
        public string Error { get; set; }
        public string Filter { get; set; }
        public string Search { get; set; }
        public string PageNumber { get; set; }
        public string RecordsPerPageCount { get; set; }
        public string OrderExpression { get; set; }
        public string OrderDirectionDescending { get; set; }

        public ViewTexts(Language language = Language.English)
        {
            Language = language;
            List = Language == Language.English ? "List" : Language == Language.Turkish ? "Liste" : "";
            Create = Language == Language.English ? "New" : Language == Language.Turkish ? "Yeni" : "";
            Save = Language == Language.English ? "Save" : Language == Language.Turkish ? "Kaydet" : "";
            Clear = Language == Language.English ? "Clear" : Language == Language.Turkish ? "Temizle" : "";
            BackToList = Language == Language.English ? "Back to List" : Language == Language.Turkish ? "Listeye Dön" : "";
            Details = Language == Language.English ? "Details" : Language == Language.Turkish ? "Detay" : "";
            Edit = Language == Language.English ? "Edit" : Language == Language.Turkish ? "Düzenle" : "";
            Delete = Language == Language.English ? "Delete" : Language == Language.Turkish ? "Sil" : "";
            DeleteYes = Language == Language.English ? "Yes" : Language == Language.Turkish ? "Evet" : "";
            DeleteNo = Language == Language.English ? "No" : Language == Language.Turkish ? "Hayır" : "";
            DeleteFile = Language == Language.English ? "Delete File" : Language == Language.Turkish ? "Dosyayı Sil" : "";
            DeleteQuestion = Language == Language.English ? "Are you sure you want to delete this record?" : Language == Language.Turkish ? "Bu kaydı silmek istediğinize emin misiniz?" : "";
            Export = Language == Language.English ? "Export to Excel" : Language == Language.Turkish ? "Excel'e Aktar" : "";
            Select = Language == Language.English ? "Select" : Language == Language.Turkish ? "Seçiniz" : "";
            DownloadFile = Language == Language.English ? "Download File" : Language == Language.Turkish ? "Dosyayı İndir" : "";
            Warning = Language == Language.English ? "Warning!" : Language == Language.Turkish ? "Uyarı!" : "";
            Error = Language == Language.English ? "Error!" : Language == Language.Turkish ? "Hata!" : "";
            Filter = Language == Language.English ? "Filter" : Language == Language.Turkish ? "Filtre" : "";
            Search = Language == Language.English ? "Search" : Language == Language.Turkish ? "Ara" : "";
            PageNumber = Language == Language.English ? "Page" : Language == Language.Turkish ? "Sayfa" : "";
            RecordsPerPageCount = Language == Language.English ? "Record Count" : Language == Language.Turkish ? "Kayıt Sayısı" : "";
            OrderExpression = Language == Language.English ? "Order" : Language == Language.Turkish ? "Sıra" : "";
            OrderDirectionDescending = Language == Language.English ? "Descending" : Language == Language.Turkish ? "Azalan" : "";
        }
    }
}
