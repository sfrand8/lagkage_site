using Lagkage.Contracts.Models;

namespace Lagkage.Contracts.Interfaces;

public interface ICakeLayerRepository
{
    Task AddCakeLayer(CakeLayer cakeLayer);
    Task<IEnumerable<CakeLayer>> GetCakeLayers();
}