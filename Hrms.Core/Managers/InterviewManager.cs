using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Managers
{
    public class InterviewManager : IInterviewManager
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InterviewManager(IInterviewRepository interviewRepository,
            IUnitOfWork unitOfWork)
        {
            _interviewRepository = interviewRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(InterviewModel model, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var interview = new Interview
                {
                   InterviewMode = model.InterviewMode,
                   InterviewType = model.InterviewType,
                   CandidateId = model.CandidateId,
                   InterviewDate = model.InterviewDate,
                   ScheduleDate = model.ScheduleDate,
                   ScheduleTime = model.ScheduleTime,
                   EligibleForNextRound = model.EligibleForNextRound,
                   Remark = model.Remark,
                   Rating = model.Rating,
                   InterviewerId = model.InterviewerId,
                   CreatedById = userId,
                   CreatedOn = Utility.GetDateTime(),
                   EffectiveFrom = Utility.GetDateTime(),
                   Status = Constants.RecordStatus.Scheduled
                };

                await _interviewRepository.AddAsync(interview);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<InterviewModel> GetAsync(int id)
        {
            return await _interviewRepository.GetAsync(id);
        }

        public async Task<List<InterviewModel>> GetListByCandidateIdAsync(int candidateId)
        {
            return await _interviewRepository.GetListByCandidateIdAsync(candidateId);
        }

        public async Task<InterviewModel> GetDetailAsync(int id)
        {
            return await _interviewRepository.GetDetailAsync(id);
        }

        public async Task<MatTableResponse<InterviewModel>> GetPagedListAsync(MatDataTableRequest model,int userId)
        {
            return await _interviewRepository.GetPagedListAsync(model, userId);
        }

        public async Task UpdateAsync(InterviewModel model, int userId)
        {
            var interview = await _interviewRepository.FindAsync(model.Id);

            interview.Rating = model.Rating;
            interview.InterviewerId = model.InterviewerId != 0 ? model.InterviewerId : interview.InterviewerId; 
            interview.EligibleForNextRound = model.EligibleForNextRound;
            interview.InterviewDate = model.InterviewDate;
            interview.ScheduleDate = model.ScheduleDate;
            interview.ScheduleTime = model.ScheduleTime;
            interview.Remark = model.Remark;
            interview.UpdatedById = userId;
            interview.UpdatedOn = Utility.GetDateTime();

            //if (model.EligibleForNextRound == false)
            //{
            //    interview.Status = Constants.RecordStatus.Rejected;
            //    interview.EffectiveTo = Utility.GetDateTime();
            //}
            //else if(model.EligibleForNextRound == true)
            //{
            //    interview.Status = Constants.RecordStatus.Approved;
            //}

            _interviewRepository.Update(interview);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
