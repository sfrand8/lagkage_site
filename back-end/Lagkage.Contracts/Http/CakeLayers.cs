using Lagkage.Contracts.Models;

namespace Lagkage.Contracts.Http;

public record AddCakeLayerRequest
{
    public HttpCakeLayerToAdd HttpCakeLayer { get; init; }
}

public record HttpCakeLayerToAdd
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string RecipeUrl { get; init; }
    public CakeLayerPossibility[] PossibleLayers { get; init; }
    
    public CakeLayer ToDomain()
    {
        CakeLayerName name = new CakeLayerName(Name);
        CakeLayerId id = new CakeLayerId(name);
        
        return new CakeLayer(
            id,
            name,
            new CakeLayerDescription(Description), 
            new CakeLayerRecipeUrl(RecipeUrl),
            PossibleLayers);
    }
}

public record GetCakeLayersResponse
{
    public IEnumerable<HttpCakeLayer> CakeLayers { get; init; }
}

public record HttpCakeLayer
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string RecipeUrl { get; init; }
    public CakeLayerPossibility[] PossibleLayers { get; init; }
}

public static class CakeLayerExtensions
{
    public static HttpCakeLayer ToHttpModel(this CakeLayer cakeLayer)
    {
        return new HttpCakeLayer()
        {
            Id = cakeLayer.Id.Value.ToString(),
            Name = cakeLayer.Name.Value,
            Description = cakeLayer.Description.Value,
            RecipeUrl = cakeLayer.RecipeUrl.Value,
            PossibleLayers = cakeLayer.PossibleLayers
        };
    }
}