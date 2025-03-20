namespace Lagkage.Contracts.Models;

public class CakeLayer
{
    public CakeLayer(CakeLayerId id, CakeLayerName name, CakeLayerDescription description, CakeLayerRecipeUrl recipeUrl, CakeLayerPossibility[] possibleLayers)
    {
        Id = id;
        Name = name;
        Description = description;
        RecipeUrl = recipeUrl;
        PossibleLayers = possibleLayers;
    }
    
    public CakeLayerId Id { get; }
    public CakeLayerName Name { get;  }
    public CakeLayerDescription Description { get;  }
    public CakeLayerRecipeUrl RecipeUrl { get;  }
    public CakeLayerPossibility[] PossibleLayers { get;  }

    public override bool Equals(object? obj)
    {
        if (obj is CakeLayer other)
        {
            return Id.Value == other.Id.Value &&
                   Name.Value == other.Name.Value &&
                   Description.Value == other.Description.Value &&
                   RecipeUrl.Value == other.RecipeUrl.Value &&
                   PossibleLayers.OrderBy(l => l).SequenceEqual(other.PossibleLayers);
        }

        return false;
    }

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(Id, Name, Description, RecipeUrl);
            
        var sortedLayers = PossibleLayers.OrderBy(l => l);
        hash = HashCode.Combine(hash, string.Join(",", sortedLayers)); // Combine as a single string

        return hash;
    }
}