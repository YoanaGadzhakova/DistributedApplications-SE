using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.UserssRequests
{
    public class GetUserRequest : ServiceRequestBase
    {
        public bool IsActive { get; set; }
        public GetUserRequest(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
