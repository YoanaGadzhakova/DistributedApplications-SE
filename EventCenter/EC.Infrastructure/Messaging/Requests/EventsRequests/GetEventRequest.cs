using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.EventsRequests
{
    public class GetEventRequest:ServiceRequestBase
    {
        public bool IsActive { get; set; }
        public GetEventRequest(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
