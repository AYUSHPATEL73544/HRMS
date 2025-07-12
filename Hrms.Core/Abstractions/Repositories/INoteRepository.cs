using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface INoteRepository
    {
        Task AddAsync(Note entity);
        Task<NoteModel> GetAsync(int id);
        Task<List<NoteModel>> GetListAsync(int employeeId);
        void Update(Note entity);

        void Delete(Note entity);

        Task<Note> FindAsync(int id);
    }
}
