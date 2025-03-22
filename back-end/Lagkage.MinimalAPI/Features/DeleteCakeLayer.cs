using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

public static class DeleteCakeLayer
{
    public record DeleteCakeLayerCommand :  IRequest<MedidatRResult<Unit>>
    {
        public CakeLayerId Id { get; init; }
    } 

    public class AddCakeLayerHandler : IRequestHandler<DeleteCakeLayerCommand, MedidatRResult<Unit>>
    {
        private readonly ICakeLayerRepository _repository;

        public AddCakeLayerHandler(ICakeLayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<MedidatRResult<Unit>> Handle(DeleteCakeLayerCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteCakeLayer(request.Id);
            return MedidatRResult<Unit>.Success();
        }
    }
    
    public static void MapDeleteCakeLayerEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("minimalapis/api/cakelayers/{id}", HandleHttp)
            .WithTags("DeleteCakeLayers")
            .WithName("Delete Cake Layer")
            .WithDescription("Deletes a cake layer from the list of possible cake layers")
            .AllowAnonymous();
    }

    public static async Task<Ok> HandleHttp(
        IMediator mediator, Guid id)
    {
        var result = await mediator.Send(new DeleteCakeLayerCommand{ Id =  new CakeLayerId(id)});

        if (!result.IsSuccess)
        {
            throw new UnhandledErrorException("Error type not handled in AddCakeLayerEndpoint", result.Error);
        }

        return TypedResults.Ok();
    }
}