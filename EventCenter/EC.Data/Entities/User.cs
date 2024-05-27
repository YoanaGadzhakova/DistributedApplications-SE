using EC.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Tickets = new HashSet<Ticket>();
        }

        [MaxLength(20)]
        required public string FirstName { get; set; }

        [MaxLength(20)]
        required public string LastName { get; set; }
        [Required]
        [MaxLength(35)]
        public string Email { get; set; }

        [MaxLength(15)]
        public required string Username { get; set; }
        [MaxLength(20)]
        [MinLength(8)]
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
