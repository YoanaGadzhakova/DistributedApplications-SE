using Azure.Core;
using EC.ApplicationServices.Interfaces;
using EC.Data.Entities;
using EC.Infrastructure.Messaging.Requests.UserssRequests;
using EC.Infrastructure.Messaging.Responses;
using EC.Infrastructure.Messaging.Responses.EventsResponses;
using EC.Infrastructure.Messaging.Responses.UsersResponses;
using EC.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.ApplicationServices.Implementations
{
    public class UsersManagementService : BaseManagementService, IUsersManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersManagementService(ILogger<EventsManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            _unitOfWork.Users.Insert(new()
            {
                Id = request.User.Id,
                FirstName = request.User.FirstName,
                LastName = request.User.LastName,
                Email = request.User.Email,
                BirthDate = request.User.BirthDate,
                Username = request.User.Username,
                Password = request.User.Password,
                Gender = request.User.Gender,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                IsActivated = true,
            });
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

            if (user == null)
            {
                _logger.LogError($"User with identifier {request.Id} not found");
                throw new Exception("");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            GetUserResponse response = new() { Users = new() };

            var users = await _unitOfWork.Users.GetAllAsync(request.IsActive);

            foreach (var user in users)
            {
                response.Users.Add(new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    Username = user.Username,
                    Password = user.Password,
                    Gender = user.Gender,
                    IsAdmin = user.IsAdmin
                });
            }

            return response;
        }

        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            if (_unitOfWork.Users.GetByIdAsync(request.CurrentUserId) == null)
            {
                _logger.LogError($"User with identifier {request.CurrentUserId} not found");
                throw new Exception("");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(request.CurrentUserId);

            user.Id = request.CurrentUserId;
            user.FirstName = request.User.FirstName;
            user.LastName = request.User.LastName;
            user.Email = request.User.Email;
            user.BirthDate = request.User.BirthDate;
            user.Username = request.User.Username;
            user.Password = request.User.Password;
            //user.Gender = request.User.Gender;
            user.IsActivated = true;
            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return new();
        }
        public async Task<UserViewModel> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("");
            }
            UserViewModel result = new UserViewModel()
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Username = user.Username,
                Password = user.Password,
                Gender = user.Gender,
                IsAdmin = user.IsAdmin
            };
            return result;
        }

        public async Task<GetUserResponse> SearchUsersByFirstName(string firstName)
        {
            GetUserResponse response = new() { Users = new() };
            var users = await _unitOfWork.Users.GetAllAsync();
            var filteredUsers = users.Where(x => x.FirstName == firstName).ToList();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in filteredUsers)
            {
                UserViewModel current = new UserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    Username = user.Username,
                    Password = user.Password,
                    Gender = user.Gender,
                    IsAdmin = user.IsAdmin
                };

                response.Users.Add(current);
            }


            return response;
        }
    }
}
