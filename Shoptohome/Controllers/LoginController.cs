using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoptohome.Data;
using Shoptohome.Domain;
using Shoptohome.Service.Interface;

namespace Shoptohome.Controllers
{
   [ApiController]
   public class LoginController : ControllerBase
   {
      public IConfiguration _configuration;
      private readonly DataContext _dataContext;
      private readonly IHashPassword _hashPassword;
      private readonly IJWTService _jWTService;

      public LoginController(IConfiguration configuration, DataContext dataContext, IHashPassword hashPassword, IJWTService jWTService)
      {
         _configuration = configuration;
         _dataContext = dataContext;
         _hashPassword = hashPassword;
         _jWTService = jWTService;
      }


      [HttpPost]
      [Route("api/login/loginuser")]
      public async Task<IActionResult> LoginUserAsync(User user)
      {
         var users = _dataContext.Users.AsQueryable();
         var dbUser = await users
                        .Where(d => d.UserName == user.UserName).ToListAsync()
                        .ConfigureAwait(false);
         dbUser ??= new List<User>();
         if (!dbUser.Any())
         {
            return BadRequest("User not found.");
         }
         var validHash = _hashPassword.VerifyPasswordHash(new Hash { Password = user.Password, PasswordSalt = dbUser.First().PasswordSalt, Passwordhash = dbUser.First().Passwordhash });
         if (!validHash)
         {
            return BadRequest("Wrong password.");
         }
         var token = _jWTService.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value);
         return Ok(token);
      }

      [HttpPost]
      [Route("api/login/registeruser")]
      public async Task<IActionResult> RegisterUserAsync(User user)
      {
         var hashedPassword = _hashPassword.CreatePasswordHash(user.Password);
         user.Passwordhash = hashedPassword.Passwordhash;
         user.PasswordSalt = hashedPassword.PasswordSalt;
         await _dataContext.Users.AddAsync(user).ConfigureAwait(false);
         await _dataContext.SaveChangesAsync()
                     .ConfigureAwait(false);
         return Ok(user);
      }

      [HttpPost]
      [Route("api/login/updateuser")]
      public async Task<IActionResult> UpdateUserAsync(User user)
      {
         var users = _dataContext.Users.AsQueryable();
         var dbusers = await users
                        .Where(d => d.UserName == user.UserName).ToListAsync()
                        .ConfigureAwait(false);
         if (dbusers == null)
         {
            return BadRequest("User not found");
         }
         dbusers.First().UserName = user.UserName;
         dbusers.First().Password = user.Password;
         dbusers.First().RoleID = user.RoleID;
         dbusers.First().StatusID = user.StatusID;
         dbusers.First().ModifiedBy = user.UserID;
         dbusers.First().ModifiedDate = DateTime.Now;
         await _dataContext.SaveChangesAsync()
                     .ConfigureAwait(false);
         return Ok(user);
      }

      [HttpDelete]
      [Route("api/login/deleteuser")]
      public async Task<IActionResult> DeleteUserAsync(User user)
      {
         var users = _dataContext.Users.AsQueryable();
         var dbusers = await users
                              .Where(d => d.UserName == user.UserName).ToListAsync()
                              .ConfigureAwait(false);
         if (dbusers == null)
         {
            return BadRequest("User not found");
         }
         _dataContext.Users.Remove(dbusers.First());
         await _dataContext.SaveChangesAsync()
                     .ConfigureAwait(false);
         return Ok(true);
      }
   }
}
