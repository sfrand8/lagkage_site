using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

public static class AddCakeLayer
{
    public static void MapAddCakeLayerEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("minimalapis/api/cakelayers", AddCakeLayers);
    }

    public static async Task<Created<string>> AddCakeLayers(
        ICakeLayerRepository repository, AddCakeLayerRequest request)
    {
        var cakeLayer = request.HttpCakeLayer.ToDomain();
        await repository.AddCakeLayer(cakeLayer);
        
        var location = $"/minimalapis/api/cakelayers/{cakeLayer.Id}";
    
        return TypedResults.Created(location, cakeLayer.Id.Value.ToString());
    }
}