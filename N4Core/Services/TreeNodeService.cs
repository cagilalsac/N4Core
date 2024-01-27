using N4Core.Entities;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Services.Bases;

namespace N4Core.Services
{
    public class TreeNodeService : TreeNodeServiceBase
    {
        public TreeNodeService(RepoBase<TreeNode> treeNodeRepo, RepoBase<TreeNodeDetail> treeNodeDetailRepo) : base(treeNodeRepo, treeNodeDetailRepo)
        {
        }
    }
}
