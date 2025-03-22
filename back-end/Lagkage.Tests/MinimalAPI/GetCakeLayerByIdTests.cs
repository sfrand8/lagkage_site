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

public class GetCakeLayerByIdTests
{
    public class HttpEndpointTests
    {
        [Theory, AutoMoqControllerData]
        public async Task ReturnsNotFoundWhenLayerNotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            Guid id)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetCakeLayerById.GetCakeLayerByIdRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<CakeLayer>.Failure(CakeLayerErrors.CakelayerNotfound));

            // Act
            var result = await GetCakeLayerById.HandleHttpRequest(mediatorMock.Object, id);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }

        [Theory, AutoMoqControllerData]
        public async Task ReturnsOkWithLayer(
            [Frozen] Mock<IMediator> mediatorMock,
            CakeLayer cakeLayer,
            Guid id)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<GetCakeLayerById.GetCakeLayerByIdRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<CakeLayer>.Success(cakeLayer));

            // Act
            var result = await GetCakeLayerById.HandleHttpRequest(mediatorMock.Object, id);

            // Assert
            var okResult = Assert.IsType<Ok<HttpCakeLayer>>(result.Result);
            var response = okResult.Value;

            Assert.NotNull(response);
            Assert.Equal(cakeLayer.ToHttpModel(), response);
        }
    }

    public class CommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ReturnsLayerFromRepository(
            [Frozen] Mock<ICakeLayerRepository> repositoryMock,
            CakeLayer cakeLayer,
            GetCakeLayerById.GetCakeLayerByIdRequest request,
            GetCakeLayerById.GetCakeLayersHandler sut)
        {
            // Arrange
            repositoryMock.Setup(x => x.GetCakeLayerById(It.IsAny<CakeLayerId>()))
                .ReturnsAsync(cakeLayer);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            repositoryMock.Verify(x => x.GetCakeLayerById(It.IsAny<CakeLayerId>()), Times.Once);
            Assert.True(result.IsSuccess);
            Assert.Equal(cakeLayer, result.Value);
        }

        [Theory, AutoMoqData]
        public async Task ReturnsNotFoundWhenLayerNotFound(
            [Frozen] Mock<ICakeLayerRepository> repositoryMock,
            GetCakeLayerById.GetCakeLayerByIdRequest request,
            GetCakeLayerById.GetCakeLayersHandler sut)
        {
            // Arrange
            repositoryMock.Setup(x => x.GetCakeLayerById(It.IsAny<CakeLayerId>()))
                .ReturnsAsync((CakeLayer)null);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            repositoryMock.Verify(x => x.GetCakeLayerById(It.IsAny<CakeLayerId>()), Times.Once);
            Assert.False(result.IsSuccess);
            Assert.Equal(CakeLayerErrors.CakelayerNotfound, result.Error);
        }
    }
}