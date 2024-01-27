#nullable disable

using N4Core.Records.Bases;

namespace N4Core.Entities
{
    public class TreeNodeDetail : Record, ISoftDelete, IModifiedBy
    {
        public string TextTurkish { get; set; }
        public string TextEnglish { get; set; }
        public int Level { get; set; }
        public List<TreeNode> TreeNodes { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
