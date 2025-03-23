using AutoFixture.Xunit2;
using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lagkage.Integration.Tests.UnitTests.MVCAPI;

public class GetCakeLayersControllerTests
{
    [Theory, AutoMoqControllerData]
    public async Task ReturnsNotFoundWhenNoLayersFound(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        Controllers.CakeLayerController sut)
    {
        // Arrange
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayers()).ReturnsAsync(new List<CakeLayer>());
        
        // Act
        var result = await sut.GetCakeLayers();
        
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        cakeLayerRepositoryMock.Verify(x => x.GetCakeLayers(), Times.Once);
    }

    [Theory, AutoMoqControllerData]
    public async Task ReturnsOkWithListOfLayers(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        List<CakeLayer> cakeLayers,
        Controllers.CakeLayerController sut)
    {
        // Arrange
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayers()).ReturnsAsync(cakeLayers);

        // Act
        var result = await sut.GetCakeLayers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetCakeLayersResponse>(okResult.Value);
    
        Assert.All(cakeLayers, cakeLayer => 
            Assert.Contains(cakeLayer.ToHttpModel(), response.CakeLayers));

        cakeLayerRepositoryMock.Verify(x => x.GetCakeLayers(), Times.Once);
    }
}

public class AddCakeLayerControllerTests
{
    [Theory, AutoMoqControllerData]
    public async Task ReturnsCreatedWithExpectedID(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        AddCakeLayerRequest request,
        Controllers.CakeLayerController sut)
    {
        // Arrange
        var expectedID = new CakeLayerId(new CakeLayerName(request.HttpCakeLayer.Name)).Value.ToString(); 
        
        // Act
        var result = await sut.AddCakeLayer(request);
        
        // Arrange
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(expectedID, createdResult.Value);
        Assert.Contains(expectedID, createdResult.Location);
        cakeLayerRepositoryMock.Verify(x => x.AddCakeLayer(request.HttpCakeLayer.ToDomain()));
    }
}

public class DeleteCakeLayerControllerTests
{
    [Theory, AutoMoqControllerData]
    public async Task ReturnsNoContent(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        Guid id,
        Controllers.CakeLayerController sut)
    {
        // Act
        var result = await sut.DeleteCakeLayer(id);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
        cakeLayerRepositoryMock.Verify(x => x.DeleteCakeLayer(
            It.Is<CakeLayerId>(cakeLayerId => cakeLayerId.Value.ToString() == id.ToString())), Times.Once);
    }
}

public class GetCakeLayerControllerTests
{
    [Theory, AutoMoqControllerData]
    public async Task ReturnsNotFoundWhenNullResponseFromRepository(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        Guid id,
        Controllers.CakeLayerController sut)
    {
        // Arrange 
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayerById(It.IsAny<CakeLayerId>())).ReturnsAsync((CakeLayer?)null);
        
        // Act
        var result = await sut.GetCakeLayerById(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Theory, AutoMoqControllerData]
    public async Task ReturnsCakeLayerWhenFound(
        [Frozen] CakeLayer cakeLayer,
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        Guid id,
        Controllers.CakeLayerController sut)
    {
        // Act
        var result = await sut.GetCakeLayerById(id);
        
        // Assert
        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
        Assert.Equal(cakeLayer.ToHttpModel(), okResult.Value);
    }
}