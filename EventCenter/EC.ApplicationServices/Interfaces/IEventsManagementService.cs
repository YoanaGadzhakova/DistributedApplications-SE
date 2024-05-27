using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ApplicationServices.Interfaces
{
    public interface IEventsManagementService
    {
        Task<CreateEventResponse> CreateEvent(CreateEventRequest request);
        Task<DeleteEventResponse> DeleteEvent(DeleteEventRequest request);
        Task<GetEventResponse> GetEvent(GetEventRequest request);
        Task<EventViewModel> GetEventById(int id);
        Task<GetEventResponse> SearchEventsByTitle(string title);
        Task<UpdateEventResponse> UpdateEvent(UpdateEventRequest request);
    }
}
