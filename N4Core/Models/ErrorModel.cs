#nullable disable

using N4Core.Enums;

namespace N4Core.Models
{
    public class ErrorModel
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public ErrorModel(Language language = Language.English)
        {
            Title = language == Language.English ? "Error!" : language == Language.Turkish ? "Hata!" : "";
            Message = language == Language.English ? "An error occurred while processing your request!" : language == Language.Turkish ? "İşlem sırasında hata meydana geldi!" : "";
        }

        public ErrorModel(string message, Language language = Language.English)
        {
            Title = language == Language.English ? "Error!" : language == Language.Turkish ? "Hata!" : "";
            Message = message;
        }
    }
}
