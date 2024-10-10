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
            await conection.QueryAsync(@"INSERT INTO Variable (idVariable, description, cdSerie)
        VALUES (@IdVariable, @Descripcion, @CdSerie)
            ON CONFLICT (idVariable) DO NOTHING;" , new
            {
                IdVariable = t.idVariable,
                Descripcion = t.descripcion,
                CdSerie = t.cdSerie
            });
            await conection.QueryAsync(@"INSERT INTO Value (dateValue, idVariable, value)
                                            VALUES (@DateValue, @IdVariable, @Value)
                                            ON CONFLICT (idVariable, dateValue) DO NOTHING;" , new
            {
                IdVariable = t.idVariable,
                DateValue =  Convert.ToDateTime(t.fecha),
                Value = t.valor
            });
        }
    }
}