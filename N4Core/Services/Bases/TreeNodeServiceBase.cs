#nullable disable

using LinqKit;
using Microsoft.EntityFrameworkCore;
using N4Core.Configurations;
using N4Core.Entities;
using N4Core.Enums;
using N4Core.Messages;
using N4Core.Models;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results;
using N4Core.Results.Bases;
using System.Linq.Expressions;

namespace N4Core.Services.Bases
{
    public abstract class TreeNodeServiceBase : IService<TreeNodeModel>
    {
        public TreeNodeServiceConfig Config { get; private set; }
        public TreeNodeServiceMessages Messages { get; private set; }

        protected readonly RepoBase<TreeNode> _treeNodeRepo;
        protected readonly RepoBase<TreeNodeDetail> _treeNodeDetailRepo;

        protected List<TreeNodeModel> _nodes;

        protected TreeNodeServiceBase(RepoBase<TreeNode> treeNodeRepo, RepoBase<TreeNodeDetail> treeNodeDetailRepo)
        {
            _treeNodeRepo = treeNodeRepo;
            _treeNodeDetailRepo = treeNodeDetailRepo;
            _nodes = Query().ToList();
            Config = new TreeNodeServiceConfig();
            Messages = new TreeNodeServiceMessages();
        }

        public void Set(Action<TreeNodeServiceConfig> config)
        {
            config.Invoke(Config);
            Messages = new TreeNodeServiceMessages(Config.Language);
        }

        public virtual IQueryable<TreeNodeModel> Query()
        {
            return _treeNodeRepo.Query().Include(t => t.TreeNodeDetail).Select(t => new TreeNodeModel()
            {
                Id = t.Id,
                Guid = t.Guid,
                ParentId = t.ParentId,
                AbbreviationEnglish = t.AbbreviationEnglish,
                AbbreviationTurkish = t.AbbreviationTurkish,
                TextEnglish = t.TextEnglish,
                TextTurkish = t.TextTurkish,
                NameEnglish = t.NameEnglish,
                NameTurkish = t.NameTurkish,
                IsActive = t.IsActive,
                IsDeleted = t.IsDeleted,
                CreateDate = t.CreateDate,
                CreatedBy = t.CreatedBy,
                UpdateDate = t.UpdateDate,
                UpdatedBy = t.UpdatedBy,
                TreeNodeDetailId = t.TreeNodeDetailId,
                TreeNodeDetail = new TreeNodeDetailModel()
                {
                    Id = t.TreeNodeDetail.Id,
                    Guid = t.TreeNodeDetail.Guid,
                    TextEnglish = t.TreeNodeDetail.TextEnglish,
                    TextTurkish = t.TreeNodeDetail.TextTurkish,
                    Level = t.TreeNodeDetail.Level,
                    IsDeleted = t.TreeNodeDetail.IsDeleted,
                    CreateDate = t.TreeNodeDetail.CreateDate,
                    CreatedBy = t.TreeNodeDetail.CreatedBy,
                    UpdateDate = t.TreeNodeDetail.UpdateDate,
                    UpdatedBy = t.TreeNodeDetail.UpdatedBy
                }
            });
        }

        public virtual IQueryable<TreeNodeDetailModel> GetDetailNodesQuery()
        {
            return _treeNodeDetailRepo.Query().Include(d => d.TreeNodes).Select(d => new TreeNodeDetailModel()
            {
                Id = d.Id,
                Guid = d.Guid,
                TextEnglish = d.TextEnglish,
                TextTurkish = d.TextTurkish,
                Level = d.Level,
                IsDeleted = d.IsDeleted,
                CreateDate = d.CreateDate,
                CreatedBy = d.CreatedBy,
                UpdateDate = d.UpdateDate,
                UpdatedBy = d.UpdatedBy,
                TreeNodes = d.TreeNodes.Select(t => new TreeNodeModel()
                {
                    Id = t.Id,
                    Guid = t.Guid,
                    ParentId = t.ParentId,
                    AbbreviationEnglish = t.AbbreviationEnglish,
                    AbbreviationTurkish = t.AbbreviationTurkish,
                    TextEnglish = t.TextEnglish,
                    TextTurkish = t.TextTurkish,
                    NameEnglish = t.NameEnglish,
                    NameTurkish = t.NameTurkish,
                    IsActive = t.IsActive,
                    IsDeleted = t.IsDeleted,
                    CreateDate = t.CreateDate,
                    CreatedBy = t.CreatedBy,
                    UpdateDate = t.UpdateDate,
                    UpdatedBy = t.UpdatedBy,
                    TreeNodeDetailId = t.TreeNodeDetailId
                }).ToList()
            });
        }

        public virtual IQueryable<TreeNodeDetailModel> GetDetailNodesQuery(int level)
        {
            return GetDetailNodesQuery().Where(q => q.Level == level).OrderBy(q => q.Level);
        }

        public virtual List<TreeNodeJqueryOrgchartModel> GetJqueryOrgchartNodes()
        {
            var query = Query().Where(q => (q.IsDeleted ?? false) == false);
            if (Config.ShowOnlyActive)
                query = query.Where(q => q.IsActive);
            return query.Select(q => new TreeNodeJqueryOrgchartModel()
            {
                id = q.Id,
                parent = q.ParentId,
                level = q.TreeNodeDetail.Level,
                name = (
                            Config.Language == Languages.Türkçe ?
                                q.NameTurkish ?? "" :
                                q.NameEnglish ?? ""
                       )
                       + "<br />" +
                       (
                            Config.ShowDetailTexts ?
                            (
                                Config.Language == Languages.Türkçe ?
                                    q.TreeNodeDetail.TextTurkish ?? "" :
                                    q.TreeNodeDetail.TextEnglish ?? ""
                            ) :
                            (
                                Config.Language == Languages.Türkçe ?
                                    q.TextTurkish ?? "" :
                                    q.TextEnglish ?? ""
                            )
                        )
                        + "<br />" +
                        (
                            Config.ShowAbbreviations ?
                            (
                                Config.Language == Languages.Türkçe ?
                                    q.AbbreviationTurkish ?? "" :
                                    q.AbbreviationEnglish ?? ""
                            ) : ""
                        )
            }).ToList();
        }

        public virtual List<TreeNodeModel> GetNodes(int parentId = 0)
        {
            List<TreeNodeModel> nodes = null;
            var recursiveNodes = GetRecursiveNodes(parentId);
            return GetNodes(recursiveNodes, nodes);
        }

        protected virtual List<TreeNodeModel> GetNodes(List<TreeNodeRecursiveModel> recursiveNodes, List<TreeNodeModel> nodes = null)
        {
            if (nodes == null)
                nodes = new List<TreeNodeModel>();
            TreeNodeModel node;
            foreach (var recursiveNode in recursiveNodes)
            {
                node = _nodes.FirstOrDefault(n => n.Id == recursiveNode.Id);
                nodes.Add(node);
                GetNodes(recursiveNode.Nodes, nodes);
            }
            return nodes;
        }

        public virtual List<TreeNodeModel> GetNodesByLevel(int level)
        {
            List<TreeNodeModel> nodes = GetNodes();
            return nodes.Where(n => n.TreeNodeDetail.Level == level && (n.IsDeleted ?? false == false)).ToList();
        }

        public virtual List<TreeNodeRecursiveModel> GetRecursiveNodes(int parentId = 0)
        {
            return _nodes.Where(n => n.ParentId == parentId).Select(n => new TreeNodeRecursiveModel()
            {
                Id = n.Id,
                ParentId = n.ParentId,
                AbbreviationEnglish = n.AbbreviationEnglish,
                AbbreviationTurkish = n.AbbreviationTurkish,
                Guid = n.Guid,
                IsActive = n.IsActive,
                NameEnglish = n.NameEnglish,
                NameTurkish = n.NameTurkish,
                TextEnglish = n.TextEnglish,
                TextTurkish = n.TextTurkish,
                TreeNodeDetailId = n.TreeNodeDetailId,
                Nodes = GetRecursiveNodes(n.Id),
                CreateDate = n.CreateDate,
                CreatedBy = n.CreatedBy,
                UpdateDate = n.UpdateDate,
                UpdatedBy = n.UpdatedBy
            }).ToList();
        }

        public virtual TreeNodeModel GetNode(int id)
        {
            return Query().Where(q => (q.IsDeleted ?? false) == false).SingleOrDefault(q => q.Id == id);
        }

        public virtual List<TreeNodeDetailModel> GetDetailNodes(int level)
        {
            return GetDetailNodesQuery(level).Where(q => (q.IsDeleted ?? false) == false).ToList();
        }

        public virtual TreeNodeDetailModel GetDetailNode(int id)
        {
            return GetDetailNodesQuery().Where(q => (q.IsDeleted ?? false) == false).SingleOrDefault(q => q.Id == id);
        }

        public virtual Result Add(TreeNodeModel model)
        {
            Result result;
            if (model.TreeNodeDetail == null)
                return new ErrorResult(Messages.NodeDetailNotFound + " " + Messages.OperationFailed);
            if (string.IsNullOrWhiteSpace(model.NameEnglish) && string.IsNullOrWhiteSpace(model.NameTurkish))
                return new ErrorResult(Messages.NodeNameRequired + " " + Messages.OperationFailed);
            Expression<Func<TreeNodeModel, bool>> predicate = p => p.Id != model.Id;
            Expression<Func<TreeNodeModel, bool>> namePredicate;
            if (!string.IsNullOrWhiteSpace(model.NameEnglish))
            {
                namePredicate = p => (p.NameEnglish ?? "").ToLower() == (model.NameEnglish ?? "").ToLower();
                if (!string.IsNullOrWhiteSpace(model.NameTurkish))
                {
                    namePredicate = namePredicate.Or(p => (p.NameTurkish ?? "").ToLower() == (model.NameTurkish ?? "").ToLower());
                }
            }
            else
            {
                namePredicate = p => (p.NameTurkish ?? "").ToLower() == (model.NameTurkish ?? "").ToLower();
            }
            predicate = predicate.And(namePredicate);
            if (Query().Any(predicate))
                return new ErrorResult(Messages.NodeFound + " " + Messages.OperationFailed);
            var entity = new TreeNode()
            {
                Id = Query().Max(q => q.Id) + 1,
                AbbreviationEnglish = model.AbbreviationEnglish?.Trim(),
                AbbreviationTurkish = model.AbbreviationTurkish?.Trim(),
                IsActive = model.IsActive,
                NameEnglish = model.NameEnglish?.Trim(),
                NameTurkish = model.NameTurkish?.Trim(),
                ParentId = model.ParentId,
                TextEnglish = model.TextEnglish?.Trim(),
                TextTurkish = model.TextTurkish?.Trim()
            };
            if (model.TreeNodeDetailId > 0)
            {
                entity.TreeNodeDetailId = model.TreeNodeDetailId;
                if (!string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextEnglish) || !string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextTurkish))
                {
                    result = UpdateDetailNode(model.TreeNodeDetail);
                    if (!result.IsSuccessful)
                        return result;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextEnglish) && string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextTurkish))
                    return new ErrorResult(Messages.NodeDetailTextRequired + " " + Messages.OperationFailed);
                result = AddDetailNode(entity, model.TreeNodeDetail);
                if (!result.IsSuccessful)
                    return result;
            }
            _treeNodeRepo.Add(entity);
            model.Id = entity.Id;
            return new SuccessResult();
        }

        public virtual Result Update(TreeNodeModel model)
        {
            Result result;
            if (model.TreeNodeDetail == null)
                return new ErrorResult(Messages.NodeDetailNotFound + " " + Messages.OperationFailed);
            if (string.IsNullOrWhiteSpace(model.NameEnglish) && string.IsNullOrWhiteSpace(model.NameTurkish))
                return new ErrorResult(Messages.NodeNameRequired + " " + Messages.OperationFailed);
            Expression<Func<TreeNodeModel, bool>> predicate = p => p.Id != model.Id;
            Expression<Func<TreeNodeModel, bool>> namePredicate;
            if (!string.IsNullOrWhiteSpace(model.NameEnglish))
            {
                namePredicate = p => (p.NameEnglish ?? "").ToLower() == (model.NameEnglish ?? "").ToLower();
                if (!string.IsNullOrWhiteSpace(model.NameTurkish))
                {
                    namePredicate = namePredicate.Or(p => (p.NameTurkish ?? "").ToLower() == (model.NameTurkish ?? "").ToLower());
                }
            }
            else
            {
                namePredicate = p => (p.NameTurkish ?? "").ToLower() == (model.NameTurkish ?? "").ToLower();
            }
            predicate = predicate.And(namePredicate);
            if (Query().Any(predicate))
                return new ErrorResult(Messages.NodeFound + " " + Messages.OperationFailed);
            var entity = _treeNodeRepo.Query().SingleOrDefault(q => q.Id == model.Id);
            if (entity == null)
                return new ErrorResult(Messages.NodeNotFound + " " + Messages.OperationFailed);
            var entities = new List<TreeNode>();
            if (entity.IsActive != model.IsActive)
            {
                if (entity.ParentId == 0)
                    return new ErrorResult(Messages.NoUpdateForRootNodeActive + " " + Messages.OperationFailed);
                entities = _treeNodeRepo.Query().Where(q => GetNodes(model.Id).Select(e => e.Id).Contains(q.Id)).ToList();
                foreach (var e in entities)
                {
                    e.IsActive = model.IsActive;
                }
            }
            entity.AbbreviationEnglish = model.AbbreviationEnglish?.Trim();
            entity.AbbreviationTurkish = model.AbbreviationTurkish?.Trim();
            entity.IsActive = model.IsActive;
            entity.NameEnglish = model.NameEnglish?.Trim();
            entity.NameTurkish = model.NameTurkish?.Trim();
            entity.ParentId = model.ParentId;
            entity.TextEnglish = model.TextEnglish?.Trim();
            entity.TextTurkish = model.TextTurkish?.Trim();
            if (model.TreeNodeDetailId > 0)
            {
                entity.TreeNodeDetailId = model.TreeNodeDetailId;
                if (!string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextEnglish) || !string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextTurkish))
                {
                    result = UpdateDetailNode(model.TreeNodeDetail);
                    if (!result.IsSuccessful)
                        return result;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextEnglish) && string.IsNullOrWhiteSpace(model.TreeNodeDetail.TextTurkish))
                    return new ErrorResult(Messages.NodeDetailTextRequired + " " + Messages.OperationFailed);
                result = AddDetailNode(entity, model.TreeNodeDetail);
                if (!result.IsSuccessful)
                    return result;
            }
            entities.Add(entity);
            foreach (var e in entities)
            {
                _treeNodeRepo.Update(e, false);
            }
            _treeNodeRepo.Save();
            return new SuccessResult();
        }

        public virtual Result AddDetailNode(TreeNode node, TreeNodeDetailModel detailNode)
        {
            Expression<Func<TreeNodeDetailModel, bool>> predicate = p => p.Id != detailNode.Id;
            Expression<Func<TreeNodeDetailModel, bool>> textPredicate;
            if (!string.IsNullOrWhiteSpace(detailNode.TextEnglish))
            {
                textPredicate = p => (p.TextEnglish ?? "").ToLower() == (detailNode.TextEnglish ?? "").ToLower();
                if (!string.IsNullOrWhiteSpace(detailNode.TextTurkish))
                {
                    textPredicate = textPredicate.Or(p => (p.TextTurkish ?? "").ToLower() == (detailNode.TextTurkish ?? "").ToLower());
                }
            }
            else
            {
                textPredicate = p => (p.TextTurkish ?? "").ToLower() == (detailNode.TextTurkish ?? "").ToLower();
            }
            predicate = predicate.And(textPredicate);
            if (GetDetailNodesQuery().Any(predicate))
            {
                return new ErrorResult(Messages.NodeDetailFound + " " + Messages.OperationFailed);
            }
            node.TreeNodeDetail = new TreeNodeDetail()
            {
                Level = detailNode.Level,
                TextEnglish = detailNode.TextEnglish?.Trim(),
                TextTurkish = detailNode.TextTurkish?.Trim()
            };
            return new SuccessResult();
        }

        public virtual Result UpdateDetailNode(TreeNodeDetailModel detailNode)
        {
            Expression<Func<TreeNodeDetailModel, bool>> predicate = p => p.Id != detailNode.Id;
            Expression<Func<TreeNodeDetailModel, bool>> textPredicate;
            if (!string.IsNullOrWhiteSpace(detailNode.TextEnglish))
            {
                textPredicate = p => (p.TextEnglish ?? "").ToLower() == (detailNode.TextEnglish ?? "").ToLower();
                if (!string.IsNullOrWhiteSpace(detailNode.TextTurkish))
                {
                    textPredicate = textPredicate.Or(p => (p.TextTurkish ?? "").ToLower() == (detailNode.TextTurkish ?? "").ToLower());
                }
            }
            else
            {
                textPredicate = p => (p.TextTurkish ?? "").ToLower() == (detailNode.TextTurkish ?? "").ToLower();
            }
            predicate = predicate.And(textPredicate);
            if (GetDetailNodesQuery().Any(predicate))
            {
                return new ErrorResult(Messages.NodeDetailFound + " " + Messages.OperationFailed);
            }
            var detailNodeEntity = _treeNodeDetailRepo.Query().SingleOrDefault(q => q.Id == detailNode.Id);
            if (detailNodeEntity == null)
                return new ErrorResult(Messages.NodeDetailNotFound + " " + Messages.OperationFailed);
            detailNodeEntity.Level = detailNode.Level;
            detailNodeEntity.TextEnglish = detailNode.TextEnglish?.Trim();
            detailNodeEntity.TextTurkish = detailNode.TextTurkish?.Trim();
            _treeNodeDetailRepo.Update(detailNodeEntity, false);
            return new SuccessResult();
        }

        public virtual Result Delete(params int[] ids)
        {
            Result result = new SuccessResult();
            foreach (int id in ids)
            {
                _treeNodeRepo.Delete(t => t.Id == id, false);
            }
            _treeNodeRepo.Save();
            return result;
        }

        public virtual Result Delete(int id)
        {
            var entity = _treeNodeRepo.Query().SingleOrDefault(q => q.Id == id);
            if (entity == null)
                return new ErrorResult(Messages.NodeNotFound + " " + Messages.OperationFailed);
            if (entity.ParentId == 0)
                return new ErrorResult(Messages.NoDeleteForRootNode + " " + Messages.OperationFailed);
            var entities = _treeNodeRepo.Query().Where(q => GetNodes(entity.Id).Select(e => e.Id).Contains(q.Id)).ToList();
            entities.Add(entity);
            foreach (var e in entities)
            {
                _treeNodeRepo.Delete(t => t.Id == e.Id, false);
            }
            _treeNodeRepo.Save();
            return new SuccessResult();
        }

        public virtual Result DeleteDetailNode(int id)
        {
            var detailNode = GetDetailNodesQuery().SingleOrDefault(q => q.Id == id);
            if (detailNode == null)
                return new ErrorResult(Messages.NodeDetailNotFound + " " + Messages.OperationFailed);
            if (detailNode.TreeNodes != null && detailNode.TreeNodes.Count > 0)
                return new ErrorResult(Messages.NoDeleteForNodeDetailHasNodes + " " + Messages.OperationFailed);
            _treeNodeDetailRepo.Delete(e => e.Id == id);
            return new SuccessResult(Messages.NodeDetailDeleted);
        }

        public void Dispose()
        {
            _treeNodeRepo.Dispose();
            _treeNodeDetailRepo.Dispose();
        }
    }
}
