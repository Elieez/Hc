using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCareABApi.Models
{
	public class User
    {
        public int Id { get; set; }
		[Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
		// List of roles, a User can have one or more roles if needed.
		// Not specifying a role during User creation sets it to User by default
		[Column(TypeName = "jsonb")]
		public List<string> Roles { get; set; } = new List<string>();
	}

}
