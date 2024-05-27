﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Responses.TicketsResponses
{
    public class GetTicketResponse : ServiceResponseBase
    {
        public List<TicketViewModel>? Tickets { get; set; }
    }
}
