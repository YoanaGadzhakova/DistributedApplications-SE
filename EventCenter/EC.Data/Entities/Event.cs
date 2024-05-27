using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Data.Entities
{
    public class Event:BaseEntity
    {
        public Event()
        {
            Tickets = new HashSet<Ticket>();
        }

        [MaxLength(100)]
        required public string Title { get; set; }

        [MaxLength(1000)]
        required public string Description { get; set; }
        [Required]  
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public decimal Price { get; set; }
    }
}
