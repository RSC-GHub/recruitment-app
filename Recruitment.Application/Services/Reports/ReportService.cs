using Recruitment.Application.DTOs.Reports;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.Reports;
using Recruitment.Domain.Entities.Reports;

namespace Recruitment.Application.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            var reports = await _unitOfWork.ReportsRepository
                .GetAllWithParametersAsync();

            return reports.Select(MapToDto);
        }

        public async Task<IEnumerable<ReportDto>> GetActiveReportsAsync()
        {
            var reports = await _unitOfWork.ReportsRepository
                .GetActiveWithParametersAsync();

            return reports.Select(MapToDto);
        }

        public async Task<ReportDto?> GetByIdAsync(int id)
        {
            var report = await _unitOfWork.ReportsRepository
                .GetByIdWithParametersAsync(id);

            return report == null ? null : MapToDto(report);
        }

        public async Task<int> CreateAsync(CreateReportDto dto)
        {
            var exists = await _unitOfWork.ReportsRepository
                .AnyAsync(r => r.StoredProcedure == dto.StoredProcedure);

            if (exists)
                throw new Exception("Stored Procedure already exists");

            var report = new Report
            {
                Name = dto.Name,
                StoredProcedure = dto.StoredProcedure,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Parameters = dto.Parameters.Select(p => new ReportParameter
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            };

            await _unitOfWork.ReportsRepository.AddAsync(report);
            await _unitOfWork.CompleteAsync();

            return report.Id;
        }

        public async Task UpdateAsync(UpdateReportDto dto)
        {
            var report = await _unitOfWork.ReportsRepository
                .GetByIdWithParametersAsync(dto.Id);

            if (report == null)
                throw new Exception("Report not found");

            report.Name = dto.Name;
            report.StoredProcedure = dto.StoredProcedure;
            report.Description = dto.Description;
            report.IsActive = dto.IsActive;


            var removedParameters = report.Parameters
                .Where(p => !dto.Parameters.Any(dp => dp.Id == p.Id))
                .ToList();

            foreach (var param in removedParameters)
            {
                report.Parameters.Remove(param);
            }

            foreach (var paramDto in dto.Parameters)
            {
                if (paramDto.Id > 0)
                {
                    // Update existing
                    var existingParam = report.Parameters
                        .FirstOrDefault(p => p.Id == paramDto.Id);

                    if (existingParam == null)
                        continue;

                    existingParam.Name = paramDto.Name;
                    existingParam.DisplayName = paramDto.DisplayName;
                    existingParam.Type = paramDto.Type;
                    existingParam.IsRequired = paramDto.IsRequired;
                }
                else
                {
                    // Add new
                    report.Parameters.Add(new ReportParameter
                    {
                        Name = paramDto.Name,
                        DisplayName = paramDto.DisplayName,
                        Type = paramDto.Type,
                        IsRequired = paramDto.IsRequired
                    });
                }
            }

            await _unitOfWork.CompleteAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var report = await _unitOfWork.ReportsRepository.GetByIdAsync(id);

            if (report == null)
                throw new Exception("Report not found");

            _unitOfWork.ReportsRepository.Delete(report);
            await _unitOfWork.CompleteAsync();
        }

        // =========================
        // Mapping
        // =========================
        private static ReportDto MapToDto(Report report)
        {
            return new ReportDto
            {
                Id = report.Id,
                Name = report.Name,
                StoredProcedure = report.StoredProcedure,
                Description = report.Description,
                IsActive = report.IsActive,
                Parameters = report.Parameters.Select(p => new ReportParameterDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            };
        }
    }
}
