﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.TicketsRequests
{
    public class DeleteTicketRequest:IntegerServiceRequestBase
    {
        public DeleteTicketRequest(int id) : base(id) { }
    }
}
