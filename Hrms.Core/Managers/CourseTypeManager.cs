using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;

namespace Hrms.Core.Managers
{
    public class CourseTypeManager: ICourseTypeManager
    {
        private readonly ICourseTypeRepository _courseTypeRepository;
        public CourseTypeManager(ICourseTypeRepository courseTypeRepository)
        {
            _courseTypeRepository = courseTypeRepository;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _courseTypeRepository.GetSelectListItemsAsync();
        }
    }
}
