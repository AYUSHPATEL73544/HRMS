using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class NoteManager : INoteManager
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NoteManager(INoteRepository noteRepository, IUnitOfWork unitOfWork)
        {
            _noteRepository = noteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(NoteModel model, int id)
        {
            var note = new Note
            {   
                EmployeeId = model.EmployeeId,
                Description = model.Description,
                CreatedById = id,
                UpdatedById = id,
                Status = Constants.RecordStatus.Active,
                EffectiveFrom = Utility.GetDateTime(),
                CreatedOn = Utility.GetDateTime(),

            };
            await _noteRepository.AddAsync(note);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<NoteModel>> GetListAsync(int employeeId)
        {
            return await _noteRepository.GetListAsync(employeeId);
        }
        public async Task<NoteModel> GetAsync(int id)
        {
            return await _noteRepository.GetAsync(id);
        }
        public async Task DeleteAsync(int id)
        {
            var note = await _noteRepository.FindAsync(id);
            note.Status = Constants.RecordStatus.Deleted;
            note.UpdatedOn = Utility.GetDateTime();
             _noteRepository.Update(note);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateAsync(NoteModel model)
        {
            var entity = await _noteRepository.FindAsync(model.Id);

            entity.Description = model.Description;
            entity.UpdatedOn = Utility.GetDateTime();
            _noteRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
