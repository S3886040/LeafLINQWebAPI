using LeafLINQWebAPI.DTOs;
using LeafLINQWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Data;

namespace LeafLINQWebAPI.Controllers;

[ApiController]
[Route("recovery/")]
public class RecoveryController : ControllerBase
{

    private IConfiguration _config;
    private readonly LeafLINQContext _context;
    public RecoveryController(IConfiguration config, LeafLINQContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult ServerLogin([FromBody] ServerLoginDTO login)
    {
        if (login.UserName == _config["ManualServerLogin:UserName"] && login.Password == _config["ManualServerLogin:Password"])
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create Jwt token
            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(5),
              signingCredentials: credentials);

            // Get the token
            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            // Return 200 ok with response object
            return Ok(token);
        }
        else
        {
            return BadRequest("Wrong UserName or Password!");
        }

    }

    // Need to verify that we have a user setup for this email address 
    // before sending a verifcation code and allowing them to be setup as
    // a valid user. Must exist first, access is not available to the general public.
    // Needs to be here so we dodge the Authoristaion process [Authorize]
    [HttpGet("GetUserUsingEmail")]
    [Authorize]
    public async Task<ActionResult<TempUser>> GetUserUsingEmail(string email)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return BadRequest();
        }
        return Ok(user);
    }

    // See above function comments for GetUser()
    [HttpGet("GetUserUsingUserID")]
    [Authorize]
    public async Task<ActionResult<User>> GetUserUsingUserID(int userID)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userID);

        if (user == null)
        {
            return BadRequest();
        }
        return Ok(user);
    }

    // See above function comments for GetUser()
    [HttpGet("GetTempUser")]
    [Authorize]
    public async Task<ActionResult<TempUser>> GetTempUser(string email)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);
        var tempUser = await _context.TempUser.FirstOrDefaultAsync(x => x.Id == user.Id);

        if (user != null && tempUser != null)
        {
            return Ok(tempUser);
        }
        else
        {
            return BadRequest();
        }

    }

    // See above function comments for GetUser()
    [HttpPut("WriteTempUser")]
    [Authorize]
    public async Task<ActionResult> WriteTempUser(TempUserModel tempUserModel)
    {
        bool newUser = false;

        if (ModelState.IsValid)
        {
            try
            {
                TempUser tempUser = _context.TempUser.FirstOrDefault(x => x.Id == tempUserModel.Id);

                if (tempUser == null)
                {
                    newUser = true;
                    tempUser = new TempUser();
                }

                tempUser.ConfirmationCode = tempUserModel.ConfirmationCode;
                tempUser.EncryptedCode = tempUserModel.EncryptedCode;

                // Assign new user.
                if (newUser)
                {
                    tempUser.Id = tempUserModel.Id;
                    _context.Add(tempUser);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts if necessary
                return Conflict(); // Return 409 Conflict if concurrency conflict occurs
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
        return NoContent(); ;
    }

    [HttpPut("UpdatePassword")]
    [Authorize]
    public async Task<ActionResult> UpdatePassword(UserModel userModel)
    {

        if (ModelState.IsValid)
        {
            try
            {
                User user = _context.User.FirstOrDefault(x => x.Id == userModel.Id);

                if (user == null)
                {
                    return NoContent();
                }
                // Encrypt new password and write to User record.
                user.PasswordHash = user.HashPassword(userModel.NewPassword);
                // Make sure the User file is updated.
                _context.Update(user);
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts if necessary
                return Conflict(); // Return 409 Conflict if concurrency conflict occurs
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
        return NoContent(); ;
    }
}
