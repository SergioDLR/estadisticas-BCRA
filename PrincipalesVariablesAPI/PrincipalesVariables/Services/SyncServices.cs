using System.Data.SqlClient;
using Dapper;
using Npgsql;
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
        var conection = new NpgsqlConnection(_config.GetConnectionString("DbStringConnection"));
        var response = await _httpClient.GetAsync("/estadisticas/v2.0/PrincipalesVariables");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<PrincipalStatsResponse>();

        if (responseBody == null) return;
        
        foreach (var t in responseBody.results)
        {
            await conection.QueryAsync(@"if  0 = (SELECT COUNT(*) FROM Variable v WHERE v.idVariable = @IdVariable) INSERT INTO BCRAPrincipal.dbo.Variable (idVariable, description, cdSerie) VALUES(@IdVariable,@Descripcion, @CdSerie)" , new
            {
                IdVariable = t.idVariable,
                Descripcion = t.descripcion,
                CdSerie = t.cdSerie
            });
            await conection.QueryAsync(@"if  0 = (SELECT COUNT(*) FROM Value v WHERE v.idVariable = @IdVariable AND  v.dateValue = @DateValue ) INSERT INTO BCRAPrincipal.dbo.Value (dateValue , idVariable, value) VALUES(@DateValue,@IdVariable, @Value)" , new
            {
                IdVariable = t.idVariable,
                DateValue = t.fecha,
                Value = t.valor
            });
        }
    }
}