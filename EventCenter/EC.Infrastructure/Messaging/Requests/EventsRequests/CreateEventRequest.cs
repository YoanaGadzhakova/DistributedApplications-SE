using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.EventsRequests
{
    public class CreateEventRequest:ServiceRequestBase
    {
        public EventModel Event { get; set; }
        public CreateEventRequest(EventModel eventModel)
        {
            Event = eventModel;
        }
    }
}
