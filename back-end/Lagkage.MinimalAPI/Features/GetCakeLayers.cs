using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lagkage.MinimalAPI.Features;

public static class GetCakeLayers
{
    public record GetCakeLayersRequest : IRequest<MedidatRResult<IEnumerable<CakeLayer>>>{}

    public class GetCakeLayersHandler : IRequestHandler<GetCakeLayersRequest, MedidatRResult<IEnumerable<CakeLayer>>>
    {
        private readonly ICakeLayerRepository _repository;

        public GetCakeLayersHandler(ICakeLayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<MedidatRResult<IEnumerable<CakeLayer>>> Handle(GetCakeLayersRequest request, CancellationToken cancellationToken)
        {
            return MedidatRResult<IEnumerable<CakeLayer>>.Success(await _repository.GetCakeLayers());
        }
    } 
    
    public static void MapGetCakeLayersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("minimalapis/api/cakelayers", HandleHttpRequest)
            .WithTags("GetCakelayers")
            .WithName("GetCakelayers")
            .WithDescription("Get all cake layers that can be added to cakes")
            .AllowAnonymous();
    }

    public static async Task<Results<Ok<GetCakeLayersResponse>, NotFound>> HandleHttpRequest(IMediator mediator)
    {
        var result = await mediator.Send(new GetCakeLayersRequest());

        if (!result.IsSuccess)
        {
            throw new UnhandledErrorException("Non successful get cake layers request", result.Error);
        }
        
        var cakeLayers = result.Value.ToList();
        if (cakeLayers.Count == 0)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(new GetCakeLayersResponse{CakeLayers = cakeLayers.Select(x => x.ToHttpModel())});
    }
}