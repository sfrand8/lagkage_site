namespace Lagkage.Contracts.Models;

public class CakeLayer
{
    private sealed class CakeLayerEqualityComparer : IEqualityComparer<CakeLayer>
    {
        public bool Equals(CakeLayer? x, CakeLayer? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(CakeLayer obj)
        {
            throw new NotImplementedException();
        }
    }

    public static IEqualityComparer<CakeLayer> CakeLayerComparer { get; } = new CakeLayerEqualityComparer();

    public string Name { get; set; }
    public string Description { get; set; }
    public string RecipeUrl { get; set; }
    public string[] PossibleLayers { get; set; }
}