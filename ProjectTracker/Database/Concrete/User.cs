using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Database.Concrete
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
