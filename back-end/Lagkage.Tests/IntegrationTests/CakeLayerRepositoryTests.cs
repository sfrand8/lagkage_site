using AutoFixture.Xunit2;
using Lagkage.Contracts.Models;
using Lagkage.Database;

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
    public async Task GetCakeLayers_ReturnEmptyListWhenNoneSaved()
    {
        _databaseFixture.TruncateAllTables();
        var sut = new CakeLayerRepository(_databaseFixture.Connection);

        var layers = await sut.GetCakeLayers();
        
        Assert.Empty(layers);
    }

    [Theory, AutoData]
    public async Task GetCakeLayers_ReturnSavedCakeLayersWhenSaved(CakeLayer cakeLayer)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        await sut.AddCakeLayer(cakeLayer);
        var layer = await sut.GetCakeLayerById(cakeLayer.Id);
        
        Assert.True(cakeLayer.Equals(layer));
    }
    
    [Theory, AutoData]
    public async Task GetCakeLayerById_ReturnNullWhenNotSaved(CakeLayerId id)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        var layer = await sut.GetCakeLayerById(id);
        
        Assert.Null(layer);
    }
    
    [Theory, AutoData]
    public async Task GetCakeLayerById_ReturnSavedCakeLayerWhenSaved(CakeLayer cakeLayer)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        await sut.AddCakeLayer(cakeLayer);
        var layer = await sut.GetCakeLayerById(cakeLayer.Id);
        
        Assert.True(cakeLayer.Equals(layer));
    }  
    
    [Theory, AutoData]
    public async Task AddCakeLayer_ReturnOneWhenAddingNew(CakeLayer cakeLayer)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        var result = await sut.AddCakeLayer(cakeLayer);
        
        Assert.Equal(1, result);
    }
    
    [Theory, AutoData]
    public async Task AddCakeLayer_ReturnZeroWhenAddingDuplicate(CakeLayer cakeLayer)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        await sut.AddCakeLayer(cakeLayer);
        var result = await sut.AddCakeLayer(cakeLayer);
        
        Assert.Equal(0, result);
    }
    
    [Theory, AutoData]
    public async Task DeleteLayer(CakeLayer cakeLayer)
    {
        var sut = new CakeLayerRepository(_databaseFixture.Connection);
        
        await sut.AddCakeLayer(cakeLayer);
        await sut.DeleteCakeLayer(cakeLayer.Id);
        var layer = await sut.GetCakeLayerById(cakeLayer.Id);
        
        Assert.Null(layer);
    }
}