using EC.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Data.Entities
{
    public class Ticket : BaseEntity
    {
        public decimal Price { get; set; }


        [Required]
        required public TicketType Type { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Event Event { get; set; }
        public int EventId { get; set; }
    }
}
