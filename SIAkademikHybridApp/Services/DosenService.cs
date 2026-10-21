using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using SIAkademikHybridApp.Models;
using System.Net.Http;

namespace SIAkademikHybridApp.Services
{
    public class DosenService
    {
        private readonly HttpClient _http;

        public DosenService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DosenModel>> GetDosensAsync()
        {
            return await _http.GetFromJsonAsync<List<DosenModel>>("api/dosen") ?? new();
        }

        public async Task<bool> CreateDosenAsync(DosenModel dosen)
        {
            var res = await _http.PostAsJsonAsync("api/dosen", dosen);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDosenAsync(int id, DosenModel dosen)
        {
            var res = await _http.PutAsJsonAsync($"api/dosen/{id}", dosen);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDosenAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/dosen/{id}");

            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    $"Gagal menghapus dosen. Status: {(int)res.StatusCode}. {error}"
                );
            }

            return true;
        }

        public async Task<DosenModel?> GetDosenByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DosenModel>(
                $"api/dosen/{id}"
            );
        }
    }
}
