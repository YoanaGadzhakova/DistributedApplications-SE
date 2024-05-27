using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.TicketsRequests
{
    public class UpdateTicketRequest:ServiceRequestBase
    {
        public TicketModel Ticket { get; set; }
        public int CurrentTicketId { get; set; }
        public UpdateTicketRequest()
        {
            
        }
        public UpdateTicketRequest(int currentTicketId, TicketModel ticket)
        {
            CurrentTicketId = currentTicketId;
            Ticket = ticket;
        }
    }
}
