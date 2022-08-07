using Shoptohome.Domain;

namespace Shoptohome.Service.Interface
{
   public interface IHashPassword
   {
      Hash CreatePasswordHash(string password);
      bool VerifyPasswordHash(Hash userHash);
   }
}