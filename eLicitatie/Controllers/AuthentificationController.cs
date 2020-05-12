using Common;
using Common.Authentication;
using eLicitatie.Api.Entities;
using eLicitatie.Api.Extensions;
using eLicitatie.Api.Extensions.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eLicitatie.Api.Controllers
{
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly eLicitatieDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthentificationController(eLicitatieDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }

        [HttpPost("api/signin")]
        public async Task<IActionResult> Login([FromBody] SignInRequest signInRequest)
        {
            if (signInRequest == null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            User user = await _context.Users.SingleOrDefaultAsync(e => e.Email == signInRequest.Email);
            if (user == null) return NotFound();

            if (!signInRequest.Password.VerifyMd5Hash(user.PasswordHash)) return NotFound();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTOptions:Secret"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            IdentityModelEventSource.ShowPII = true;
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = user.MapToResponse();
            response.Token = tokenHandler.WriteToken(token);

            return Ok(response);
        }

        [HttpPost("api/signup")]
        public async Task<IActionResult> Register([FromBody]SignUpRequest signUpRequest)
        {
            if (signUpRequest == null) return UnprocessableEntity(Constants.ErrorMessages.UnprocessableEntity);

            if (!RegisterFieldsAreValid(signUpRequest))
                return BadRequest(Constants.ErrorMessages.EmptyFields);

            if (await _context.Users.AnyAsync(user => user.Email == signUpRequest.Email)) return BadRequest("The given e-mail is associated with an account.");

            User user = signUpRequest.MapToEntity();

            _context.Users.Add(user);

            try { await _context.SaveChangesAsync(); }
            catch (Exception) { return this.InternalServerError(); }

            return Ok(user);
        }

        private bool RegisterFieldsAreValid(SignUpRequest signUp)
        {
            if (string.IsNullOrEmpty(signUp.FirstName) ||
                string.IsNullOrEmpty(signUp.LastName) ||
                string.IsNullOrEmpty(signUp.Email) ||
                string.IsNullOrEmpty(signUp.Password))
                return false;

            return true;
        }

    }
}