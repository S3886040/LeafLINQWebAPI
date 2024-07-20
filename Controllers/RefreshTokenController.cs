using LeafLINQWebAPI.DTOs;
using LeafLINQWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Data;

namespace LeafLINQWebAPI.Controllers;

[ApiController]
[Route("api/")]
public class RefreshTokenController : ControllerBase
{
    private IConfiguration _config;
    private readonly LeafLINQContext _context;
    private readonly TokenService _tokenService;
    public RefreshTokenController(IConfiguration config, LeafLINQContext context, TokenService tokenService)
    {
        _config = config;
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("refreshToken")]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
    {
        // Determine if the token requires refreshing
        if (_tokenService.VerifyJwtToken(tokenDTO.AccessToken))
        {
            return NoContent();
        }

        string accessToken = tokenDTO.AccessToken;
        string refreshToken = tokenDTO.RefreshToken;
        string sessionID = tokenDTO.SessionId;

        // Get the principal from expired token
        var principal = new ClaimsPrincipal();
        try
        {
            principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        } catch (SecurityTokenException ex)
        {
            return BadRequest("Invalid access token!!"); 
        }
        
        if (principal == null)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        // Get the user information from the token claims
        var nameIdentifierClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            return BadRequest("NameIdentifier claim not found.");
        }

        // Parse the user ID and Get the user object
        var userIdString = nameIdentifierClaim.Value;
        var userId = int.Parse(userIdString);
        var user = _context.User.FirstOrDefault(x => x.Id == userId);

        // get session
        var session = _context.Session.Where(x => x.SessionId ==  sessionID).FirstOrDefault();

        // Check the users refresh token validity and expiry date
        if (user == null || session == null || !session.RefreshToken.Equals(refreshToken) || session.RefreshTokenExpiration <= DateTime.UtcNow || session.UserId != user.Id)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        // create new tokens
        var secToken = _tokenService.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Save your changes to the db
        session.RefreshToken = newRefreshToken;
        await _context.SaveChangesAsync();

        // return the new tokens to the caller
        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(secToken),
            refreshToken = newRefreshToken
        });


    }

}
