using System.Data.SqlClient;
using Dapper;
using PrincipalesVariables.Models;

namespace PrincipalesVariables.Services;

public class SyncServices
{
    private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.bcra.gob.ar") };

    private readonly IConfiguration _config;
    
    
    public SyncServices(IConfiguration config)
    {
        _config = config;
    }
    
    public async void SyncStats()
    {
        var conection = new SqlConnection(_config.GetConnectionString("DbStringConnection"));
        var response = await _httpClient.GetAsync("/estadisticas/v2.0/PrincipalesVariables");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<PrincipalStatsResponse>();

        if (responseBody == null) return;
        
        for (var i = 0; i < responseBody.results.Length; i++)
        {
            await conection.QueryAsync(@"if  0 = (SELECT COUNT(*) FROM Variable v WHERE v.idVariable = @IdVariable) INSERT INTO BCRAPrincipal.dbo.Variable (idVariable, description, cdSerie) VALUES(@IdVariable,@Descripcion, @CdSerie)" , new
            {
                IdVariable = responseBody.results[i].idVariable,
                Descripcion = responseBody.results[i].descripcion,
                CdSerie = responseBody.results[i].cdSerie
            });
        }
    }
}