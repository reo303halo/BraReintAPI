using bra_reint_API.Models;
using bra_reint_API.Models.ViewModel;
using bra_reint_API.Services.PostalCodeServices;
using Microsoft.AspNetCore.Mvc;

namespace bra_reint_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostalCodeController(IPostalCodeService service) : Controller
{
    // GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPostalCodeViewModel>>> GetAllowedPostalCodes()
    {
        var postalCodes = await service.GetPostalCodesAsync();
        var result = postalCodes.Select(pc => new GetPostalCodeViewModel
        {
            Code = pc.Code,
            City = pc.City
        });

        return Ok(result);
    }
    
    // CREATE
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] GetPostalCodeViewModel postalCode)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingCode = service.GetPostalCodeAsync(postalCode.Code).Result;
        if (existingCode != null) return Conflict("The code already exists.");

        var newPostalCode = new PostalCode
        {
            Code = postalCode.Code,
            City = postalCode.City
        };
        
        await service.Save(newPostalCode);
        return Ok($"Postal code {postalCode} added to service area.");
    }
    
    // DELETE
    [HttpDelete("{code}")]
    public async Task<ActionResult> Delete(string code)
    {
        var existingCode = await service.GetPostalCodeAsync(code);
        if (existingCode == null) return NotFound("Postal code not found.");

        await service.Delete(code);
        return Ok($"Postal code {code} removed from service area.");
    }
}