using LeafLINQWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using LeafLINQWebAPI.DTOs;

namespace LeafLINQWebAPI.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly LeafLINQContext _context;

    public UserController(LeafLINQContext context)
    {
        _context = context;
    }


    [HttpGet("user")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<User>> GetUserAsync()
    {
        var nameIdentifierClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            return BadRequest();
        }

        // Retrieve the user's ID from the NameIdentifier claim
        var userIdValue = nameIdentifierClaim.Value;
        int userId = 0;
        try
        {
            //Parse to int from nameIdentifier claim value
            userId = Int32.Parse(userIdValue);
        } catch
        {
            return BadRequest("Invalid JWT Token");
        }

        var user = await _context.User
                          .Where(u => u.Id == userId)
                          .Select(u => new
                          {
                              u.Id,
                              u.FullName,
                              u.Email,
                              u.PicUrl,
                              //u.Desc,
                              u.LastLoginDate,
                              u.Block
                          })
                          .FirstOrDefaultAsync();

        if (user == null)
        {
            return BadRequest();
        }

        return Ok(user);
    }

    [HttpPut("userUpdate")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult> UpdateUserInfoAsync([FromBody] UserUpdateDTO newUserInfo)
    {
        var currentUserInfo = await _context.User.FindAsync(newUserInfo.Id);
        var nameIdentifierClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            return BadRequest("Bad JWT Token");
        }

        // Retrieve the user's ID from the NameIdentifier claim
        var userIdValue = nameIdentifierClaim.Value;
         
        if (currentUserInfo == null || !userIdValue.Equals(newUserInfo.Id.ToString()))
        {
            return BadRequest("Cannot change other users information in the database.");
        }

        currentUserInfo.Id = newUserInfo.Id;
        currentUserInfo.FullName = newUserInfo.FullName;
        currentUserInfo.Email = newUserInfo.Email;  
        currentUserInfo.PicUrl = newUserInfo.PicUrl;
        

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

    [HttpGet("settings")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<Setting>> GetSettings()
    {
        var nameIdentifierClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);

        if (nameIdentifierClaim == null)
        {
            return BadRequest();
        }

        // Retrieve the user's ID from the NameIdentifier claim
        var userIdValue = nameIdentifierClaim.Value;
        int userId = 0;
        try
        {
            //Parse to int from nameIdentifier claim value
            userId = Int32.Parse(userIdValue);

            if(userId > 0)
            {
                var setting = await _context.Setting.FirstOrDefaultAsync(x => x.UserId == userId);
                
                if (setting != null)
                {
                    return Ok(setting);
                } else
                {
                    return BadRequest($"Could not locate setting record for user {userId}");
                }

            } else
            {
                return BadRequest("Invalid JWT Token. Could not find user");
            }
        }
        catch
        {
            return BadRequest("Invalid JWT Token");
        }

    }

    [HttpPatch("logout")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult> Logout([FromQuery] string sessionId)
    {
        var session = _context.Session.Where(x => x.SessionId.Equals(sessionId)).FirstOrDefault();

        if (session == null)
        {
            return BadRequest();
        }

        try
        {
            _context.Session.Remove(session);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }

        return NoContent();

    }
}