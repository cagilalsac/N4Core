﻿#nullable disable

using N4Core.Enums;
using N4Core.Messages;
using N4Core.Texts;

namespace N4Core.Models
{
    public class ViewModel
    {
        public Language Language { get; private set; }
        public RecordMessages RecordMessages { get; private set; }
        public ViewTexts ViewTexts { get; private set; }

        public bool PageOrderFilter { get; set; }
        public int TotalRecordsCount { get; set; }
        public string TotalRecordsCountOutput
        {
            get
            {
                if (RecordMessages is null)
                {
                    return TotalRecordsCount.ToString();
                }
                return TotalRecordsCount == 0 ? RecordMessages.RecordNotFound
                    : TotalRecordsCount == 1 ? (TotalRecordsCount + " " + RecordMessages.RecordFound).ToLower()
                    : (TotalRecordsCount + " " + RecordMessages.RecordsFound).ToLower();
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
        public bool TimePicker { get; set; }

        public ViewModel(Language language = Language.English)
        {
            Language = language;
            RecordMessages = new ServiceMessages(Language);
            ViewTexts = new ViewTexts(Language);
            RecordsPerPageCounts = new List<string>() { "5", "10", "25", "50", "100", RecordMessages.AllRecords };
        }
    }
}
