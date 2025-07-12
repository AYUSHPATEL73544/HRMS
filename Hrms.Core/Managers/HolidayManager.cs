using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Managers
{
    public class HolidayManager : IHolidayManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHolidayRepository _holidayRepository;
        public HolidayManager(IHolidayRepository holidayRepository,
            IUnitOfWork unitOfWork)
        {
            _holidayRepository = holidayRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(HolidayGroupModel model)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var holidayList = await _holidayRepository.GetListByYearAsync(model.Year);

                if (holidayList.Count > 0)
                {
                    var holidaysToAdd = model.Holidays.Where(h => !holidayList.Exists(x => x.Id == h.Id));

                    var holidaysToUpdate = model.Holidays.Where(h => holidayList.Exists(x => x.Id == h.Id));

                    var holidaysToRemove = holidayList.Where(h => !model.Holidays.Exists(x => x.Id == h.Id));

                    foreach (var updateHoliday in holidaysToUpdate)
                    {
                        var holiday = await _holidayRepository.FindAsync(updateHoliday.Id);
                        holiday.Name = updateHoliday.Name;
                        holiday.Date = updateHoliday.Date;
                        holiday.Year = model.Year;

                        _holidayRepository.Update(holiday);
                    }

                    foreach (var removeHoliday in holidaysToRemove)
                    {
                        var existingHoliday = await _holidayRepository.FindAsync(removeHoliday.Id);
                        existingHoliday.Status = Constants.RecordStatus.Deleted;

                        _holidayRepository.Update(existingHoliday);
                    }

                    foreach (var holidayModel in holidaysToAdd)
                    {
                        var holiday = new Holiday
                        {
                            Year = model.Year,
                            Name = holidayModel.Name,
                            Description = holidayModel.Description,
                            Date = holidayModel.Date,
                            Status = Constants.RecordStatus.Active
                        };
                        await _holidayRepository.AddAsync(holiday);
                    } 
                }
                else
                {
                    foreach (var holidayModel in model.Holidays)
                    {
                        var holiday = new Holiday
                        {
                            Year = model.Year,
                            Name = holidayModel.Name,
                            Description = holidayModel.Description,
                            Date = holidayModel.Date,
                            Status = Constants.RecordStatus.Active
                        };
                        await _holidayRepository.AddAsync(holiday);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<HolidayModel>> GetListAsync(int year, int month)
        {
            return await _holidayRepository.GetListAsync(year, month);
        }

        public async Task<List<HolidayModel>> GetByYearAsync(int year)
        {
            return await _holidayRepository.GetByYearAsync(year);
        }

        public async Task<List<HolidayModel>> GetPreviousYearAsync(int year, bool isChecked)
        {
            var currentYearHolidays = new List<HolidayModel>();
            int previousYear = year - 1;

            if (isChecked)
            {
                var lastYearHolidays = await _holidayRepository.GetByYearAsync(previousYear, isChecked);
                 currentYearHolidays = await _holidayRepository.GetByYearAsync(year);

                var lastYearDifferentHolidays = lastYearHolidays.Where(lyh => !currentYearHolidays
                                                .Exists(cyh => cyh.Date.Day == lyh.Date.Day
                                                && cyh.Date.Month == lyh.Date.Month
                                                && cyh.Name == lyh.Name));

                foreach (var h in lastYearDifferentHolidays)
                {
                    DateTime currentDate = new DateTime(year, h.Date.Month, h.Date.Day);

                    var holiday = new HolidayModel
                    {
                        Id = h.Id,
                        Year = year,
                        Name = h.Name,
                        Description = h.Description,
                        Date = currentDate,
                        Status = Constants.RecordStatus.Active
                    };

                    currentYearHolidays.Add(holiday); 
                }
                currentYearHolidays = currentYearHolidays.OrderBy(x => x.Date).ToList();

                return currentYearHolidays;
            }
            else
            {
                var holidays = await _holidayRepository.GetListByYearAsync(year);

                foreach (var holiday in holidays)
                {
                    var newHoliday = new HolidayModel
                    {
                        Id = holiday.Id,
                        Year = holiday.Year,
                        Name = holiday.Name,
                        Description = holiday.Description,
                        Date = holiday.Date,
                        Status = Constants.RecordStatus.Active
                    };
                    currentYearHolidays.Add(newHoliday);
                }
                currentYearHolidays = currentYearHolidays.OrderBy(x => x.Date).ToList();

                return currentYearHolidays;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var holiday = await _holidayRepository.FindAsync(id);
            holiday.Status = Constants.RecordStatus.Deleted;

            _holidayRepository.Update(holiday);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsHolidayExistAsync(DateTime holidayDate)
        {
            return await _holidayRepository.IsHolidayExistAsync(holidayDate);
        }
    }
}
