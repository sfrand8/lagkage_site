using AutoFixture.Xunit2;
using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.MinimalAPI.Features;
using Lagkage.UnitTests;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Lagkage.Integration.Tests.MinimalAPI;

public class GetCakeLayersTests
{
    [Theory, AutoMoqControllerData]
    public async Task ReturnsNotFoundWhenNoLayersFound(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock)
    {
        // Arrange
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayers()).ReturnsAsync(new List<CakeLayer>());

        // Act
        var result = await GetCakeLayers.HandleRequest(cakeLayerRepositoryMock.Object);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory, AutoMoqControllerData]
    public async Task ReturnsOkWithListOfLayers(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        List<CakeLayer> cakeLayers)
    {
        // Arrange
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayers()).ReturnsAsync(cakeLayers);

        // Act
        var result = await GetCakeLayers.HandleRequest(cakeLayerRepositoryMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<IEnumerable<HttpCakeLayer>>>(result.Result);
        var response = okResult.Value;

        Assert.NotNull(response);
        Assert.All(cakeLayers, cakeLayer => 
            Assert.Contains(cakeLayer.ToHttpModel(), response));

        cakeLayerRepositoryMock.Verify(x => x.GetCakeLayers(), Times.Once);
    }
}