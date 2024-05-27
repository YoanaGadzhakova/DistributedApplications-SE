using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.UserssRequests
{
    public class DeleteUserRequest : IntegerServiceRequestBase
    {
        public DeleteUserRequest(int id) : base(id) { }
    
    }
}
