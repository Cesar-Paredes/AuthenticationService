//using Microsoft.IdentityModel.JsonWebTokens
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Drawing2D;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationService.Services
{
    public class TokenBuilderAdmin:ITokenBuilderAdmin
    {
        public string BuildToken(string username)
        {
            var signkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("admin-cgicanadatelecomkey")); //this sign key will change everytime user sign in
            var signCredentials = new SigningCredentials(signkey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
            };

            // Set token expiration to 5 hours from now
            var tokenExpiry = DateTime.UtcNow.AddHours(5); 

            var jwt = new JwtSecurityToken(
                claims: claims, 
                signingCredentials: signCredentials,
                expires: tokenExpiry
                );
            

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //store the token either in storage or cookie, then in the filter we retrive it
            //HttpContext.

            return encodedJwt; //usually stored in session storage on browser for front end
        }
    }
}


//until user logout is going to use the same key
//
//when logout jwt token should get deleted
//
//every time the user sign in we need to send the sign key to them, that we will do it later.
//
//
