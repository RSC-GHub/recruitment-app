using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Interviewer;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Application.Services.RecruitmentProccess
{
    public class InterviewerService : IInterviewerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<InterviewerListDTO>> GetAllAsync()
        {
            var interviewers = await _unitOfWork.Interviewers.GetAllAsync();
            var dtoList = interviewers.Select(i => new InterviewerListDTO
            {
                Id = i.Id,
                Name = i.Name,
                DepartmentName = i.Department?.Name!,
                CreatedOn = i.CreatedOn
            }).ToList();

            return dtoList;
        }

        public async Task AddAsync(InterviewerCreateDTO dto)
        {
            var interviewer = new Interviewer
            {
                Name = dto.Name,
                DepartmentId = dto.DepartmentId
            };

            await _unitOfWork.Interviewers.AddAsync(interviewer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(InterviewerUpdateDTO dto)
        {
            var interviewer = await _unitOfWork.Interviewers.GetByIdAsync(dto.Id);
            if (interviewer == null)
                throw new Exception("Interviewer not found");

            interviewer.Name = dto.Name;
            interviewer.DepartmentId = dto.DepartmentId;

            _unitOfWork.Interviewers.Update(interviewer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var interviewer = await _unitOfWork.Interviewers.GetByIdAsync(id);
            if (interviewer == null)
                throw new Exception("Interviewer not found");

            _unitOfWork.Interviewers.Delete(interviewer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<InterviewerDetailsDTO?> GetByIdAsync(int id)
        {
            var interviewer = await _unitOfWork.Interviewers.GetByIdAsync(id);
            if (interviewer == null)
                return null;

            return new InterviewerDetailsDTO
            {
                Id = interviewer.Id,
                Name = interviewer.Name,
                DepartmentId = interviewer.DepartmentId,
                DepartmentName = interviewer.Department?.Name!,
                CreatedOn = interviewer.CreatedOn,
                CreatedBy = interviewer.CreatedBy
            };
        }

        public async Task<PagedResult<InterviewerListDTO>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            var pagedResult =
                await _unitOfWork.InterviewerRepository
                                 .GetPagedAsync(page, pageSize, search);

            var dtoItems = pagedResult.Items.Select(i => new InterviewerListDTO
            {
                Id = i.Id,
                Name = i.Name,
                DepartmentId = i.DepartmentId,
                DepartmentName = i.Department?.Name!,
                CreatedOn = i.CreatedOn
            }).ToList();

            return new PagedResult<InterviewerListDTO>(
                dtoItems,
                pagedResult.TotalCount,
                page,
                pageSize
            );
        }
    }
}