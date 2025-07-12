using Hrms.Core.Entities;
using Hrms.Core.Models.Leave;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IHolidayRepository
    {
        Task AddAsync(Holiday entity);
        Task<List<HolidayModel>> GetByYearAsync(int year, bool isChecked = false);
        Task<Holiday> GetByIdAsync(int id);
        Task<List<Holiday>> GetListByYearAsync(int year);
        Task<List<HolidayModel>> GetListAsync(int year, int month);
        Task<Holiday> FindAsync(int id);
        void Update(Holiday entity);
        Task<bool> IsHolidayExistAsync(DateTime holidayDate);
    }
}
