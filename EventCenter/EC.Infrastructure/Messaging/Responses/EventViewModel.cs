using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Responses
{
    public class EventViewModel
    {
        public int Id { get; set; }
        required public string Title { get; set; }
        required public string Description { get; set; }
        required public DateTime Start { get; set; }
        required public DateTime End { get; set; }
        required public int Capacity { get; set; }
        public decimal Price { get; set; }
    }
}
