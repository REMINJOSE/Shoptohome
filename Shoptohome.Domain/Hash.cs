using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shoptohome.Domain
{
   public class Hash
   {
      [JsonIgnore]
      public byte[] Passwordhash { get; set; } = Array.Empty<byte>();
      [JsonIgnore]
      public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
      [NotMapped]
      public string Password { get; set; } = string.Empty;
   }
}
