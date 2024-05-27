using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.EventsRequests
{
    public class UpdateEventRequest:ServiceRequestBase
    {
        public int CurrentEventId { get; set; }
        public EventModel Event { get; set; }

        public UpdateEventRequest()
        {
            
        }
        public UpdateEventRequest(int currentEvenetId, EventModel newEvent)
        {
            CurrentEventId = currentEvenetId;
            Event = newEvent;
        }
    }
}
