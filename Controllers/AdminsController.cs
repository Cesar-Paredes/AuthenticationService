using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;
using AuthenticationService.Services;
using AuthenticationService.ExceptionHandling;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminsController : ControllerBase
    {
        private readonly AdminAuthServiceContext _context;
        private readonly ITokenBuilderAdmin _tokenBuilder;
        private readonly ResponseExceptionHandler _responseExceptionHandler = new();
        public AdminsController(AdminAuthServiceContext context, ITokenBuilderAdmin tokenBuilder)
        {
            _context = context;
            _tokenBuilder = tokenBuilder;
        }

        
        [HttpPost("admin_login")]    
        public async Task<IActionResult> Login([FromBody] Admin user)
        {

            Console.WriteLine(user.Username);
            Console.WriteLine("Admin controller login");
            var dbUser = await _context.Admin.SingleOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null)
            {
                return BadRequest(_responseExceptionHandler.UserNotFound());
                //return NotFound("User not found!");
            }

            var adminId = dbUser.Id.ToString();

            var isValid = dbUser.Password == user.Password;
            if (!isValid)
            {
                return BadRequest(_responseExceptionHandler.WrongPassword());
            }

            var token = _tokenBuilder.BuildToken(user.Username);
            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>
            {
                { "token", token },
                { "Id", adminId }
            };
           
            return Ok(tokenDictionary);
            
            //return Ok(token);
            // you no longer need to return the token from the Login method,
            // as it will be added to the request header by the AddTokenToHeader filter.
            //if we use a filter to put the token in the header response.
            //return Ok();
        }
        
        private bool AdminExists(int id)
        {
            return (_context.Admin?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}


//3 signing keys
//JwtParser parser = Jwts.parser().setSigningKey("**NEEDTOKENKEY**");Jwt jwt = parser.parse(token);