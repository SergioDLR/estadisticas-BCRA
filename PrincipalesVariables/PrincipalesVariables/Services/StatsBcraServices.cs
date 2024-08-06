using PrincipalesVariables.DTOs;
using PrincipalesVariables.Models;

namespace PrincipalesVariables.Services;

public class StatsBcraServices
{
    private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.bcra.gob.ar") };

    public async Task<ResponseDTO<Stat[]>> GetMainStats()
    {
        var response = await _httpClient.GetAsync("/estadisticas/v2.0/PrincipalesVariables");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<PrincipalStatsResponse>();
        var responseDto = new ResponseDTO<Stat[]> { data = responseBody.results};
        return responseDto;
    }

    public async Task<ResponseDTO<Stat>> GetMainStatsById(int id)
    {
        var response = await _httpClient.GetAsync("/estadisticas/v2.0/PrincipalesVariables");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<PrincipalStatsResponse>();
        var responseDto = new ResponseDTO<Stat> { data = responseBody.results.FirstOrDefault(stat => stat.idVariable == id) };
        return responseDto;
    }
}