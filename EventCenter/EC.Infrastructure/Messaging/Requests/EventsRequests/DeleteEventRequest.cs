using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.EventsRequests
{
    public class DeleteEventRequest:IntegerServiceRequestBase
    {
        public DeleteEventRequest(int id) : base(id) { }

    }
}
