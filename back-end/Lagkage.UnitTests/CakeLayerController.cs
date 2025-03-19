using AutoFixture.Xunit2;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lagkage.UnitTests;

public class CakeLayerController
{
    [Theory, AutoMoqController]
    public async Task ReturnsNotFoundWhenNoLayersFound(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        Controllers.CakeLayerController sut)
    {
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayers()).ReturnsAsync(new List<CakeLayer>());
        
        ActionResult<IEnumerable<CakeLayer>> result = await sut.GetCakeLayers();
        
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.Null(result.Value);
        cakeLayerRepositoryMock.Verify(x => x.GetCakeLayers(), Times.Once);
    }

    [Theory, AutoMoqController]
    public async Task ReturnsOkWithListOfLayers(
        [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
        List<CakeLayer> cakeLayers,
        Controllers.CakeLayerController sut)
    {
        cakeLayerRepositoryMock.Setup(x => x.GetCakeLayers()).ReturnsAsync(cakeLayers);
        ActionResult<IEnumerable<CakeLayer>> result = await sut.GetCakeLayers();
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        var returnedLayers = Assert.IsAssignableFrom<IEnumerable<CakeLayer>>(okResult.Value);
        Assert.True(cakeLayers.TrueForAll(x => returnedLayers.Any(
            y => y.Equals(x))));
        cakeLayerRepositoryMock.Verify(x => x.GetCakeLayers(), Times.Once);
    }
}