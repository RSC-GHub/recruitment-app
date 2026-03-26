namespace Recruitment.Application.Interfaces.Services.UserManagement
{
    public interface IUserProjectService
    {
        Task<List<int>> GetUserProjectsAsync(int userId);

        Task AssignProjectsToUserAsync(int userId, List<int> projectIds);

        Task AddProjectToUserAsync(int userId, int projectId);

        Task RemoveProjectFromUserAsync(int userId, int projectId);

        Task ClearUserProjectsAsync(int userId);
        Task UpdateUserProjectsAsync(int userId, List<int> projectIds);

    }
}
