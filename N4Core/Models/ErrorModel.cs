#nullable disable

using N4Core.Enums;

namespace N4Core.Models
{
    public class ErrorModel
    {
        public Language Language { get; private set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public ErrorModel(Language language = Language.English)
        {
            Language = language;
            Title = Language == Language.English ? "Error!" : "Hata!";
            Message = Language == Language.English ? "An error occurred while processing your request!" : "İşlem sırasında hata meydana geldi!";
        }

        public ErrorModel(string message, Language language = Language.English)
        {
            Language = language;
            Title = Language == Language.English ? "Error!" : "Hata!";
            Message = message;
        }
    }
}
