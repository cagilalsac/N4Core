using N4Core.Culture;
using N4Core.Messages.Bases;

namespace N4Core.Views.Models
{
    public class ViewModel
    {
        public Languages Language { get; private set; }
        public RecordMessagesModel Messages { get; private set; }
        public ViewTextsModel ViewTexts { get; private set; }
        public string? Message { get; set; }
        public bool PageOrderFilter { get; set; }
        public int TotalRecordsCount { get; set; }
        public List<string>? RecordsPerPageCounts { get; }
        public List<string>? OrderExpressions { get; set; }
        public List<int> PageNumbers
        {
            get
            {
                var pageNumbers = new List<int>();
                if (TotalRecordsCount == 0 || RecordsPerPageCounts is not null && RecordsPerPageCounts.Count > 0 && RecordsPerPageCount == RecordsPerPageCounts.LastOrDefault())
                {
                    pageNumbers.Add(1);
                }
                else
                {
                    int numberOfPages = Convert.ToInt32(Math.Ceiling(TotalRecordsCount / Convert.ToDecimal(RecordsPerPageCount)));
                    for (int page = 1; page <= numberOfPages; page++)
                    {
                        pageNumbers.Add(page);
                    }
                }
                return pageNumbers;
            }
        }
        public int PageNumber { get; set; }
        public string? RecordsPerPageCount { get; set; }
        public string? OrderExpression { get; set; }
        public bool OrderDirectionDescending { get; set; }
        public string? Filter { get; set; }
        public bool? ListCards { get; set; }
        public bool Modal { get; set; }
        public bool FileOperations { get; set; }
        public bool ExportOperation { get; set; }
        public bool TimePicker { get; set; }

        public ViewModel(Languages language = Languages.English)
        {
            Language = language;
            Messages = new RecordMessagesModel(Language);
            ViewTexts = new ViewTextsModel(Language);
            RecordsPerPageCounts = new List<string>() { "5", "10", "25", "50", "100", Messages.AllRecords };
        }
    }
}
