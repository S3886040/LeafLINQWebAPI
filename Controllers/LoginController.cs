using LeafLINQWebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Data;
using LeafLINQWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using LeafLINQWebAPI.Services;

namespace LeafLINQWebAPI.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private IConfiguration _config;
    private readonly LeafLINQContext _context;
    private readonly TokenService _tokenService;
    public LoginController(IConfiguration config, LeafLINQContext context, TokenService tokenService)
    {
        _config = config;
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] LoginDTO login)
    {
        // Define user logging in
        var user = _context.User.FirstOrDefault(x => x.Email == login.Email);
        if (user != null && user.Block != true && user.VerifyPassword(login.Password))
        {

            // Define jwt claims variables
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, login.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserType == 'A' || user.UserType == 'S' ? "Admin" : "User")
            };

            // Get the token
            var secToken = _tokenService.CreateToken(claims.ToList());
            var token = new JwtSecurityTokenHandler().WriteToken(secToken);

            // Generate a refresh token and session id
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(240);
            var sessionId = Guid.NewGuid().ToString();

            // Define the response object
            var response = new LoginResponse();
            response.Id = user.Id;
            response.UserType = user.UserType;
            response.AccessToken = token;
            response.RefreshToken = refreshToken;
            response.SessionID = sessionId;

            // save user token and expiration
            var session = new Session();
            session.SessionId = sessionId;
            session.RefreshToken = refreshToken;
            session.RefreshTokenExpiration = refreshTokenExpiry;
            session.UserId = user.Id;
            _context.Add(session);

            // manage login timestamps, current log in now becomes last login
            user.LastLoginDate = user.CurrentLoginDate;
            user.CurrentLoginDate = DateTime.UtcNow;
            _context.SaveChanges();

            // Return 200 ok with response object
            return Ok(response);
        }
        else
        {
            return BadRequest();
        }

    }

}