namespace Lagkage.MinimalAPI.Features;

public static class Setup
{
    // ReSharper disable once InconsistentNaming
    public static void MapMinimalAPIs(this IEndpointRouteBuilder app)
    {
        GetCakeLayers.MapGetCakeLayersEndpoint(app);
        AddCakeLayer.MapAddCakeLayerEndpoint(app);
        DeleteCakeLayer.MapDeleteCakeLayerEndpoint(app);
        GetCakeLayerById.MapGetCakeLayersEndpoint(app);
    }
}