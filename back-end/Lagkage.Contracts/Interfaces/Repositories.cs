﻿using Lagkage.Contracts.Models;

namespace Lagkage.Contracts.Interfaces;

public interface ICakeLayerRepository
{
    Task<int> AddCakeLayer(CakeLayer cakeLayer);
    Task<IEnumerable<CakeLayer>> GetCakeLayers();
    Task DeleteCakeLayer(CakeLayerId id);
}