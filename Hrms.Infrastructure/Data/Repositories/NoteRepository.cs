using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly DataContext _dataContext;
        public NoteRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Note entity)
        {
            await _dataContext.Notes.AddAsync(entity);
        }

        public async Task<List<NoteModel>> GetListAsync(int employeeId)
        {
            return await _dataContext.Notes.Where(x => x.EmployeeId == employeeId && x.Status != Constants.RecordStatus.Deleted).Select(x => new NoteModel
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Description = x.Description,
                CreatedOn = x.CreatedOn,
            }).ToListAsync();

        }

        public async Task<NoteModel> GetAsync(int id)
        {
            return await _dataContext.Notes.Where(x => x.Id == id).Select(x => new NoteModel
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Description = x.Description
            }).SingleAsync();
        }

        public void Delete(Note entity)
        {
            _dataContext.Notes.Remove(entity);
        }

        public async Task<Note> FindAsync(int id)
        {

            return await _dataContext.Notes.FindAsync(id);
        }

        public void Update(Note entity)
        {
            _dataContext.Notes.Update(entity);
        }




    }

}
