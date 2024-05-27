using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.TicketsRequests
{
    public class GetTicketRequest:ServiceRequestBase
    {
        public bool IsActive { get; set; }
        public GetTicketRequest(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
