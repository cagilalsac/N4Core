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
        public string Login { get; set; }
        public string Logout { get; set; }
        public string Register { get; set; }

        public ViewTexts(Language language = Language.English)
        {
            Language = language;
            List = Language == Language.English ? "List" : "Liste";
            Create = Language == Language.English ? "New" : "Yeni";
            Save = Language == Language.English ? "Save" : "Kaydet";
            Clear = Language == Language.English ? "Clear" : "Temizle";
            BackToList = Language == Language.English ? "Back to List" : "Listeye Dön";
            Details = Language == Language.English ? "Details" : "Detay";
            Edit = Language == Language.English ? "Edit" : "Düzenle";
            Delete = Language == Language.English ? "Delete" : "Sil";
            DeleteYes = Language == Language.English ? "Yes" : "Evet";
            DeleteNo = Language == Language.English ? "No" : "Hayır";
            DeleteFile = Language == Language.English ? "Delete File" : "Dosyayı Sil";
            DeleteQuestion = Language == Language.English ? "Are you sure you want to delete this record?" : "Bu kaydı silmek istediğinize emin misiniz?";
            Export = Language == Language.English ? "Export to Excel" : "Excel'e Aktar";
            Select = Language == Language.English ? "Select" : "Seçiniz";
            DownloadFile = Language == Language.English ? "Download File" : "Dosyayı İndir";
            Warning = Language == Language.English ? "Warning!" : "Uyarı!";
            Error = Language == Language.English ? "Error!" : "Hata!";
            Filter = Language == Language.English ? "Filter" : "Filtre";
            Search = Language == Language.English ? "Search" : "Ara";
            PageNumber = Language == Language.English ? "Page" : "Sayfa";
            RecordsPerPageCount = Language == Language.English ? "Record Count" : "Kayıt Sayısı";
            OrderExpression = Language == Language.English ? "Order" : "Sıra";
            OrderDirectionDescending = Language == Language.English ? "Descending" : "Azalan";
            Login = Language == Language.English ? "Log in" : "Giriş";
            Logout = Language == Language.English ? "Log out" : "Çıkış";
            Register = Language == Language.English ? "Register" : "Kayıt";
        }
    }
}
