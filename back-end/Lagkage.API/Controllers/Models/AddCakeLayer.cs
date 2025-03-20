using Lagkage.Contracts.Models;

namespace Lagkage.Controllers.Models;

public class AddCakeLayerRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string RecipeUrl { get; set; }
    public CakeLayerPossibility[] PossibleLayers { get; set; }

    public CakeLayer ToDomain()
    {
        var id = Guid.TryParse(Id, out Guid guid) ? guid : throw new ArgumentException($"Invalid id: {Id}");
        
        return new CakeLayer(
            new CakeLayerId(id),
            new CakeLayerName(Name),
            new CakeLayerDescription(Description), 
            new CakeLayerRecipeUrl(RecipeUrl),
            PossibleLayers);
    }
}