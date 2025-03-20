namespace Lagkage.MinimalApis;

public static class Setup
{
    public static void SetupMinimalAPIs(this IEndpointRouteBuilder app)
    {
        GetCakeLayers.SetupGetCakelayersEndpoint(app);
    }
}