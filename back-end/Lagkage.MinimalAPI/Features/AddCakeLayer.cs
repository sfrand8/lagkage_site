using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

//Notes for other potential readers, I don't think using Mediator in Vertical slicing is a good idea, I was looking at what
//a few people suggested online for doing vertical slicing in .Net and gave it ago, but making this big of an overhead
//when you are deciding to more tightly couple your code in a feature by feature slicing seems like a big over-head.
//The idea with vertical slicing is also that the domain code in the feature is only triggered inside that feature,
//but a big gain with mediatoR is that you can trigger commands anywhere in the application which is simply not a big issue
//when doing vertical slicing.
//I do get test-ability of each different part of the feature, e.g. the command handler, the endpoint handler, etc.
//but you could just as well make a "commandHandler" which is just some interface you create in the feature and then
//inject that into the endpoint handler, and then test the command handler separately.
//You do get a nice pipeline setup with MediatoR that can be useful for some common functionality like logging or metrics,
//but I think you could just as well do that with a middleware pipeline in the API.
//Especially if you don't have any other I/O in the api.

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
        app.MapPost("minimalapis/api/cakelayers", HandleHttpRequest)
            .WithTags("AddCakeLayers")
            .WithName("Add Cake Layer")
            .WithDescription("Adds a cake layer to the list of possible cake layers. Name has to be a new unique name for the layer.")
            .AllowAnonymous();
    }

    public static async Task<Results<Created<AddCakeLayerResponse>, Conflict<string>>> HandleHttpRequest(
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
    
        return TypedResults.Created(location, new AddCakeLayerResponse{ CakeLayerId = result.Value.Value.ToString() });
    }
}