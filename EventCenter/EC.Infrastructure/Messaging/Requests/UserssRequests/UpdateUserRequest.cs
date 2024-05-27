using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Requests.UserssRequests
{
    public class UpdateUserRequest:ServiceRequestBase
    { 
        public int CurrentUserId { get; set; }
        public UserModel User { get; set; }
        public UpdateUserRequest()
        {
            
        }
        public UpdateUserRequest(int currentUserId, UserModel user)
        {
            CurrentUserId = currentUserId;
            User = user;
        }
    }
}
