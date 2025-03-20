using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

public static class GetCakeLayers
{
    public static void MapGetCakelayersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("minimalapis/api/cakelayers", HandleRequest)
            .WithTags("GetCakelayers")
            .WithName("GetCakelayers")
            .WithDescription("Get all cake layers that can be added to cakes")
            .AllowAnonymous();
    }

    public static async Task<Results<Ok<IEnumerable<HttpCakeLayer>>, NotFound>> HandleRequest(ICakeLayerRepository repository)
    {
        var cakeLayers = (await repository.GetCakeLayers()).ToList();
        
        if (cakeLayers.Count == 0)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(cakeLayers.Select(x => x.ToHttpModel()));
    }
}