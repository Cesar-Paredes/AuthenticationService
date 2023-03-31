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
    public class CSRAgentsController : ControllerBase
    {
        private readonly CSRAgentAuthServiceContext _context;
        private readonly ITokenBuilderCSRAgent _tokenBuilder;
        private readonly ResponseExceptionHandler _responseExceptionHandler = new();

        public CSRAgentsController(CSRAgentAuthServiceContext context, ITokenBuilderCSRAgent tokenBuilder)
        {
            _context = context;
            _tokenBuilder = tokenBuilder;
        }

        //this method is returning a dictionary with 2 values, the token and the agentId
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CSRAgent user)
        {
            var dbUser = await _context.CSRAgent.SingleOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null)
            {
                //return NotFound("User not found!");
                return BadRequest(_responseExceptionHandler.UserNotFound());
            }

            var agentId = dbUser.Id.ToString();

            var isValid = dbUser.Password == user.Password;
            if (!isValid)
            {
                return BadRequest(_responseExceptionHandler.WrongPassword());
            }

            var token = _tokenBuilder.BuildToken(user.Username);
            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>
            {
                { "token", token },
                { "Id", agentId }
            };
            return Ok(tokenDictionary);
            //return Ok(token);
        }


        //only admin would have access to this URL, it will be located the admin UI.
        //post registration
        [HttpPost("register_agent")]
        public async Task<IActionResult> Register([FromBody] CSRAgent registration)
        {
            var dbUser = await _context.CSRAgent.SingleOrDefaultAsync(u => u.Username == registration.Username);

            if (dbUser != null)
            {
                return BadRequest(_responseExceptionHandler.UserExist());
            }

            var newUser = new CSRAgent
            {
                Firstname= registration.Firstname,
                Lastname= registration.Lastname,
                Username = registration.Username,
                Password = registration.Password
            };

            _context.CSRAgent.Add(newUser);
            await _context.SaveChangesAsync();

            var token = _tokenBuilder.BuildToken(registration.Username);

            

            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>
            {
                { "token", token },
                
            };
            return Ok(tokenDictionary);
            //return Ok(token);
        }


        private bool CSRAgentExists(int id)
        {
            return (_context.CSRAgent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}





//// GET: api/CSRAgents
//[HttpGet]
//public async Task<ActionResult<IEnumerable<CSRAgent>>> GetCSRAgent()
//{
//  if (_context.CSRAgent == null)
//  {
//      return NotFound();
//  }
//    return await _context.CSRAgent.ToListAsync();
//}

//// GET: api/CSRAgents/5
//[HttpGet("{id}")]
//public async Task<ActionResult<CSRAgent>> GetCSRAgent(int id)
//{
//  if (_context.CSRAgent == null)
//  {
//      return NotFound();
//  }
//    var cSRAgent = await _context.CSRAgent.FindAsync(id);

//    if (cSRAgent == null)
//    {
//        return NotFound();
//    }

//    return cSRAgent;
//}

//// PUT: api/CSRAgents/5
//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//[HttpPut("{id}")]
//public async Task<IActionResult> PutCSRAgent(int id, CSRAgent cSRAgent)
//{
//    if (id != cSRAgent.Id)
//    {
//        return BadRequest();
//    }

//    _context.Entry(cSRAgent).State = EntityState.Modified;

//    try
//    {
//        await _context.SaveChangesAsync();
//    }
//    catch (DbUpdateConcurrencyException)
//    {
//        if (!CSRAgentExists(id))
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

//// POST: api/CSRAgents
//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//[HttpPost]
//public async Task<ActionResult<CSRAgent>> PostCSRAgent(CSRAgent cSRAgent)
//{
//  if (_context.CSRAgent == null)
//  {
//      return Problem("Entity set 'CSRAgentAuthServiceContext.CSRAgent'  is null.");
//  }
//    _context.CSRAgent.Add(cSRAgent);
//    await _context.SaveChangesAsync();

//    return CreatedAtAction("GetCSRAgent", new { id = cSRAgent.Id }, cSRAgent);
//}

//// DELETE: api/CSRAgents/5
//[HttpDelete("{id}")]
//public async Task<IActionResult> DeleteCSRAgent(int id)
//{
//    if (_context.CSRAgent == null)
//    {
//        return NotFound();
//    }
//    var cSRAgent = await _context.CSRAgent.FindAsync(id);
//    if (cSRAgent == null)
//    {
//        return NotFound();
//    }

//    _context.CSRAgent.Remove(cSRAgent);
//    await _context.SaveChangesAsync();

//    return NoContent();
//}