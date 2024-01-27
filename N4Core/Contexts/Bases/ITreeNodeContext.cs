using Microsoft.EntityFrameworkCore;
using N4Core.Entities;

namespace N4Core.Contexts.Bases
{
    public interface ITreeNodeContext
    {
        DbSet<TreeNode> TreeNodes { get; set; }
        DbSet<TreeNodeDetail> TreeNodeDetails { get; set; }
    }
}
