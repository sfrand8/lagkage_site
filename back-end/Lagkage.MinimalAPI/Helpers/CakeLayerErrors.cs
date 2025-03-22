namespace Lagkage.MinimalAPI.Features;

public static class CakeLayerErrors
{
    public static readonly Error CakeLayerAlreadyExists = new(ErrorType.Conflict, "Cake layer with the provided name already exists.");
    public static readonly Error CakelayerNotfound = new(ErrorType.NotFound, "The requested cake layer was not found.");
    public static readonly Error InternalError = new(ErrorType.ValidationError, "An internal error occurred.");
}