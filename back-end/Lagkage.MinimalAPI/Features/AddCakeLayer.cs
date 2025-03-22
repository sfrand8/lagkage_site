using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

public static class AddCakeLayer
{
    public record AddCakeLayerCommand :  IRequest<MedidatRResult<CakeLayerId>>
    {
        public CakeLayerName Name { get; init; }
        public CakeLayerDescription Description { get; init; }
        public CakeLayerRecipeUrl RecipeUrl { get; init; }
        public CakeLayerPossibility[] PossibleLayers { get; init; }
    } 

    public class AddCakeLayerHandler : IRequestHandler<AddCakeLayerCommand, MedidatRResult<CakeLayerId>>
    {
        private readonly ICakeLayerRepository _repository;

        public AddCakeLayerHandler(ICakeLayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<MedidatRResult<CakeLayerId>> Handle(AddCakeLayerCommand request, CancellationToken cancellationToken)
        {
            var cakeLayer = new CakeLayer(new CakeLayerId(request.Name), request.Name, request.Description, request.RecipeUrl, request.PossibleLayers);
            var affectedRows = await _repository.AddCakeLayer(cakeLayer);
            
            if (affectedRows == 0)
            {
                return MedidatRResult<CakeLayerId>.Failure(CakeLayerErrors.CakeLayerAlreadyExists);
            }

            return MedidatRResult<CakeLayerId>.Success(cakeLayer.Id);
        }
    }
    
    public static void MapAddCakeLayerEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("minimalapis/api/cakelayers", AddCakeLayers)
            .WithTags("AddCakeLayers")
            .WithName("Add Cake Layer")
            .WithDescription("Adds a cake layer to the list of possible cake layers. Name has to be a new unique name for the layer.")
            .AllowAnonymous();
    }

    public static async Task<Results<Created<string>, Conflict<string>>> AddCakeLayers(
        IMediator mediator, AddCakeLayerRequest request)
    {
        var cmd = new AddCakeLayerCommand
        {
            Name = new CakeLayerName(request.HttpCakeLayer.Name),
            Description = new CakeLayerDescription(request.HttpCakeLayer.Description),
            RecipeUrl = new CakeLayerRecipeUrl(request.HttpCakeLayer.RecipeUrl),
            PossibleLayers = request.HttpCakeLayer.PossibleLayers,
        };
        
        var result = await mediator.Send(cmd);

        if (!result.IsSuccess)
        {
            switch (result.Error.ErrorType)
            {
                case ErrorType.Conflict:
                    return TypedResults.Conflict(result.Error.Message);
                default:
                    throw new UnhandledErrorException("Error type not handled in AddCakeLayerEndpoint", result.Error);
            }
        }
        
        var location = $"/minimalapis/api/cakelayers/{result.Value.Value.ToString()}";
    
        return TypedResults.Created(location, result.Value.Value.ToString());
    }
}