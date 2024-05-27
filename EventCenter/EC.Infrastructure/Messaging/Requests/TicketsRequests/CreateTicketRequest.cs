using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.TicketsRequests
{
    public class CreateTicketRequest:ServiceRequestBase
    {
        public TicketModel Ticket { get; set; }
        public CreateTicketRequest(TicketModel ticket)
        {
            Ticket = ticket;
        }
    }
}
