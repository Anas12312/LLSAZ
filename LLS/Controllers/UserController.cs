using Data.Services.Data;
using Data.Services.Intefaces;
using Entities.DbSet;
using Entities.Dto.UserDto;
using LLS.Entities.Dto.AuthenticationDto.Incoming;
using LLS.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserController")]
    public class UserController : BaseController
    { 
        public UserController(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[HttpPost("add-user")]
        //public async Task<IActionResult> AddUser(UserDto userDto)
        //{
        //    var user = new User();
        //    user.FirstName = userDto.FirstName;
        //    user.Lastname = userDto.Lastname;
        //    user.Email = userDto.Email;

        //    var result = await _unitOfWork.Users.AddUser(user);
        //    await _unitOfWork.SaveAsync();


        //    if (result == true)
        //        return Ok("Added");

        //    return BadRequest("Something went wrong!");
        //}
        [HttpPost]
        [Route("Create-User-By-Admin")]
        public async Task<IActionResult> CreateUserByAdmin(RegisterReqDto registerReqDto)
        {
            // Check the model is valid
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.AccountRepository.Register(registerReqDto);
                await _unitOfWork.SaveAsync();
                return Ok(new Result()
                {
                    Status = true,
                    Data = "Done"
                });
            }
            else // Invalid object
            {
                return BadRequest(new Result
                {
                    Status = false,
                    Error = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }


        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _unitOfWork.Users.GetUserById(id);

            if (user != null)
            {
                var userDto = new UserDto();
                userDto.FirstName = user.FirstName;
                userDto.Lastname = user.Lastname;
                userDto.Email = user.Email;
                return Ok(user);
            }
            return BadRequest("User not found");

        }

        [HttpGet("get-user-by-email")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            if (user != null)
            {
                var userDto = new UserDto();
                userDto.FirstName = user.FirstName;
                userDto.Lastname = user.Lastname;
                userDto.Email = user.Email;
                return Ok(user);
            }
            return BadRequest("User not found");

        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _unitOfWork.Users.GetAllUsers();

            if (users.Any() == true)
            {
                var usersDto = new List<UserDto>();

                foreach (User user in users)
                {
                    var userDto = new UserDto();
                    userDto.FirstName = user.FirstName;
                    userDto.Lastname = user.Lastname;
                    userDto.Email = user.Email;

                    usersDto.Add(userDto);
                }

                return Ok(users);
            }
            return BadRequest("Something went wrong!");

        }

        [HttpGet("Get-All-Student")]
        public async Task<IActionResult> GetAllStudent()
        {
            var studnetList = await _unitOfWork.Users.GetAllStudnet();
            if (!studnetList.Any())
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Students"
                });

            var listDto = new List<StudentListDto>();

            foreach(var x in studnetList)
            {
                var y = new StudentListDto()
                {
                    StudentName = x.FirstName + " " + x.Lastname,
                    Email = x.Email,
                    Role = x.Role,
                    Status = x.Status.ToString(),
                    Updated = x.UpdateDate
                };
                listDto.Add(y);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = listDto
            });
        }

        [HttpGet("Get-All-Teachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var studnetList = await _unitOfWork.Users.GetAllTeacher();
            if (!studnetList.Any())
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Teachers"
                });

            var listDto = new List<StudentListDto>();

            foreach (var x in studnetList)
            {
                var y = new StudentListDto()
                {
                    StudentName = x.FirstName + " " + x.Lastname,
                    Email = x.Email,
                    Role = x.Role,
                    Status = x.Status.ToString(),
                    Updated = x.UpdateDate
                };
                listDto.Add(y);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = listDto
            });
        }

        [HttpGet("get-all-users-id")]
        public async Task<IActionResult> GetAllUserId()
        {
            var users = await _unitOfWork.Users.GetAllUsers();

            if (users.Any() == true)
            {
                return Ok(users);
            }
            return BadRequest("Something went wrong!");

        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(string email, UserDto userDto)
        {
            var OldUser = await _unitOfWork.Users.GetUserByEmail(email);
            var user = new User();
            user.FirstName = userDto.FirstName;
            user.Lastname = userDto.Lastname;

            var result = await _unitOfWork.Users.Update(OldUser.Id, user);
            await _unitOfWork.SaveAsync();

            if (result == true)
                return Ok("Updated");

            return BadRequest("User not found!");
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            var result = await _unitOfWork.Users.Delete(user.Id);
            await _unitOfWork.SaveAsync();

            if (result == true)
                return Ok("Deleted");

            return BadRequest("User not found or already deleted!");
        }

        [HttpDelete("activate-user")]
        public async Task<IActionResult> ActivateUser(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            var result = await _unitOfWork.Users.Activate(user.Id);
            await _unitOfWork.SaveAsync();

            if (result == true)
                return Ok("Activated");

            return BadRequest("User not found or already active!");
        }

    }
}
