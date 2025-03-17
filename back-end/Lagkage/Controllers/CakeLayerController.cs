using Lagkage.Database;
using Lagkage.Models;
using Microsoft.AspNetCore.Mvc;
namespace Lagkage.Controllers;

[ApiController]
[Route("api/cakelayers")]
public class CakeLayerController : ControllerBase
{
    private readonly CakeLayerRepository _cakeLayerRepository;
    
    public CakeLayerController(CakeLayerRepository cakeLayerRepository)
    {
        _cakeLayerRepository = cakeLayerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CakeLayer>>> GetUsers()
    {
        return Ok(await _cakeLayerRepository.GetCakeLayers());
    }
}