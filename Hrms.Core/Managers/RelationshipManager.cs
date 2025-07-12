using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Models;
using Hrms.Core.Abstractions.Managers;

namespace Hrms.Core.Managers
{
    public class RelationshipManager: IRelationshipManager
    {
        private readonly IRelationshipRepository _relationshipRepository;
        public RelationshipManager(IRelationshipRepository relationshipRepository)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _relationshipRepository.GetSelectListItemsAsync();
        }
    }
}
