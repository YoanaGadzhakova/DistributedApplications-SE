using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests
{
    public class EventModel
    {
        public int Id { get; set; }
        [MaxLength(100,ErrorMessage ="Maximum leght if '100'")]
        required public string Title { get; set; }

        [MaxLength(1000,ErrorMessage ="Maximum leght if '100'")]
        required public string Description { get; set; }
        required public DateTime Start { get; set; }
        required public DateTime End { get; set; }
        required public int Capacity { get; set; }
        public decimal Price { get; set; }
    }
}
