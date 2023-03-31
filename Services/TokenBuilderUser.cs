using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Services
{
    public class TokenBuilderUser:ITokenBuilderUser
    {
        public string BuildToken(string username)
        {
            // creates a new SymmetricSecurityKey object using a byte array representation of
            // the secret key string "user-cgicanadatelecomkey".
            // This key is used to sign the JWT token and should be kept secret.

            //creates a simmetricSecurityKey object that uses the byte array that is generated with the secret key
            //this object will be used to sign and verify the authenticity of the JWT(Json Web Token) token.
            //This new SymmetricSecurityKey object is referred as the key.
            //This key is used in the tokenSignature, to then be used later in the code.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("user-cgicanadatelecomkey")); //this sign key will change everytime user sign in

            //this is a signature that will be used to sign a JWT token later in the code.
            //creates a new SigningCredentials object using the key  and a specified algorithm (HmacSha256).
            //This is used to sign the JWT token.
            //The SigningCredentials object (signCredentials) contains both the key and the algorithm used to sign the token,
            //and is used to sign the token when it is generated later on in the code.
            var tokenSignature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            //This creates a new array of Claim objects and assigns it to the variable claims.
            var claims = new Claim[]
            {
                //creates a new Claim object with the Sub key (represented by the JwtRegisteredClaimNames.Sub constant)
                //and the username value. This claim will be included in the JWT token
                new Claim(JwtRegisteredClaimNames.Sub, username),
            };

            // Set token expiration to 5 hours from now
            var tokenExpiry = DateTime.UtcNow.AddHours(5);

            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: tokenSignature,
                expires: tokenExpiry
                );


            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //store the token either in storage or cookie, then in the filter we retrive it
            //HttpContext.

            return encodedJwt; //usually stored in session storage on browser for front end
        }
    }
}


