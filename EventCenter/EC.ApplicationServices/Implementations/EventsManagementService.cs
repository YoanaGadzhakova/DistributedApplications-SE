using EC.ApplicationServices.Interfaces;
using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ApplicationServices.Implementations
{
    public class EventsManagementService : BaseManagementService, IEventsManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventsManagementService(ILogger<EventsManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateEventResponse> CreateEvent(CreateEventRequest request)
        {
            //Validations
            _unitOfWork.Events.Insert(new()
            {
                Id = request.Event.Id,
                Price = request.Event.Price,
                Title = request.Event.Title,
                Description = request.Event.Description,
                Start = request.Event.Start,
                End = request.Event.End,
                Capacity = request.Event.Capacity,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                IsActivated = true,
            });
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteEventResponse> DeleteEvent(DeleteEventRequest request)
        {
            var tempEvent = await _unitOfWork.Events.GetByIdAsync(request.Id);

            if (tempEvent == null)
            {
                _logger.LogError($"Event with identifier {request.Id} not found");
                throw new Exception("");
            }

            _unitOfWork.Events.Delete(tempEvent);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetEventResponse> GetEvent(GetEventRequest request)
        {
            GetEventResponse response = new() { Events = new() };

            var events = await _unitOfWork.Events.GetAllAsync(request.IsActive);

            foreach (var tempEvent in events)
            {
                response.Events.Add(new()
                {
                    Id = tempEvent.Id,
                    Price = tempEvent.Price,
                    Title = tempEvent.Title,
                    Description = tempEvent.Description,
                    Start = tempEvent.Start,
                    End = tempEvent.End,
                    Capacity = tempEvent.Capacity
                });
            }

            return response;
        }

        public async Task<EventViewModel> GetEventById(int id)
        {
            var tempEvent = await _unitOfWork.Events.GetByIdAsync(id);
            if (tempEvent == null)
            {
                throw new Exception("");
            }
            EventViewModel result = new EventViewModel()
            {
                Id = id,
                Price = tempEvent.Price,
                Title = tempEvent.Title,
                Description = tempEvent.Description,
                Start = tempEvent.Start,    
                End = tempEvent.End,
                Capacity= tempEvent.Capacity

            };
            return result;
        }

        public async Task<GetEventResponse> SearchEventsByTitle(string title)
        {
            GetEventResponse response = new GetEventResponse() { Events = new()};
            var events = await _unitOfWork.Events.GetAllAsync();
            var filteredEvents = events.Where(x => x.Title == title).ToList();
            List<EventViewModel> result = new List<EventViewModel>();
            foreach (var tempEvent in filteredEvents)
            {
                EventViewModel current = new EventViewModel()
                {
                    Id = tempEvent.Id,
                    Price = tempEvent.Price,
                    Title = tempEvent.Title,
                    Description = tempEvent.Description,
                    Start = tempEvent.Start,
                    End = tempEvent.End,
                    Capacity = tempEvent.Capacity

                };

                response.Events.Add(current);
            }


            return response;
        }

        public async Task<UpdateEventResponse> UpdateEvent(UpdateEventRequest request)
        {
            var tempEvent = await _unitOfWork.Events.GetByIdAsync(request.CurrentEventId);

            if (tempEvent == null)
            {
                _logger.LogError($"Event with identifier {request.CurrentEventId} not found");
                throw new ArgumentNullException("");
            }            

            tempEvent.Id = request.CurrentEventId;
            tempEvent.Price = request.Event.Price;
            tempEvent.Title = request.Event.Title;
            tempEvent.Description = request.Event.Description;
            tempEvent.Start = request.Event.Start;
            tempEvent.End = request.Event.End;
            tempEvent.Capacity = request.Event.Capacity;

            _unitOfWork.Events.Update(tempEvent);

            await _unitOfWork.SaveChangesAsync();

            return new();
        }
    }
}
