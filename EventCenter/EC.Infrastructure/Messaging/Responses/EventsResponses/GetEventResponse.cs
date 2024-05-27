using EC.Infrastructure.Messaging.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Responses.EventsResponses
{
    public class GetEventResponse:ServiceResponseBase
    {
        public List<EventViewModel>? Events { get; set; }
    }
}
