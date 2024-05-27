using EC.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests
{
    public class UserModel
    {
        public int Id { get; set; }
        [MaxLength(20, ErrorMessage = "Maximum length of '20'")]
        required public string FirstName { get; set; }

        [MaxLength(20)]
        required public string LastName { get; set; }
        [Required]
        [MaxLength(35, ErrorMessage = "Maximum length of '35'")]
        public string Email { get; set; }

        [MaxLength(15, ErrorMessage = "Maximum length of '15'")]
        public required string Username { get; set; }
        [MaxLength(20, ErrorMessage = "Minimum length of '8'")]
        [MinLength(8,ErrorMessage="Minimum length of '8'")]
        public required string Password { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
