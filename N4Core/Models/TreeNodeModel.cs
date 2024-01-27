#nullable disable

using N4Core.Entities;
using N4Core.Records.Bases;

namespace N4Core.Models
{
    public class TreeNodeModel : Record, ISoftDelete, IModifiedBy
    {
        public int ParentId { get; set; }
        public string NameTurkish { get; set; }
        public string NameEnglish { get; set; }
        public string TextTurkish { get; set; }
        public string TextEnglish { get; set; }
        public string AbbreviationTurkish { get; set; }
        public string AbbreviationEnglish { get; set; }
        public bool IsActive { get; set; }
        public int TreeNodeDetailId { get; set; }
        public TreeNodeDetailModel TreeNodeDetail { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
