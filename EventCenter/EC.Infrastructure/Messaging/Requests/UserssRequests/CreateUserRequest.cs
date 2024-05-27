using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.UserssRequests
{
    public class CreateUserRequest : ServiceRequestBase
    {
        public UserModel User { get; set; }
        public CreateUserRequest(UserModel user)
        {
            User = user;
        }
    }
}

