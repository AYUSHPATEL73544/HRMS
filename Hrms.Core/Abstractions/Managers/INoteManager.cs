using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;


namespace Hrms.Core.Abstractions.Managers
{
    public interface INoteManager
    {
        Task AddAsync(NoteModel model, int id);
        Task<List<NoteModel>> GetListAsync(int employeeId);
        Task<NoteModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(NoteModel model);

    }
}
