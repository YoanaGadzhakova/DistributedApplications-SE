using EC.Infrastructure.Messaging.Requests.TicketsRequests;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.TicketsResponses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ApplicationServices.Interfaces
{
    public interface IUsersManagementService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request);
        Task<GetUserResponse> GetUser(GetUserRequest request);
        Task<UserViewModel> GetUserById(int id);
        Task<GetUserResponse> SearchUsersByFirstName(string firstName);
        Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);
    }
}
