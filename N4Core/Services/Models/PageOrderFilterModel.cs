namespace N4Core.Services.Models
{
    public class PageOrderFilterModel : PageModel
    {
        public string? OrderExpression { get; set; }
        public bool OrderDirectionDescending { get; set; }
        public string? Filter { get; set; }

        public bool? ListCards { get; set; }

        public PageOrderFilterModel()
        {
            OrderExpression = string.Empty;
            Filter = string.Empty;
        }
    }
}
