using EC.ApplicationServices.Interfaces;
using EC.Data.Entities;
using EC.Infrastructure.Messaging.Requests.TicketsRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.TicketsResponses;
using EC.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ApplicationServices.Implementations
{
    public class TicketsManagementService : BaseManagementService, ITicketsManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketsManagementService(ILogger<EventsManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateTicketResponse> CreateTicket(CreateTicketRequest request)
        {
            User user = await _unitOfWork.Users.GetByIdAsync(request.Ticket.UserId);
            Event ticketEvent = await _unitOfWork.Events.GetByIdAsync(request.Ticket.EventId);
            decimal price = ticketEvent.Price;
            if (request.Ticket.Type == Data.Enums.TicketType.VIP)
            {
                price *= (decimal)2;
            }
            //Validations
            _unitOfWork.Tickets.Insert(new()
            {
                Id = request.Ticket.Id,
                Price = price,
                Type = request.Ticket.Type,
                Note = request.Ticket.Note,
                UserId = request.Ticket.UserId,
                EventId = request.Ticket.EventId,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                IsActivated = true,
                
            });



            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteTicketResponse> DeleteTicket(DeleteTicketRequest request)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.Id);

            if (ticket == null)
            {
                _logger.LogError($"Ticket with identifier {request.Id} not found");
                throw new Exception("");
            }

            _unitOfWork.Tickets.Delete(ticket);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetTicketResponse> GetTicket(GetTicketRequest request)
        {
            GetTicketResponse response = new() { Tickets = new() };

            var tickets = await _unitOfWork.Tickets.GetAllAsync(request.IsActive);

            foreach (var ticket in tickets)
            {
                response.Tickets.Add(new()
                {
                    Id = ticket.Id,
                   Price = ticket.Price,
                   Type = ticket.Type,
                   Note = ticket.Note,
                   UserId = ticket.UserId,
                   EventId = ticket.EventId,
                });
            }

            return response;
        }

        public async Task<TicketViewModel> GetTicketById(int id)
        {
            var user = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("");
            }

            TicketViewModel result = new TicketViewModel()
            {
                Id = id,
                Price = user.Price,
                Type = user.Type,
                Note = user.Note,
                UserId = user.UserId,
                EventId = user.EventId
            };
            return result;
        }

        public async Task<GetTicketResponse> SearchTicketsByEventId(int id)
        {
            GetTicketResponse response = new GetTicketResponse() { Tickets = new()};
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            var filteredTickets = tickets.Where(x => x.EventId == id).ToList();
            List<TicketViewModel> result = new List<TicketViewModel>();
            foreach (var user in filteredTickets)
            {
                TicketViewModel current = new TicketViewModel()
                {
                    Id = user.Id,
                    Price = user.Price,
                    Type = user.Type,
                    Note = user.Note,
                    UserId = user.UserId,
                    EventId = user.EventId
                };

                response.Tickets.Add(current);
            }


            return response;
        }

        public async Task<UpdateTicketResponse> UpdateTicket(UpdateTicketRequest request)
        {
            if (_unitOfWork.Users.GetByIdAsync(request.CurrentTicketId) == null)
            {
                _logger.LogError($"Ticket with identifier {request.CurrentTicketId} not found");
                throw new Exception("");
            }

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.CurrentTicketId);

            ticket.Id = request.CurrentTicketId;
            ticket.Type = request.Ticket.Type;
            ticket.Note = request.Ticket.Note;
            ticket.UserId = request.Ticket.UserId;
            ticket.EventId = request.Ticket.EventId;
            _unitOfWork.Tickets.Update(ticket);



            await _unitOfWork.SaveChangesAsync();

            return new();
        }
    }
}
