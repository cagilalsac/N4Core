#nullable disable

namespace N4Core.Models
{
    public class TreeNodeRecursiveModel : TreeNodeModel
    {
        public List<TreeNodeRecursiveModel> Nodes { get; set; }
    }
}
