using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Requests.TicketsRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.TicketsResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ApplicationServices.Interfaces
{
    public interface ITicketsManagementService
    {
        Task<CreateTicketResponse> CreateTicket(CreateTicketRequest request);
        Task<DeleteTicketResponse> DeleteTicket(DeleteTicketRequest request);
        Task<GetTicketResponse> GetTicket(GetTicketRequest request);
        Task<TicketViewModel> GetTicketById(int id);
        Task<GetTicketResponse> SearchTicketsByEventId(int id);
        Task<UpdateTicketResponse> UpdateTicket(UpdateTicketRequest request);
    }
}
