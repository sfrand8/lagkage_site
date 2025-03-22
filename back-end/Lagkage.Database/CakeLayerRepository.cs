using System.Data;
using Dapper;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.Database.Mappers;
using Npgsql;

namespace Lagkage.Database;

public class CakeLayerRepository : ICakeLayerRepository
{
    private readonly IDbConnection _db;

    public CakeLayerRepository(IDbConnection connection)
    {
      _db = connection;
    }

    public async Task<int> AddCakeLayer(CakeLayer cakeLayer)
    {
        const string query = """
                             INSERT INTO cake_layers (id, name, description, recipe_url, possible_layers)
                                                                         VALUES (@Id, @Name, @Description, @RecipeUrl, @PossibleLayers) 
                                                                         ON CONFLICT (id) DO NOTHING 
                                                                         RETURNING 1;
                             """;
        
        return (await _db.QueryAsync<int?>(query, cakeLayer.MapToDTO())).SingleOrDefault() ?? 0;
    }

    public async Task<IEnumerable<CakeLayer>> GetCakeLayers()
    {
        const string query = "SELECT * FROM cake_layers";
        return (await _db.QueryAsync<CakeLayerDTO>(query)).Select(CakeLayerMapper.MapToDomain);
    }

    public async Task<CakeLayer?> GetCakeLayerById(CakeLayerId id)
    {
        const string query = "SELECT * FROM cake_layers WHERE id = @Id";
        
        return ( await _db.QueryAsync<CakeLayerDTO>(query, new { Id = id.Value.ToString() }))
            .SingleOrDefault()?.MapToDomain();
    }

    public Task DeleteCakeLayer(CakeLayerId id)
    {
        const string query = "DELETE FROM cake_layers WHERE id = @Id";
        
        return _db.ExecuteAsync(query, new { Id = id.Value.ToString() });
    }
}

public class CakeLayerDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string RecipeUrl { get; set; }
    public string[] PossibleLayers { get; set; }
}



