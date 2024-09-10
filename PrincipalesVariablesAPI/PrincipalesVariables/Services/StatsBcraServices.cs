using PrincipalesVariables.DTOs;
using PrincipalesVariables.Models;
using System.Data.SqlClient;
using Dapper;
    
namespace PrincipalesVariables.Services;

public class StatsBcraServices
{
    private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.bcra.gob.ar") };

    private readonly IConfiguration _config;
    public StatsBcraServices(IConfiguration config)
    {
        _config = config;
    }
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

    public async Task<ResponseDTO<Stat[]>> GetMinStatsFromDB()
    {
        var conection = new SqlConnection(_config.GetConnectionString("DbStringConnection"));
        
        var res = await conection.QueryAsync<Stat>("");
      
      
        var responseDto = new ResponseDTO<Stat[]> { data = res.ToArray()};
        return responseDto;
    }


    public async Task<ResponseDTO<Variable[]>> GetVariableCombo()
    {
       
        var conection = new SqlConnection(_config.GetConnectionString("DbStringConnection"));
        
        var res = await conection.QueryAsync<Variable>("select description,idVariable,cdSerie from Variable");
        
        var responseDto = new ResponseDTO<Variable[]> { data = res.ToArray()};
        return responseDto;
    }
}