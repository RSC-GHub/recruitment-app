using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Web.ViewModels.UserManagement;
using Recruitment.Web.ViewModels.UserManagement.User;

namespace Recruitment.Web.Controllers
{
    public class UserProjectsController : Controller
    {
        private readonly IUserProjectService _userProjectService;
        private readonly IUnitOfWork _unitOfWork;

        public UserProjectsController(
            IUserProjectService userProjectService,
            IUnitOfWork unitOfWork)
        {
            _userProjectService = userProjectService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Assign(int userId)
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            var userProjects = await _userProjectService.GetUserProjectsAsync(userId);

            var vm = new AssignUserProjectsVM
            {
                UserId = userId,
                AllProjects = projects.ToList(),
                SelectedProjectIds = userProjects
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(AssignUserProjectsVM model)
        {
            await _userProjectService.AssignProjectsToUserAsync(
                model.UserId,
                model.SelectedProjectIds
            );

            TempData["Success"] = "Projects assigned successfully";

            return RedirectToAction("Assign", new { userId = model.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProjects(UserProfileViewModel model)
        {
            await _userProjectService.AssignProjectsToUserAsync(
                model.Id,
                model.SelectedProjectIds
            );

            return RedirectToAction("Profile", new { id = model.Id });
        }
        public async Task<IActionResult> Remove(int userId, int projectId)
        {
            await _userProjectService.RemoveProjectFromUserAsync(userId, projectId);

            return RedirectToAction("Assign", new { userId });
        }
    }
}
