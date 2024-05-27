using EC.Infrastructure.Messaging.Responses.UsersResponses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using System.Text;
using EC.Infrastructure.Messaging.Requests;
using EC.Website.Filters;
using EC.Website.Models;
using Microsoft.EntityFrameworkCore;

namespace EC.Website.Controllers
{
    public class UsersController : Controller
    {


        private readonly Uri uri = new("https://localhost:7182/api/users");
        HttpClient client;
        private const int PageSize = 1;

        public UsersController()
        {
            client = new HttpClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel model)
        {
            List<UserViewModel> modelList = new List<UserViewModel>();
            HttpResponseMessage response = client.GetAsync(uri + "/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                UserViewModel loggedUser = responseData.Users.FirstOrDefault(user => user.Username == model.Username && user.Password == model.Password);
                if (loggedUser == null)
                {
                    ModelState.AddModelError("login error", "Wrong Username or Password");
                    return View(model);
                }
                HttpContext.Session.SetInt32("loggedUserId", loggedUser.Id);
                AuthUser.LoggedUser = loggedUser;
                return RedirectToAction("Index", "Home");
            }
            return View();

        }


        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            HttpResponseMessage getResponse = client.GetAsync(uri + "/Get").Result;

            if (getResponse.IsSuccessStatusCode)
            {
                var jsonContent = await getResponse.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                if (responseData.Users.FirstOrDefault(user => user.Username == user.Username) != null)
                {
                    ModelState.AddModelError("User error", "This username is taken");
                    return View(user);
                }
                if (responseData.Users.FirstOrDefault(user => user.Email == user.Email) != null)
                {
                    ModelState.AddModelError("User error", "This mail is already taken");
                    return View(user);
                }
            }



            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateUser", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(user);

            }
        }
        [LoggedUserFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            AuthUser.LoggedUser = null;
            return RedirectToAction("Login");
        }
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            HttpResponseMessage response = client.GetAsync(uri + "/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                PagenatedList<UserViewModel> paginatedList = await PagenatedList<UserViewModel>.CreateAsync(responseData.Users, pageIndex, PageSize);
                return View(paginatedList);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchByFirstName(string firstName, int pageIndex = 1)
        {
            
            HttpResponseMessage response = client.GetAsync(uri + $"/SearchUsersByFirstName/{firstName}").Result;


            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<UserViewModel> paginatedList = await PagenatedList<UserViewModel>.CreateAsync(responseData.Users, pageIndex, PageSize);
                ViewData["CurrentFilter"] = firstName;
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
        public async Task<IActionResult> Create(UserModel user)
        {

            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateUser", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(user);

            }
        }
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = uri;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            HttpResponseMessage response = await client.DeleteAsync(uri + $"/DeleteUser/{id}");

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
            var response = await client.GetAsync(uri + $"/GetUserById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var userJson = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(userJson);
            return View(user);
        }
        [LoggedUserFilter]
        [AdminFilter]
        [HttpPost]
        [ActionName("SaveUser")]
        public IActionResult Edit(int id, UserModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with the current model to show validation errors
                return View(model);
            }

            // Create the UpdateUserRequest object
            var updateRequest = new UpdateUserRequest(id, model);

            string data = JsonConvert.SerializeObject(updateRequest);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            //return StatusCode(500, $"{id}");
            HttpResponseMessage response = client.PutAsync(uri + $"/UpdateUser/{id}", content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return StatusCode(500, $"{data}");
            // Log the response or extract error messages to provide feedback to the user
            var errorMessage = response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Update failed. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}, Error: {errorMessage}");

            return View(model);
        }

    }
}


