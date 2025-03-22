using Lagkage.MinimalAPI.Features;

namespace Lagkage.MinimalApis;

public static class Setup
{
    // ReSharper disable once InconsistentNaming
    public static void MapMinimalAPIs(this IEndpointRouteBuilder app)
    {
        GetCakeLayers.MapGetCakeLayersEndpoint(app);
        AddCakeLayer.MapAddCakeLayerEndpoint(app);
    }
}