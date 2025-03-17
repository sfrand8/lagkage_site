using System.Data;
using Dapper;
using Lagkage.Models;
using Npgsql;

namespace Lagkage.Database;

public class CakeLayerRepository
{
    private readonly IDbConnection _db;

    public CakeLayerRepository(IDbConnection connection)
    {
      _db = connection;
    }
    public async Task<IEnumerable<CakeLayer>> GetCakeLayers()
    {
        return await _db.QueryAsync<CakeLayer>("SELECT * FROM cakelayers");
    }

    public Task AddCakeLayer(CakeLayer cakeLayer)
    {
        return _db.ExecuteAsync(@"INSERT INTO cakelayers (name, description, recipe_url, possible_layers)
                                            VALUES (@Name,@Description,@RecipeUrl,@PossibleLayers)", cakeLayer);
    }
}

