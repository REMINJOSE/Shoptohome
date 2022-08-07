using Shoptohome.Domain;
using Shoptohome.Service.Interface;
using System.Security.Cryptography;

namespace Shoptohome.Service
{
   public class HashPassword : IHashPassword
   {
      public Hash CreatePasswordHash(string password)
      {
         using var hmac = new HMACSHA512();
         var passwordSalt = hmac.Key;
         var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
         return new Hash { Passwordhash = passwordHash, PasswordSalt = passwordSalt };
      }
      public bool VerifyPasswordHash(Hash userHash) {
         using var hmac = new HMACSHA512(userHash.PasswordSalt);
         var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userHash.Password));
         return computedHash .SequenceEqual( userHash.Passwordhash);
      }
   }

}