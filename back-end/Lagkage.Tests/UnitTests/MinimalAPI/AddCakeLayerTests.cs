using AutoFixture.Xunit2;
using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.MinimalAPI.Features;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Lagkage.Integration.Tests.UnitTests.MinimalAPI;

public class AddCakeLayerTests
{
    public class HttpEndpointTests
    {
        [Theory, AutoMoqControllerData]
        public async Task ReturnWithCreatedIdFromCommandHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            AddCakeLayerRequest cakeLayerToAdd,
            CakeLayerId expectedId)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<AddCakeLayer.AddCakeLayerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<CakeLayerId>.Success(expectedId));

            // Act
            var result = await AddCakeLayer.HandleHttpRequest(mediatorMock.Object, cakeLayerToAdd);

            // Assert
            var createdResult = Assert.IsAssignableFrom<Created<AddCakeLayerResponse>>(result.Result);
            Assert.Equal(expectedId.Value.ToString(), createdResult.Value.CakeLayerId);
        }
        
        [Theory, AutoMoqControllerData]
        public async Task ReturnWithConflictWhenConflictErrorReturnedFromCommandHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            AddCakeLayerRequest cakeLayerToAdd,
            CakeLayerId expectedId)
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<AddCakeLayer.AddCakeLayerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MedidatRResult<CakeLayerId>.Failure(CakeLayerErrors.CakeLayerAlreadyExists));

            // Act
            var result = await AddCakeLayer.HandleHttpRequest(mediatorMock.Object, cakeLayerToAdd);

            // Assert
            var conflictResult = Assert.IsAssignableFrom<Conflict<string>>(result.Result);
            Assert.Equal(CakeLayerErrors.CakeLayerAlreadyExists.Message, conflictResult.Value);
        }
    }
    
    public class CommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task AddCakeLayerToRepository(
            [Frozen] Mock<ICakeLayerRepository> cakeLayerRepositoryMock,
            AddCakeLayer.AddCakeLayerCommand command,
            AddCakeLayer.AddCakeLayerHandler sut)
        {
            // Arrange
            cakeLayerRepositoryMock.Setup(x => x.AddCakeLayer(It.IsAny<CakeLayer>()))
                .ReturnsAsync(1);
            var expectedCakeLayer = new CakeLayer(
                new CakeLayerId(command.Name), 
                command.Name,
                command.Description, 
                command.RecipeUrl, 
                command.PossibleLayers);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            cakeLayerRepositoryMock.Verify(x => x.AddCakeLayer(expectedCakeLayer), Times.Once);
            Assert.Equal(expectedCakeLayer.Id.Value, result.Value.Value);
        }
    }
}