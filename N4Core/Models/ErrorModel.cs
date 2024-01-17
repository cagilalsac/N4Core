#nullable disable

using N4Core.Enums;

namespace N4Core.Models
{
    public class ErrorModel
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public ErrorModel(Languages language = Languages.English)
        {
            Title = language == Languages.English ? "Error!" : "Hata!";
            Message = language == Languages.English ? "An error occurred while processing your request!" : "İşlem sırasında hata meydana geldi!";
        }

        public ErrorModel(string message, Languages language = Languages.English)
        {
            Title = language == Languages.English ? "Error!" : "Hata!";
            Message = message;
        }

        public ErrorModel(string message, string title)
        {
            Title = title;
            Message = message;
        }
    }
}
