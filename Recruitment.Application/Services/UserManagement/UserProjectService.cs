using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Entities;

namespace Recruitment.Application.Services.UserManagement
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<int>> GetUserProjectsAsync(int userId)
        {
            var userProjects = await _unitOfWork.UserProjects
                .FindAsync(x => x.UserId == userId);

            return userProjects.Select(x => x.ProjectId).ToList();
        }

        public async Task AssignProjectsToUserAsync(int userId, List<int> projectIds)
        {
            var existing = await _unitOfWork.UserProjects
                .FindAsync(x => x.UserId == userId);

            _unitOfWork.UserProjects.RemoveRange(existing);

            foreach (var projectId in projectIds)
            {
                await _unitOfWork.UserProjects.AddAsync(new UserProject
                {
                    UserId = userId,
                    ProjectId = projectId
                });
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task AddProjectToUserAsync(int userId, int projectId)
        {
            var exists = await _unitOfWork.UserProjects
                .AnyAsync(x => x.UserId == userId && x.ProjectId == projectId);

            if (exists) return;

            await _unitOfWork.UserProjects.AddAsync(new UserProject
            {
                UserId = userId,
                ProjectId = projectId
            });

            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveProjectFromUserAsync(int userId, int projectId)
        {
            var relation = await _unitOfWork.UserProjects
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProjectId == projectId);

            if (relation == null) return;

            _unitOfWork.UserProjects.Delete(relation);

            await _unitOfWork.CompleteAsync();
        }

        public async Task ClearUserProjectsAsync(int userId)
        {
            var relations = await _unitOfWork.UserProjects
                .FindAsync(x => x.UserId == userId);

            _unitOfWork.UserProjects.RemoveRange(relations);

            await _unitOfWork.CompleteAsync();
        }
        public async Task UpdateUserProjectsAsync(int userId, List<int> projectIds)
        {
            // Remove all existing projects for the user
            var existingProjects = await _unitOfWork.UserProjects.FindAsync(x => x.UserId == userId);
            _unitOfWork.UserProjects.RemoveRange(existingProjects);

            // Add new projects
            if (projectIds != null && projectIds.Any())
            {
                foreach (var projectId in projectIds)
                {
                    await _unitOfWork.UserProjects.AddAsync(new UserProject
                    {
                        UserId = userId,
                        ProjectId = projectId
                    });
                }
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
