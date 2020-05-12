using eLicitatie.Api.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLicitatie.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public int Role { get; set; }

        public string PasswordHash { get; private set; }
        [NotMapped]
        public string Password { set => PasswordHash = value.GetMd5Hash(); }
    }
}