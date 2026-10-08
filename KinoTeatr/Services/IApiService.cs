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

        Task<StaffDTO?> LoginIn(string? login, string? password);
        Task<List<Specialization>> GetSpecializationsAsync();
    }
}
