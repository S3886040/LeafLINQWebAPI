using LeafLINQWebAPI.DTOs;
using LeafLINQWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Data;
using PasswordGenerator;
using LeafLINQWebAPI.Services;
using Microsoft.Data.SqlClient;

namespace LeafLINQWebAPI.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{

    private readonly LeafLINQContext _context;
    private readonly EmailService  _emailService;
    public AdminController(LeafLINQContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserAndPlants>> GetUserAndPlantsAsync([FromQuery] int userId)
    {
        var user = await _context.User
                          .Where(u => u.Id == userId)
                          .Select(u => new UserModel
                          {
                              Id = u.Id,
                              FullName = u.FullName,
                              Email = u.Email,
                              //Desc = u.Desc,
                              PicUrl = u.PicUrl,
                              LastLoginDate = u.LastLoginDate,
                              UserType = u.UserType,
                              Block = u.Block,
                          })
                          .FirstOrDefaultAsync();

        UserAndPlants result = new UserAndPlants(); 
        result.user = user;
        result.plants = await _context.Plant
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

        if (user == null)
        {
            return BadRequest("Invalid User Id");
        }
        return Ok(result);
    }

    [HttpGet("userOnly")]
    public async Task<ActionResult<UserAndPlants>> GetAnyUser([FromQuery] int userId)
    {
        var user = await _context.User
                          .Where(u => u.Id == userId)
                          .Select(u => new UserModel
                          {
                              Id = u.Id,
                              FullName = u.FullName,
                              Email = u.Email,
                              PicUrl = u.PicUrl,
                              LastLoginDate = u.LastLoginDate,
                              UserType = u.UserType,
                              Block = u.Block,
                          })
                          .FirstOrDefaultAsync();

        if (user == null)
        {
            return BadRequest("Invalid User Id");
        }
        return Ok(user);
    }

    [HttpGet("userList")]
    public async Task<ActionResult<User>> GetUserListWithoutCallerAsync()
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
        }
        catch
        {
            return BadRequest("Invalid JWT Token");
        }

        var users = await _context.User
                          .Where(u => u.Id != userId)
                          .Where(u => u.UserType != 'S')
                          .Where(u => !u.FullName.Equals("Scheduler"))
                          .Select(u => new
                          {
                              u.Id,
                              u.FullName,
                              u.Email,
                              u.PicUrl,
                              u.LastLoginDate,
                              u.UserType,
                              u.Block
                          })
                          .ToListAsync();

        if (users == null)
        {
            return BadRequest();
        }

        return Ok(users);
    }

    [HttpGet("userListAll")]
    public async Task<ActionResult<User>> GetFullUserListAsync()
    {
        var users = await _context.User
                          .Where(u => u.UserType != 'S')
                          .Where(u => !u.FullName.Equals("Scheduler"))
                          .Select(u => new
                          {
                              u.Id,
                              u.FullName,
                              u.Email,
                              u.PicUrl,
                              u.LastLoginDate,
                              u.UserType,
                              u.Block
                          })
                          .ToListAsync();
        if (users == null )
        {
            return NotFound();
        }
        return Ok(users);
    }

    [HttpPut("adminUserUpdate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateUserInfoAsync([FromBody] UserModel newUserInfo)
    {

        var currentUserInfo = await _context.User.FindAsync(newUserInfo.Id);

        if (currentUserInfo == null)
        {
            return BadRequest("Invalid User information");
        }

        currentUserInfo.Id = newUserInfo.Id;
        currentUserInfo.FullName = newUserInfo.FullName;
        currentUserInfo.Email = newUserInfo.Email;
        currentUserInfo.PicUrl = newUserInfo.PicUrl;
        currentUserInfo.UserType = newUserInfo.UserType;
        
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

    [HttpPut("addUser")]
    [Authorize(Roles = "Admin,User")]
    public async Task<ActionResult> AddUserAsync([FromBody] AddUserDTO newUserInfo)
    {
        var newUser = new User();

        // Generate a new password for the user
        var pwd = new Password().IncludeLowercase().IncludeUppercase().IncludeSpecial().LengthRequired(20);
        var password = pwd.Next();
        // Setting user variables with init data to be amended later
        newUser.PasswordHash = newUser.HashPassword(password);
        newUser.LastLoginDate = DateTime.UtcNow;
        newUser.CurrentLoginDate = DateTime.UtcNow;

        // Setting variable reieved from call
        newUser.FullName = newUserInfo.FullName;
        newUser.Email = newUserInfo.Email;
        newUser.PicUrl = newUserInfo.PicUrl;
        newUser.UserType = newUserInfo.UserType;

        if (!ModelState.IsValid) 
        {
            return BadRequest("Incomplete Information");
        }

        try
        {
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // returns 409 on error
            return Conflict();
        }
        catch (DbUpdateException)
        {
            return Conflict();
        }

        await _emailService.SendUserLoginInfo(newUser.Email, password);

        return Ok(newUser.Id);
    }

    [HttpDelete("removeUser")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RemoveUser([FromQuery] int userId)
    {
         var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return BadRequest();
        }

        try
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            await _emailService.RemoveUserEmail(user.Email);
        } catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }
        
        return NoContent();
    }

    [HttpPatch("blockUser")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> BlockUser([FromQuery] int userId)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return BadRequest();
        }

        try
        {
            user.Block = true;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }

        return NoContent();
    }

    [HttpPatch("unBlockUser")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UnBlockUser([FromQuery] int userId)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return BadRequest();
        }

        try
        {
            user.Block = false;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }

        return NoContent();
    }


    [HttpPut("updatePicUrl")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdatePicURL([FromBody] UserPicURLUpdate userDTO)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userDTO.Id);

        if (user == null)
        {
            return BadRequest();
        }

        try
        {
            user.PicUrl = userDTO.PicUrl;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }

        return NoContent();
    }
}
