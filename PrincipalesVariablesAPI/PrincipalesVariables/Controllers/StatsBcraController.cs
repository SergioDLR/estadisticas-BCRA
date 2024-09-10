using PrincipalesVariables.DTOs;
using PrincipalesVariables.Models;
using PrincipalesVariables.Services;

namespace PrincipalesVariables.Controllers;

public class StatsBcraController
{


    private readonly StatsBcraServices _statsService;

    public StatsBcraController(IConfiguration configuration)
    {
        _statsService = new StatsBcraServices(configuration);
    }

    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/statsbcra", GetMainStatsHandler);
        routes.MapGet("/statsbcra/{id:int}", GetMainStatsHandlerById);
        routes.MapGet("/statsbcra/comboVarible", GetVariabletCombo);
    }

    private async Task<ResponseDTO<Stat[]>> GetMainStatsHandler()
    {
        return await _statsService.GetMainStats();
    }

    private async Task<ResponseDTO<Stat>> GetMainStatsHandlerById(int id)
    {
        return await _statsService.GetMainStatsById(id);
    }

    private async Task<ResponseDTO<Variable[]>> GetVariabletCombo()
    {
        return await _statsService.GetVariableCombo();
    }
}