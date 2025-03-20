using Lagkage.Contracts.Interfaces;

namespace Lagkage.MinimalApis;

public static class GetCakeLayers
{
    public static void SetupGetCakelayersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("minimalapis/api/cakelayers", HandleRequest)
            .WithTags("GetCakelayers")
            .WithName("GetCakelayers")
            .WithDescription("Get all cake layers that can be added to cakes")
            .AllowAnonymous();
    }

    public static async Task<IResult> HandleRequest(ICakeLayerRepository repository)
    {
        var cakeLayers = await repository.GetCakeLayers();
        
        if (!cakeLayers.Any())
        {
            return Results.NotFound();
        }
        
        return Results.Ok(cakeLayers);
    }
}