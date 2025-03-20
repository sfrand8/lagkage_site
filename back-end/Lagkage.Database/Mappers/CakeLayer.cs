using Lagkage.Contracts.Models;

namespace Lagkage.Database.Mappers;

internal static class CakeLayerMapper
{
    internal static CakeLayerDTO MapToDTO(this CakeLayer cakeLayer)
    {
        var dto = new CakeLayerDTO();
        
        dto.Id = cakeLayer.Id.Value.ToString();
        dto.Name = cakeLayer.Name.Value;
        dto.Description = cakeLayer.Description.Value;
        dto.RecipeUrl = cakeLayer.RecipeUrl.Value;
        dto.PossibleLayers = cakeLayer.PossibleLayers.Select(x => x.ToString()).ToArray();
        return dto;
    }

    internal static CakeLayer MapToDomain(this CakeLayerDTO cakeLayerDTO)
    {
        var possibleLayers = cakeLayerDTO.PossibleLayers
            .Select(name => Enum.TryParse(name, out CakeLayerPossibility layer)
                ? layer
                : throw new ArgumentException($"Invalid layer name: {name}"))
            .ToArray();
        
        
        var id = Guid.TryParse(cakeLayerDTO.Id, out Guid guid) ? guid : throw new ArgumentException($"Invalid id: {cakeLayerDTO.Id}");
        
        return new CakeLayer(
            new CakeLayerId(id),
            new CakeLayerName(cakeLayerDTO.Name),
            new CakeLayerDescription(cakeLayerDTO.Description), 
            new CakeLayerRecipeUrl(cakeLayerDTO.RecipeUrl),
            possibleLayers);
    }
}