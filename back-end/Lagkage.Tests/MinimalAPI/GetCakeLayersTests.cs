using AutoFixture.Xunit2;
using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.MinimalAPI.Features;
using Lagkage.UnitTests;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Lagkage.Integration.Tests.MinimalAPI;

public class GetCakeLayersTests
{
    public class HttpEndpointTests
    {
        [Theory, AutoMoqControllerData]
        public async Task ReturnsNotFoundWhenNoLayersFound(
            Mock<IMediator> mediatorMock)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<MedidatRResult<IEnumerable<CakeLayer>>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<IEnumerable<CakeLayer>>.Success(new List<CakeLayer>()));

            // Act
            var result = await GetCakeLayers.HandleRequest(mediatorMock.Object);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }

        [Theory, AutoMoqControllerData]
        public async Task ReturnsOkWithListOfLayers(
            Mock<IMediator> mediatorMock,
            List<CakeLayer> cakeLayers)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<MedidatRResult<IEnumerable<CakeLayer>>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<IEnumerable<CakeLayer>>.Success(cakeLayers));

            // Act
            var result = await GetCakeLayers.HandleRequest(mediatorMock.Object);

            // Assert
            var okResult = Assert.IsType<Ok<IEnumerable<HttpCakeLayer>>>(result.Result);
            var response = okResult.Value;

            Assert.NotNull(response);
            Assert.All(cakeLayers, cakeLayer =>
                Assert.Contains(cakeLayer.ToHttpModel(), response));

            mediatorMock.Verify(x => x.Send(new GetCakeLayers.GetCakeLayersRequest(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class CommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ReturnsRetrievedLayersFromRepository(
            [Frozen] Mock<ICakeLayerRepository> repositoryMock,
            [Frozen] IEnumerable<CakeLayer> cakeLayers,
            GetCakeLayers.GetCakeLayersHandler sut)
        {
            // Act
            var response = await sut.Handle(new GetCakeLayers.GetCakeLayersRequest(), CancellationToken.None);
            
            // Assert
            repositoryMock.Verify(x => x.GetCakeLayers(), Times.Once);
            Assert.Equal(cakeLayers, response.Value.ToList());
        }
    }
}