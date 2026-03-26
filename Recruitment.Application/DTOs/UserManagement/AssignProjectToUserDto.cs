namespace Recruitment.Application.DTOs.UserManagement
{
    public class AssignProjectToUserDto
    {
        public int UserId { get; set; }
        public List<int> ProjectIds { get; set; }
    }
}
