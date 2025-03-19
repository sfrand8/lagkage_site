using System.Data;
using Dapper;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Npgsql;

namespace Lagkage.Database;

public class CakeLayerRepository : ICakeLayerRepository
{
    private readonly IDbConnection _db;

    public CakeLayerRepository(IDbConnection connection)
    {
      _db = connection;
    }


    public Task AddCakeLayer(CakeLayer cakeLayer)
    {
        return _db.ExecuteAsync(@"INSERT INTO cakelayers (name, description, recipe_url, possible_layers)
                                            VALUES (@Name,@Description,@RecipeUrl,@PossibleLayers)", cakeLayer);
    }

    public async Task<IEnumerable<CakeLayer>> GetCakeLayers()
    {
        return await _db.QueryAsync<CakeLayer>("SELECT * FROM cakelayers");
    }
}



