using Shoptohome.Domain;

namespace Shoptohome.Service.Interface
{
   public interface IJWTService
   {
      string CreateToken(User user, string _key);
   }
}