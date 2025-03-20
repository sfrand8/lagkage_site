using Lagkage.Contracts.Interfaces;
using Lagkage.Contracts.Models;
using Lagkage.Controllers.Models;
using Lagkage.Database;
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
    public async Task<ActionResult<IEnumerable<CakeLayer>>> GetCakeLayers()
    {
        var resp = await _cakeLayerRepository.GetCakeLayers();
        if (!resp.Any())
        {
            return NotFound();
        }
        return Ok(resp);
    }

    [HttpPost]
    public async Task<ActionResult> AddCakeLayer(AddCakeLayerRequest request)
    {
        await _cakeLayerRepository.AddCakeLayer(request.ToDomain());
        return Ok();
    }

    [HttpDelete]
    [Route("/{id}")]
    public async Task<ActionResult> DeleteCakeLayer([FromRoute] Guid id)
    {
        await _cakeLayerRepository.DeleteCakeLayer(new CakeLayerId(id));
        return Ok();
    }
}