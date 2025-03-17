using AutoFixture.Xunit2;
using Lagkage.Database;
using Lagkage.Models;

namespace Lagkage.Integration.Tests;

[Collection("Database collection")]
public class CakeLayerRepositoryTests
{
    private readonly DatabaseFixture _databaseFixture;

    public CakeLayerRepositoryTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    [Fact]
    public async Task ReturnEmptyListWhenNoneSaved()
    {
        _databaseFixture.TruncateAllTables();
        var sut = new CakeLayerRepository(_databaseFixture.Connection);

        var layers = await sut.GetCakeLayers();
        
        Assert.Empty(layers);
    }

    [Theory]
    [AutoData]
    public async Task ReturnSavedCakeLayersWhenSaved(CakeLayer cakeLayer)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        await sut.AddCakeLayer(cakeLayer);
        var layers = await sut.GetCakeLayers();
        
        Assert.Contains(layers, x =>
            x.Name == cakeLayer.Name &&
            x.RecipeUrl == cakeLayer.RecipeUrl &&
            x.Description == cakeLayer.Description &&
           new HashSet<string>(x.PossibleLayers).SetEquals(cakeLayer.PossibleLayers)
        );
    }
}