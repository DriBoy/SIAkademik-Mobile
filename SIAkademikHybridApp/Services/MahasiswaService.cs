using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http.Json;
using SIAkademikHybridApp.Models;

namespace SIAkademikHybridApp.Services
{
    public class MahasiswaService
    {
        private readonly HttpClient _http;

        public MahasiswaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<MahasiswaModel>> GetMahasiswasAsync()
        {
            var data = await _http.GetFromJsonAsync<List<MahasiswaModel>>(
                "api/mahasiswa");

            return data ?? new List<MahasiswaModel>();
        }

        public async Task CreateMahasiswaAsync(MahasiswaModel mhs)
        {
            var response = await _http.PostAsJsonAsync("api/mahasiswa", mhs);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Gagal menambahkan mahasiswa. Status: {(int)response.StatusCode}. {error}");
            }
        }

        public async Task UpdateMahasiswaAsync(int id, MahasiswaModel mhs)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/mahasiswa/{id}", mhs);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Gagal memperbarui mahasiswa. Status: {(int)response.StatusCode}. {error}");
            }
        }

        public async Task DeleteMahasiswaAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/mahasiswa/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Gagal menghapus mahasiswa. Status: {(int)response.StatusCode}. {error}");
            }
        }

        public async Task<MahasiswaModel?> GetMahasiswaByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<MahasiswaModel>(
                $"api/mahasiswa/{id}"
            );
        }
    }
}

