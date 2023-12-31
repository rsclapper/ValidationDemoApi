﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ValidationDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost, Route("login")]
        public IActionResult Login(LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid request");
            }
            if (user.Username != "admin" || user.Password != "admin")
            {
                return Unauthorized();
            }
            // generate a JWT token
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "https://localhost:7124",
                audience: "https://localhost:7124",
                claims: new List<Claim>() { new Claim("UserId", "1")},
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
           
            return Ok(new { Token = tokenString});
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
