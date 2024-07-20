using LeafLINQWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace LeafLINQWebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlantsController : ControllerBase
{

    private readonly LeafLINQContext _context;
    public PlantsController(LeafLINQContext context)
    {
        _context = context;

    }

    [HttpGet("plant")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IEnumerable<Plant>>> GetSpecifiedPlant([FromQuery]string id)
    {
        
        var plant = await _context.Plant.FirstOrDefaultAsync(x => x.Id == Int32.Parse(id));
        if(plant == null)
        {
            return NotFound();
        }

        return Ok(plant);
    }

    [HttpGet("plants")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IEnumerable<Plant>>> GetUserPlants()
    {
        var nameIdentifierClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            return BadRequest("NameIdentifier claim not found.");
        }

        // Retrieve the user's ID from the NameIdentifier claim
        var userIdValue = nameIdentifierClaim.Value;
        List<Plant> userPlants = null;
        
        if (userIdValue != null)
        {
            var userId = Int32.Parse(userIdValue);
            userPlants = await _context.Plant
            .Where(p => p.UserId == userId)
            .Select(p => new Plant
            {
                Id = p.Id,
                Name = p.Name,
                Desc = p.Desc,
                PicUrl = p.PicUrl,
                Location = p.Location,
                Level = p.Level,
                UserId = p.UserId,
                LastWateredDate = p.LastWateredDate,
                HealthCheckStatus = p.HealthCheckStatus,
            })
            .ToListAsync();
        }


        // Returns null if user not found
        if (userIdValue == null)
        {
            return NotFound();
        }

        return Ok(userPlants);
    }

    [HttpPost("addPlant")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<Plant>> AddPlant([FromBody] Plant plant)
    {
        try
        {
            // Retrieve the user's ID from the claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest("NameIdentifier claim not found.");
            }

            var userIdValue = userIdClaim.Value;
            if (userIdValue == null)
            {
                return BadRequest("User ID not found in claims.");
            }

            // Parse the user ID
            var userId = int.Parse(userIdValue);

            // Set the user ID for the new plant
            plant.UserId = userId;

            // Add the new plant to the database
            _context.Plant.Add(plant);
            await _context.SaveChangesAsync();

            // Return the newly added plant with HTTP status code 201 (Created)
            return CreatedAtAction(nameof(GetUserPlants), new { id = plant.Id }, plant);
        }
        catch (Exception ex)
        {
            // Handle exceptions and return an error response
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding plant: {ex.Message}");
        }
    }


    [HttpPut("updatePlant")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateUsersPlant([FromBody] Plant updatedPlant)
    {
        // Retrieve the existing Plant from the database
        var existingPlant = await _context.Plant.FindAsync(updatedPlant.Id);

        if (existingPlant == null)
        {
            return NotFound(); // Return 404 Not Found if Plant with the specified ID is not found
        }

        // Update the existing Plant object with the new values
        existingPlant.Name = updatedPlant.Name;
        existingPlant.Desc = updatedPlant.Desc;
        existingPlant.PicUrl = updatedPlant.PicUrl; 
        existingPlant.Location = updatedPlant.Location; 
        existingPlant.Level = updatedPlant.Level;   
        existingPlant.LastWateredDate = updatedPlant.LastWateredDate;
        existingPlant.HealthCheckStatus = updatedPlant.HealthCheckStatus;
        existingPlant.UserId = updatedPlant.UserId;
        existingPlant.DeviceId = updatedPlant.DeviceId;

        try
        {
            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency conflicts if necessary
            return Conflict(); // Return 409 Conflict if concurrency conflict occurs
        }

        return NoContent();
    }

    [HttpPatch("editUserId")]

    public async Task<ActionResult> EditUserID(string plantId, int newUserId)
    {
        var plant = await _context.Plant.FirstOrDefaultAsync(x => x.Id == Int32.Parse(plantId));

        if (plant == null)
        {
            return BadRequest("Plant not found");
        }

        try
        {
            plant.UserId = newUserId;

            await _context.SaveChangesAsync();

        }catch(DbUpdateConcurrencyException)
        {
            return Conflict();
        }
        return NoContent();
    }

    [HttpGet("allPlants")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IEnumerable<Plant>>> GetAllPlants()
    {
        List<Plant> plants = await _context.Plant.ToListAsync();

        if(plants.Count > 0)
        {
            return Ok(plants);
        }
        return NotFound();
    }
}
