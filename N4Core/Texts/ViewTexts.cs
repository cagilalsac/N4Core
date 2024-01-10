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
            List = Language == Language.English ? "List" : Language == Language.Türkçe ? "Liste" : "";
            Create = Language == Language.English ? "New" : Language == Language.Türkçe ? "Yeni" : "";
            Save = Language == Language.English ? "Save" : Language == Language.Türkçe ? "Kaydet" : "";
            Clear = Language == Language.English ? "Clear" : Language == Language.Türkçe ? "Temizle" : "";
            BackToList = Language == Language.English ? "Back to List" : Language == Language.Türkçe ? "Listeye Dön" : "";
            Details = Language == Language.English ? "Details" : Language == Language.Türkçe ? "Detay" : "";
            Edit = Language == Language.English ? "Edit" : Language == Language.Türkçe ? "Düzenle" : "";
            Delete = Language == Language.English ? "Delete" : Language == Language.Türkçe ? "Sil" : "";
            DeleteYes = Language == Language.English ? "Yes" : Language == Language.Türkçe ? "Evet" : "";
            DeleteNo = Language == Language.English ? "No" : Language == Language.Türkçe ? "Hayır" : "";
            DeleteFile = Language == Language.English ? "Delete File" : Language == Language.Türkçe ? "Dosyayı Sil" : "";
            DeleteQuestion = Language == Language.English ? "Are you sure you want to delete this record?" : Language == Language.Türkçe ? "Bu kaydı silmek istediğinize emin misiniz?" : "";
            Export = Language == Language.English ? "Export to Excel" : Language == Language.Türkçe ? "Excel'e Aktar" : "";
            Select = Language == Language.English ? "Select" : Language == Language.Türkçe ? "Seçiniz" : "";
            DownloadFile = Language == Language.English ? "Download File" : Language == Language.Türkçe ? "Dosyayı İndir" : "";
            Warning = Language == Language.English ? "Warning!" : Language == Language.Türkçe ? "Uyarı!" : "";
            Error = Language == Language.English ? "Error!" : Language == Language.Türkçe ? "Hata!" : "";
            Filter = Language == Language.English ? "Filter" : Language == Language.Türkçe ? "Filtre" : "";
            Search = Language == Language.English ? "Search" : Language == Language.Türkçe ? "Ara" : "";
            PageNumber = Language == Language.English ? "Page" : Language == Language.Türkçe ? "Sayfa" : "";
            RecordsPerPageCount = Language == Language.English ? "Record Count" : Language == Language.Türkçe ? "Kayıt Sayısı" : "";
            OrderExpression = Language == Language.English ? "Order" : Language == Language.Türkçe ? "Sıra" : "";
            OrderDirectionDescending = Language == Language.English ? "Descending" : Language == Language.Türkçe ? "Azalan" : "";
        }
    }
}
