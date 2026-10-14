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
        Task<bool> RegisterStaffAsync(string? family, string? name, string? father, int? role, string? phone, string? email, decimal? stavka, string? datalog, string? password);
        Task<bool> UpdateStaffAsync(string? family, string? name, string? father, int? role, string? phone, string? email, decimal? stavka, string? datalog, string? password, int? userId, string? newDatalog);
        Task<bool> AddNewProjectAsync(string? name, decimal? budget, int? vidId, DateTimeOffset? dateStart, DateTimeOffset? dateEnd, int? statusId, int? adultId, string? idea);
        Task<bool> UpdateSelectedProjectAsync(int videoId, string? name, decimal? budget, int? vidId, DateTimeOffset? dateEnd, int? statusId, int? adultId, string? idea);
        Task<bool> DeleteStaffAsync(int? staffId);
        Task<List<StaffDTO>> GetStaffAsync();
        Task<StaffDTO?> LoginInAsync(string? login, string? password);
        Task<List<Specialization>> GetSpecializationsAsync();
        Task<List<VideoProductDTO>> GetVideoProductDTOsAsync();
        Task<List<StatusVideoProduct>> GetStatusVideoAsync();
        Task<List<VidVideoProduct>> GetVidVideoAsync();
        Task<List<AdultVideoProduct>> GetAdultsAsync();
        Task<List<CinemaGroupDTO>> GetAllGroupsAsync();
        Task<bool> AddNewGroupAsync(string? name, int? productId);
        Task<bool> UpdateGroupAsync(string? name, int? productId, int? groupId);
        Task<List<StaffGroupDTO>> GetStaffGroupDTOsAsync(int? groupId);
        Task<bool> AddStaffInGroup(int? staffId, int? videoId, int? groupId, int? roleId);
        Task<bool> DeleteStaffGroupAsync(int? staffId, int? groupId);
        Task<List<AreaDTO>> GetAreaDTOsAsync();
        Task<List<StreetDTO>> GetStreetsAsync();
        Task<List<TypeArea>> GetTypesAreaAsync();
        Task<bool> AddNewArea(int? typeId, string? adress, int? streetId, decimal? dayCost, int? capacity);
        Task<List<VideoProductInCinemasDTO>> GetVideoCinemasAsync();
        Task<List<Cinema>> GetCinemasAsync();
        Task<bool> AddNewProcatAsync(int? videoId, int? cinemaId, decimal? revenue, decimal? rating);
    }
}
