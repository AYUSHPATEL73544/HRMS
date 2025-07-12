using Hrms.Core.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IAttendanceManager
    {
        Task AddAsync(AttendanceModel model);
        Task<AttendanceModel> GetAsync(int id);
        Task<List<AttendanceModel>> GetListAsync();
        Task UpdateAsync(AttendanceModel model);
        Task DeleteAsync(int id);
    }
}
