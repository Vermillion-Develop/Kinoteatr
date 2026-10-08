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

        public async Task<StaffDTO?> LoginIn(string? login, string? password)
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
    }
}
