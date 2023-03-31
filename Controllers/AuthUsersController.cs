using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AuthenticationService.Services;
using AuthenticationService.ExceptionHandling;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUsersController : ControllerBase
    {
        private readonly UserAuthServiceContext _context;
        private readonly ITokenBuilderUser _tokenBuilder;
        private readonly ResponseExceptionHandler _responseExceptionHandler = new();

        public AuthUsersController(UserAuthServiceContext context, ITokenBuilderUser tokenBuilder)
        {
            _context = context;
            _tokenBuilder = tokenBuilder;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthUser user)
        {
            //SingleOrDefaultAsync method is used to retrieve a single AuthUser object from the database
            //that matches the provided Username
            var dbUser = await _context.AuthUser.SingleOrDefaultAsync(u => u.Username == user.Username);
            

            if (dbUser == null)
            {
                //return NotFound("User not found!");
                return BadRequest(_responseExceptionHandler.UserNotFound());
            }

            var clientId = dbUser.Id.ToString();

            var isValid = dbUser.Password == user.Password;
            if (!isValid)
            {
                return BadRequest(_responseExceptionHandler.WrongPassword());
            }

            var token = _tokenBuilder.BuildToken(user.Username);
            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>
            {
                { "token", token },
                { "Id",clientId}
            };

            return Ok(tokenDictionary);
            //return Ok(token);
        }

      

        //post registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthUser registration)
        {
            var dbUser = await _context.AuthUser.SingleOrDefaultAsync(u => u.Username == registration.Username);

            if (dbUser != null)
            {
                return BadRequest(_responseExceptionHandler.UserExist());
            }

            var newUser = new AuthUser
            {
                FirstName= registration.FirstName,
                LastName= registration.LastName,
                Email= registration.Email,
                Username = registration.Username,
                Password = registration.Password
            };

            _context.AuthUser.Add(newUser);
            await _context.SaveChangesAsync();

            var token = _tokenBuilder.BuildToken(registration.Username);

            

            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>
            {
                { "token", token },
                
            };
            return Ok(tokenDictionary);
            //return Ok(token);
        }




        private bool AuthUserExists(int id)
        {
            return (_context.AuthUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}



//// GET: api/AuthUsers
//[HttpGet]
//public async Task<ActionResult<IEnumerable<AuthUser>>> GetAuthUser()
//{
//  if (_context.AuthUser == null)
//  {
//      return NotFound();
//  }
//    return await _context.AuthUser.ToListAsync();
//}

//// GET: api/AuthUsers/5
//[HttpGet("{id}")]
//public async Task<ActionResult<AuthUser>> GetAuthUser(int id)
//{
//  if (_context.AuthUser == null)
//  {
//      return NotFound();
//  }
//    var authUser = await _context.AuthUser.FindAsync(id);

//    if (authUser == null)
//    {
//        return NotFound();
//    }

//    return authUser;
//}

//// PUT: api/AuthUsers/5
//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//[HttpPut("{id}")]
//public async Task<IActionResult> PutAuthUser(int id, AuthUser authUser)
//{
//    if (id != authUser.Id)
//    {
//        return BadRequest();
//    }

//    _context.Entry(authUser).State = EntityState.Modified;

//    try
//    {
//        await _context.SaveChangesAsync();
//    }
//    catch (DbUpdateConcurrencyException)
//    {
//        if (!AuthUserExists(id))
//        {
//            return NotFound();
//        }
//        else
//        {
//            throw;
//        }
//    }

//    return NoContent();
//}

//// POST: api/AuthUsers
//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//[HttpPost]
//public async Task<ActionResult<AuthUser>> PostAuthUser(AuthUser authUser)
//{
//  if (_context.AuthUser == null)
//  {
//      return Problem("Entity set 'UserAuthServiceContext.AuthUser'  is null.");
//  }
//    _context.AuthUser.Add(authUser);
//    await _context.SaveChangesAsync();

//    return CreatedAtAction("GetAuthUser", new { id = authUser.Id }, authUser);
//}

//// DELETE: api/AuthUsers/5
//[HttpDelete("{id}")]
//public async Task<IActionResult> DeleteAuthUser(int id)
//{
//    if (_context.AuthUser == null)
//    {
//        return NotFound();
//    }
//    var authUser = await _context.AuthUser.FindAsync(id);
//    if (authUser == null)
//    {
//        return NotFound();
//    }

//    _context.AuthUser.Remove(authUser);
//    await _context.SaveChangesAsync();

//    return NoContent();
//}


//[HttpGet("verify")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public async Task<IActionResult> VerifyToken()
//{
//    var username = User.Claims.SingleOrDefault();
//    if (username == null)
//    {
//        return Unauthorized();
//    }

//    var userExits = await _context.AuthUser
//        .AnyAsync(u => u.Username == username.Value);

//    if (!userExits)
//    {
//        return Unauthorized();
//    }

//    return NoContent();
//}
