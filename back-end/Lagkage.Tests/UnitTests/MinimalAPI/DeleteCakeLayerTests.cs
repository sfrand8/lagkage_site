using AutoFixture.Xunit2;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.MinimalAPI.Features;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Lagkage.Integration.Tests.UnitTests.MinimalAPI;

public class DeleteCakeLayerTests
{
    public class HttpEndpointTests
    {
        [Theory, AutoMoqControllerData]
        public async Task ReturnNoContentWhenLayerDeleted(
            [Frozen] Mock<IMediator> mediatorMock,
            Guid id)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteCakeLayer.DeleteCakeLayerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<Unit>.Success(Unit.Value));

            // Act
            var result = await DeleteCakeLayer.HandleHttp(mediatorMock.Object, id);

            // Assert
            Assert.IsType<NoContent>(result);
        }
    }
    
    public class CommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task DeleteCakeLayerFromRepository(
            [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
            DeleteCakeLayer.DeleteCakeLayerCommand command,
            DeleteCakeLayer.AddCakeLayerHandler sut)
        {
            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            cakeLayerRepositoryMock.Verify(x => x.DeleteCakeLayer(It.IsAny<CakeLayerId>()), Times.Once);
            Assert.True(result.IsSuccess);
        }
    }
}