namespace Recruitment.Application.DTOs.RecruitmentProccess
{
    public class ApplicationPagedResultDto
    {
        public List<ApplicationListDto> Items { get; set; } = new List<ApplicationListDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
