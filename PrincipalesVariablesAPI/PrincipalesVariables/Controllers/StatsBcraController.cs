using PrincipalesVariables.DTOs;
using PrincipalesVariables.Models;
using PrincipalesVariables.Services;

namespace PrincipalesVariables.Controllers;

public class StatsBcraController
{
    private readonly StatsBcraServices _statsService = new();

    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/statsbcra", GetMainStatsHandler);
        routes.MapGet("/statsbcra/{id:int}", GetMainStatsHandlerById);
    }

    private async Task<ResponseDTO<Stat[]>> GetMainStatsHandler()
    {
        return await _statsService.GetMainStats();
    }

    private async Task<ResponseDTO<Stat>> GetMainStatsHandlerById(int id)
    {
        return await _statsService.GetMainStatsById(id);
    }
}