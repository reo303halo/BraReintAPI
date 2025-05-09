using bra_reint_API.Models;
using bra_reint_API.Models.ViewModel;
using bra_reint_API.Services.BookingTypeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bra_reint_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingTypeController(IBookingTypeService service) : ControllerBase
{
    // GET ALL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBookingTypeViewModel>>> GetBookingTypes()
    {
        var types = await service.GetBookingTypesAsync();

        var typesViewModel = types
            .Select(t => new GetBookingTypeViewModel
            {
                Id = t.Id,
                TypeName = t.TypeName,
                Description = t.Description,
                Price = t.Price
            }).ToList();
        
        return Ok(typesViewModel);
    }
    
    // GET BY ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetBookingTypeViewModel>> GetBookingType(int id)
    {
        var bookingType = await service.GetBookingType(id);
        
        if (bookingType == null) return NotFound($"No booking type with id {id} was found.");
        
        var bookingTypeViewModel = new GetBookingTypeViewModel
        {
            Id = bookingType.Id,
            TypeName = bookingType.TypeName,
            Description = bookingType.Description,
            Price = bookingType.Price
        };
            
        return Ok(bookingTypeViewModel);
    }
    
    // CREATE
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GetBookingTypeViewModel>> Create([FromBody] CreateBookingTypeViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingBookingType = service.GetBookingType(model.TypeName).Result;
        if (existingBookingType != null) return Conflict("Booking type already exists.");

        var newBookingType = new BookingType
        {
            TypeName = model.TypeName,
            Description = model.Description,
            Price = model.Price
        };
        
        await service.Save(newBookingType);
        return CreatedAtAction(nameof(GetBookingType), new { id = newBookingType.Id }, model);
    }
    
    // DELETE
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var bookingType = await service.GetBookingType(id);
        if (bookingType == null) return NotFound($"No booking type with id {id} was found.");
        
        await service.Delete(id);
        return Ok($"Booking type with id {id} deleted.");
    }
}