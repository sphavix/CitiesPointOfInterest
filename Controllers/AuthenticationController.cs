using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CityPointOfInterest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityPointOfInterest.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        
        private IConfiguration _configuration { get; }
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }

        

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequest request)
        {
            //verify username and password
            var user = ValidateUserCredentials(request.Username, request.Password);

            if(user is null)
            {
                return Unauthorized();
            }

            //create a token
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(
                            _configuration["Authentication:Secretkey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create claims
            var claims = new List<Claim>();
            claims.Add(new Claim("sub", user.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claims.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    signingCredentials);
            
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);

        }

        private User ValidateUserCredentials(string username, string password)
        {
            return new User(
                1,
                username ?? "",
                "Spha",
                "Zolakhe",
                "Johannesburg"
            );
        }

        public class AuthenticationRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}