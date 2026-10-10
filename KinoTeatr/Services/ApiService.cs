using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using Avalonia.Controls.Documents;
using System.Diagnostics;

namespace CinemaHub.Services
{
    public class ApiService:IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://100.101.246.43:80") };
        }

        public async Task<List<StaffDTO>> GetStaffAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<StaffDTO>>("api/Staff/getStaff");
                if (response == null)
                {
                    return new List<StaffDTO>();
                }
                return response;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Произошла ошибка: " + ex.Message);
                return new List<StaffDTO>();
            }
        }

        public async Task<StaffDTO?> LoginInAsync(string? login, string? password)
        {
            try
            {
                NewLoginRequest dataRequest = new NewLoginRequest() { Login = login, Password = password };
                var response = await _httpClient.PostAsJsonAsync("api/Staff/loginStaff", dataRequest);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<StaffDTO>();
                }
                return null;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }

        }

        public async Task<List<Specialization>> GetSpecializationsAsync() 
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Specialization>>("api/Staff/getRoles");
                if (response == null)
                {
                    Debug.WriteLine("Ошибка при получении специализаций");
                    return new List<Specialization>();
                }
                return response;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Ошибка: " + ex.Message);
                return new List<Specialization>();
            }
        }

        public async Task<bool> RegisterStaffAsync(string? family, string? name, string? father, int? role, string? phone, string? email, decimal? stavka, string? datalog, string? password)
        {
            try
            {
                NewStaffRequest dataStaff = new NewStaffRequest() { Family = family, Name = name, Father = father, SpecializationId = role, Phone = phone, Email = email, Stavka = stavka, DataLogId = datalog, Password = password};
                var response = await _httpClient.PostAsJsonAsync("api/Staff/newStaff", dataStaff);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при регистрации сотрудника");
                return false;
            }
        }

        public async Task<bool> UpdateStaffAsync(string? family, string? name, string? father, int? role, string? phone, string? email, decimal? stavka, string? datalog, string? password, int? userId, string? newDatalog)
        {
            try
            {
                NewStaffRequest dataStaff = new NewStaffRequest() { Family = family, Name = name, Father = father, SpecializationId = role, Phone = phone, Email = email, Stavka = stavka, DataLogId = datalog, Password = password, Id = userId, NewDataLogId = newDatalog };
                var response = await _httpClient.PostAsJsonAsync("api/Staff/updateStaff", dataStaff);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Ошибка при редактировании сотрудника");//
                return false;
            }
        }
        
        public async Task<bool> DeleteStaffAsync(int? staffId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Staff/deleteStaff", staffId);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при удалении сотрудника" + ex.Message);//
                return false;
            }
        }

        public async Task<List<VideoProductDTO>> GetVideoProductDTOsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<VideoProductDTO>>("api/Project/getProjects");
                if(response == null)
                {
                    Debug.WriteLine("Ошибка при получении специализаций");
                    return new List<VideoProductDTO>();
                }
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка: " + ex.Message);
                return new List<VideoProductDTO>();
            }
        }

        public async Task<List<StatusVideoProduct>> GetStatusVideoAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<StatusVideoProduct>>("api/Project/getStatuses");
                if(response == null)
                {
                    Debug.WriteLine("Ошибка при получении специализаций");
                    return new List<StatusVideoProduct>();
                }
                return response;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Ошибка: " + ex.Message);
                return new List<StatusVideoProduct>();
            }
        }
    }
}
