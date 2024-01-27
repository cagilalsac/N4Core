#nullable disable

using N4Core.Entities;
using N4Core.Records.Bases;

namespace N4Core.Models
{
    public class TreeNodeDetailModel : Record, ISoftDelete, IModifiedBy
    {
        public string TextTurkish { get; set; }
        public string TextEnglish { get; set; }
        public int Level { get; set; }
        public List<TreeNodeModel> TreeNodes { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
