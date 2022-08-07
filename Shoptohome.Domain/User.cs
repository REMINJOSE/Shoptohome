using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shoptohome.Domain
{
   public class User:Hash
   {
      [JsonIgnore]
      [Key]
      public int UserID { get; set; }
      public string UserName { get; set; } = string.Empty;     
      public int RoleID { get; set; }
      public int StatusID { get; set; }
      public int CreatedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public int ModifiedBy { get; set; }
      public DateTime ModifiedDate { get; set; }
   }
}