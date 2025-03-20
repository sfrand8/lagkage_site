using Lagkage.MinimalAPI.Features;

namespace Lagkage.MinimalApis;

public static class Setup
{
    public static void MapMinimalAPIs(this IEndpointRouteBuilder app)
    {
        GetCakeLayers.MapGetCakelayersEndpoint(app);
        AddCakeLayer.MapAddCakeLayerEndpoint(app);
    }
}