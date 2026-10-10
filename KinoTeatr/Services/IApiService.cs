using System;
using System.Collections.Generic;
using System.Text;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using System.Threading.Tasks;

namespace CinemaHub.Services
{
    public interface IApiService
    {
        Task<List<StaffDTO>> GetStaffAsync();

        Task<StaffDTO?> LoginInAsync(string? login, string? password);
        Task<List<Specialization>> GetSpecializationsAsync();
        Task<bool> RegisterStaffAsync(string? family, string? name, string? father, int? role, string? phone, string? email, decimal? stavka, string? datalog, string? password);
        Task<bool> UpdateStaffAsync(string? family, string? name, string? father, int? role, string? phone, string? email, decimal? stavka, string? datalog, string? password, int? userId, string? newDatalog);

        Task<bool> DeleteStaffAsync(int? staffId);
        Task<List<VideoProductDTO>> GetVideoProductDTOsAsync();
        Task<List<StatusVideoProduct>> GetStatusVideoAsync();
    }
}
