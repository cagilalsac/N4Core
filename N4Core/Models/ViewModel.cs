#nullable disable

using N4Core.Messages;

namespace N4Core.Models
{
    public class ViewModel
    {
        protected RecordMessages _recordMessages;

        public bool PageOrderFilter { get; set; }
        public int TotalRecordsCount { get; set; }
        public string TotalRecordsCountOutput
        {
            get
            {
                if (_recordMessages is null)
                {
                    return TotalRecordsCount.ToString();
                }
                return TotalRecordsCount == 0 ? _recordMessages.RecordNotFound
                    : TotalRecordsCount == 1 ? (TotalRecordsCount + " " + _recordMessages.RecordFound).ToLower()
                    : (TotalRecordsCount + " " + _recordMessages.RecordsFound).ToLower();
            }
        }
        public List<string> RecordsPerPageCounts { get; }
        public List<string> OrderExpressions { get; set; }
        public List<int> PageNumbers
        {
            get
            {
                var pageNumbers = new List<int>();
                if (TotalRecordsCount == 0 || RecordsPerPageCounts != null && RecordsPerPageCounts.Count > 0 && RecordsPerPageCount == RecordsPerPageCounts.LastOrDefault())
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
        public string RecordsPerPageCount { get; set; }
        public string OrderExpression { get; set; }
        public bool OrderDirectionDescending { get; set; }
        public string Filter { get; set; }
        public bool Modal { get; set; }
        public bool FileOperations { get; set; }
        public bool ExportOperation { get; set; }

        public ViewModel(RecordMessages recordMessages)
        {
            _recordMessages = recordMessages;
            RecordsPerPageCounts = new List<string>() { "5", "10", "25", "50", "100", _recordMessages.AllRecords };
        }
    }
}
