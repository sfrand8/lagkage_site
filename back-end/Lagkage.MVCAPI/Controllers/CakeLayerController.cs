using Lagkage.Contracts.Http;
using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace Lagkage.Controllers;

[ApiController]
[Route("controller/api/cakelayers")]
public class CakeLayerController : ControllerBase
{
    private readonly ICakeLayerRepository _cakeLayerRepository;
    
    public CakeLayerController(ICakeLayerRepository cakeLayerRepository)
    {
        _cakeLayerRepository = cakeLayerRepository;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCakeLayersResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetCakeLayersResponse>> GetCakeLayers()
    {
        var resp = await _cakeLayerRepository.GetCakeLayers();

        if (!resp.Any())
        {
            return NotFound(); // MVC-style response
        }

        var response = new GetCakeLayersResponse
        {
            CakeLayers = resp.Select(x => x.ToHttpModel())
        };

        return Ok(response); // MVC-style response
    }

    [HttpPost]   
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    public async Task<ActionResult<string>> AddCakeLayer(AddCakeLayerRequest request)
    {
        CakeLayer cakeLayer = request.HttpCakeLayer.ToDomain();
        await _cakeLayerRepository.AddCakeLayer(request.HttpCakeLayer.ToDomain());
        
        return Created($"{Request.Path}/{cakeLayer.Id.Value}", cakeLayer.Id.Value.ToString());
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCakeLayer([FromRoute] Guid id)
    {
        await _cakeLayerRepository.DeleteCakeLayer(new CakeLayerId(id));
        return NoContent();
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpCakeLayer))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HttpCakeLayer>> GetCakeLayerById([FromRoute] Guid id)
    {
        CakeLayer? cakeLayer = await _cakeLayerRepository.GetCakeLayerById(new CakeLayerId(id));

        if (cakeLayer == null)
        {
            return NotFound();
        }
        
        return Ok(cakeLayer.ToHttpModel());
    }
}