using Hrms.Core.Models.Leave;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IHolidayManager
    {
        Task AddAsync(HolidayGroupModel model);
        Task<List<HolidayModel>> GetListAsync(int year, int month);
        Task<List<HolidayModel>> GetByYearAsync(int year);
        Task<List<HolidayModel>> GetPreviousYearAsync(int year, bool isChecked);
        Task DeleteAsync(int id);
        Task<bool> IsHolidayExistAsync(DateTime holidayDate);
    }
}
