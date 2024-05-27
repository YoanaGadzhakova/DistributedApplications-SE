using Azure.Core;
using EC.Data.Entities;
using EC.Infrastructure.Messaging.Requests;
using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Requests.TicketsRequests;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.TicketsResponses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using EC.Website.Filters;
using EC.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace EC.Website.Controllers
{
    public class TicketsController : Controller
    {


        private readonly Uri uri = new("https://localhost:7182/api/tickets");
        HttpClient client;
        private const int PageSize = 10;

        public TicketsController()
        {
            client = new HttpClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
        }
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            HttpResponseMessage response = client.GetAsync(uri + "/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetTicketResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                PagenatedList<TicketViewModel> paginatedList = await PagenatedList<TicketViewModel>.CreateAsync(responseData.Tickets, pageIndex, PageSize);
                return View(paginatedList);
            }
            return StatusCode(500, response);
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> SearchByEventTitle(string eventTitle, int pageIndex = 1)
        {


            HttpResponseMessage eventResponse = client.GetAsync($"https://localhost:7182/api/events/SearchEventsByTitle/{eventTitle}").Result;

                var eventjsonContent = await eventResponse.Content.ReadAsStringAsync();

                var eventresponseData = System.Text.Json.JsonSerializer.Deserialize<GetEventResponse>(eventjsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<EventViewModel> eventpaginatedList = await PagenatedList<EventViewModel>.CreateAsync(eventresponseData.Events, pageIndex, PageSize);

            if(eventpaginatedList.Count==0)
            {
                return StatusCode(500, "Event not found");
            }
            int id = eventpaginatedList[0].Id;
            //return StatusCode(500, id);
            HttpResponseMessage response = client.GetAsync(uri + $"/SearchTicketsByEventId/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();

                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetTicketResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<TicketViewModel> paginatedList = await PagenatedList<TicketViewModel>.CreateAsync(responseData.Tickets, pageIndex, PageSize);
                ViewData["CurrentFilter"] = eventTitle;
                return View("Index", paginatedList);
            }
            return View("Index");
        }

        [LoggedUserFilter]

        public IActionResult Create()
        {
            return View();
        }

        [LoggedUserFilter]
        [HttpPost]
        public async Task<IActionResult> Create([FromRoute]int id, TicketModel ticket)
        {
            if (!AuthUser.LoggedUser.IsAdmin)
            {
                ticket.UserId = AuthUser.LoggedUser.Id;
                ticket.EventId = id;
            }
            

            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(ticket);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateTicket", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(ticket);
            }
        }

        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = uri;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            HttpResponseMessage response = await client.DeleteAsync(uri + $"/DeleteTicket/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [LoggedUserFilter]
        [AdminFilter]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await client.GetAsync(uri + $"/GetTicketById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var userJson = await response.Content.ReadAsStringAsync();
            var eventResult = JsonConvert.DeserializeObject<TicketModel>(userJson);

            //return StatusCode(500, $"{response}");
            return View(eventResult);
        }

        [LoggedUserFilter]
        [AdminFilter]
        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> Edit(int id, TicketModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with the current model to show validation errors
                return View(model);
            }

            // Create the UpdateUserRequest object
            var updateRequest = new UpdateTicketRequest(id, model);

            string data = JsonConvert.SerializeObject(updateRequest);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsJsonAsync(uri + $"/UpdateTicket/{id}", content);
            //return StatusCode(500, $"{data}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return StatusCode(500, $"{data}");
            // Log the response or extract error messages to provide feedback to the user
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Update failed. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}, Error: {errorMessage}");

            return View(model);
        }
    }
}
