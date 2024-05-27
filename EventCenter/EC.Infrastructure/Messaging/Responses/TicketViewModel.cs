using EC.Data.Entities;
using EC.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Responses
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        required public TicketType Type { get; set; }
        public string? Note { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
