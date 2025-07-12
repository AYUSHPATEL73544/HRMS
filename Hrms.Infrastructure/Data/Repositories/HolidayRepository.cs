using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly DataContext _dataContext;
        public HolidayRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Holiday entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<List<HolidayModel>> GetByYearAsync(int year, bool isChecked = false)
        {
            var holidays = await _dataContext.Holidays
                        .Where(x => x.Year == year
                        && x.Status != Constants.RecordStatus.Deleted)
                        .OrderBy(x => x.Date)
                        .Select(x => new HolidayModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Date = x.Date,
                            Year = x.Year,
                            Description = x.Description,
                            Status = x.Status
                        }).ToListAsync();

            while (holidays.Count == 0 && isChecked && year >= 2020)
            { 
                year--;

                holidays = await _dataContext.Holidays
                      .Where(x => x.Status != Constants.RecordStatus.Deleted && x.Year == year)
                      .OrderBy(x => x.Date)
                      .Select(x => new HolidayModel
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Date = x.Date,
                          Year = x.Year,
                          Description = x.Description,
                          Status = x.Status
                      }).ToListAsync();
            }

            return holidays;
        }

        public async Task<List<HolidayModel>> GetListAsync(int year, int month)
        {
            return await _dataContext.Holidays
                           .Where(x => x.Status != Constants.RecordStatus.Deleted
                           && x.Date.Year == year
                           && x.Date.Month == month)
                           .Select(x => new HolidayModel
                           {
                               Id = x.Id,
                               Name = x.Name,
                               Description = x.Description,
                               Date = x.Date,
                               Year = x.Year,
                               Status = x.Status
                           }).ToListAsync();

        }

        public async Task<List<Holiday>> GetListByYearAsync(int year)
        {
            return await _dataContext.Holidays
                           .Where(x => x.Year == year
                           && x.Status != Constants.RecordStatus.Deleted)
                           .Select(x => new Holiday
                           {
                               Id = x.Id,
                               Name = x.Name,
                               Description = x.Description,
                               Date = x.Date,
                               Year = x.Year,
                               Status = x.Status
                           }).ToListAsync();
        }

        public async Task<Holiday> GetByIdAsync(int id)
        {
            return await _dataContext.Holidays
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new Holiday
                {
                    Status = x.Status,
                }).SingleOrDefaultAsync();
        }

        public async Task<Holiday> FindAsync(int id)
        {
            return await _dataContext.Holidays.FindAsync(id);
        }

        public void Update(Holiday entity)
        {
            _dataContext.Holidays.Update(entity);
        }

        public async Task<bool> IsHolidayExistAsync(DateTime holidayDate)
        {
            return await (_dataContext.Holidays
                .Where(x => x.Date == holidayDate
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => x.Name)).AnyAsync();
        }
    }
}
