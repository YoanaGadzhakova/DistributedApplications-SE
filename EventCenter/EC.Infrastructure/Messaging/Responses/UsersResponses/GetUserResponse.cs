using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging.Responses.UsersResponses
{
    public class GetUserResponse : ServiceResponseBase
    {
        public List<UserViewModel>? Users { get; set; }
    }
}
