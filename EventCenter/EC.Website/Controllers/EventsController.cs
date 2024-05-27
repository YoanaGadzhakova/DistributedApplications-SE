using Azure.Core;
using EC.Data.Entities;
using EC.Infrastructure.Messaging.Requests;
using EC.Infrastructure.Messaging.Requests.EventsRequests;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using EC.Website.Filters;
using EC.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace EC.Website.Controllers
{
    public class EventsController : Controller
    {


        private readonly Uri uri = new("https://localhost:7182/api/events");
        HttpClient client;
        private const int PageSize = 1;

        public EventsController()
        {
            client = new HttpClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
        }
        //public async Task<IActionResult> Index()
        //{
        //    var products = _context.Products.AsNoTracking();
        //    var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageIndex, PageSize);
        //    return View(paginatedList);
        //}

        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            List<EventViewModel> modelList = new List<EventViewModel>();
            HttpResponseMessage response = client.GetAsync(uri + "/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetEventResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                PagenatedList<EventViewModel> paginatedList = await PagenatedList<EventViewModel>.CreateAsync(responseData.Events, pageIndex, PageSize);
                return View(paginatedList);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchByTitle(string title, int pageIndex = 1)
        {

            HttpResponseMessage response = client.GetAsync(uri + $"/SearchEventsByTitle/{title}").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();

                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetEventResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<EventViewModel> paginatedList = await PagenatedList<EventViewModel>.CreateAsync(responseData.Events, pageIndex, PageSize);
                ViewData["CurrentFilter"] = title;
                return View("Index", paginatedList);
            }
            return View("Index");
        }


        [LoggedUserFilter]
        [AdminFilter]
        public IActionResult Create()
        {
            return View();
        }

        [LoggedUserFilter]
        [AdminFilter]
        [HttpPost]
        public async Task<IActionResult> Create(EventModel newEvent)
        {

            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(newEvent);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateEvent", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(newEvent);

            }
        }
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = uri;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            HttpResponseMessage response = await client.DeleteAsync(uri + $"/DeleteEvent/{id}");

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
            var response = await client.GetAsync(uri + $"/GetEventById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var userJson = await response.Content.ReadAsStringAsync();
            var eventResult = JsonConvert.DeserializeObject<EventModel>(userJson);
            return View(eventResult);
        }

        [LoggedUserFilter]
        [AdminFilter]
        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> Edit(int id, EventModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with the current model to show validation errors
                return View(model);
            }

            // Create the UpdateUserRequest object
            var updateRequest = new UpdateEventRequest(id, model);

            string data = JsonConvert.SerializeObject(updateRequest);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(uri + $"/UpdateEvent/{id}", content);
            return StatusCode(500, $"{data}");

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
