﻿using PrincipalesVariables.Services;

namespace PrincipalesVariables.Controllers;

public class SyncController(IConfiguration config)
{
    private readonly SyncServices _syncServices = new(config);


    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost("/sync", Sync);
    }
    
    private void Sync()
    
    {
        try
        {
            _syncServices.SyncStats();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}