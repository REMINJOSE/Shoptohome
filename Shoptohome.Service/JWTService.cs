using Microsoft.IdentityModel.Tokens;
using Shoptohome.Domain;
using Shoptohome.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shoptohome.Service
{
   public class JWTService : IJWTService
   {
      public string CreateToken(User user, string _key)
      {
         List<Claim> claims = new() {
            new Claim(ClaimTypes.Name,user.UserName ),
            new Claim(ClaimTypes.Role,user.RoleID.ToString() ),
         };
         var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_key));
         var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

         var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: cred
            );
         var jwt = new JwtSecurityTokenHandler().WriteToken(token);
         return jwt;
      }
   }
}
