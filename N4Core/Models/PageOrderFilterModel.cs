#nullable disable

namespace N4Core.Models
{
    public class PageOrderFilterModel
    {
        public int PageNumber { get; set; }
        public string RecordsPerPageCount { get; set; }
        public string OrderExpression { get; set; }
        public bool OrderDirectionDescending { get; set; }
        public string Filter { get; set; }

        public PageOrderFilterModel()
        {
            PageNumber = 1;
            RecordsPerPageCount = "10";
            OrderExpression = "";
            Filter = "";
        }
    }
}
