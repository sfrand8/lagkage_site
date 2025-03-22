using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

public static class GetCakeLayerById
{
    public record GetCakeLayerByIdRequest : IRequest<MedidatRResult<CakeLayer>>
    {
        public CakeLayerId Id { get; init; }
    }

    public class GetCakeLayersHandler : IRequestHandler<GetCakeLayerByIdRequest, MedidatRResult<CakeLayer>>
    {
        private readonly ICakeLayerRepository _repository;

        public GetCakeLayersHandler(ICakeLayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<MedidatRResult<CakeLayer>> Handle(GetCakeLayerByIdRequest request, CancellationToken cancellationToken)
        {
            var cakeLayer = await _repository.GetCakeLayerById(request.Id);
            if (cakeLayer == null)
            {
                return MedidatRResult<CakeLayer>.Failure(CakeLayerErrors.CakelayerNotfound);
            }
            return MedidatRResult<CakeLayer>.Success(cakeLayer);
        }
    } 
    
    public static void MapGetCakeLayersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("minimalapis/api/cakelayer/{id}", HandleHttpRequest)
            .WithTags("GetCakelayer")
            .WithName("Get a Cake layer")
            .WithDescription("Get a cake layer that can be added to cakes")
            .AllowAnonymous();
    }

    public static async Task<Results<Ok<HttpCakeLayer>, NotFound>> HandleHttpRequest(IMediator mediator, Guid id)
    {
        var result = await mediator.Send(new GetCakeLayerByIdRequest{ Id = new CakeLayerId(id) });

        if (!result.IsSuccess)
        {
            switch (result.Error.ErrorType)
            {
                case ErrorType.NotFound:
                    return TypedResults.NotFound();
            }
            throw new UnhandledErrorException("Non successful get cake layers request", result.Error);
        }
        
        if (result.Value == null)
        {
            throw new UnhandledErrorException("Cake layer was null after retrieval", result.Error);
        }
        
        return TypedResults.Ok(result.Value.ToHttpModel());
    }
}