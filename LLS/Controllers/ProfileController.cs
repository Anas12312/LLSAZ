using Data.Services.Intefaces;
using Entities.Dto.ProfileDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileController(IUnitOfWork unitOfWork,
                                UserManager<IdentityUser> userManager)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPeofile()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (loggedInUser == null)
            {
                return BadRequest("User not found");
            }

            var identityId = new Guid(loggedInUser.Id);

            var profile = await _unitOfWork.Users.GetUserByIdentityId(identityId);

            if (profile == null)
            {
                return BadRequest("User not found");
            }

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto updateProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload");
            }

            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (loggedInUser == null)
            {
                return BadRequest("User not found");
            }

            var identityId = new Guid(loggedInUser.Id);

            var profile = await _unitOfWork.Users.GetUserByIdentityId(identityId);

            if (profile == null)
            {
                return BadRequest("User not found");
            }

            profile.Country = updateProfile.Country;
            profile.PhoneNumber = updateProfile.PhoneNumber;

            var isUpdated = await _unitOfWork.Users.Update(profile.Id, profile);

            if (!isUpdated)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(profile);
        }

        [HttpGet("get-current-role")]
        public async Task<IActionResult> GetCurrentUserRole()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

            if (loggedInUser == null)
            {
                return BadRequest("User not found");
            }

            var identityId = new Guid(loggedInUser.Id);

            var profile = await _unitOfWork.Users.GetUserByIdentityId(identityId);

            if (profile == null)
            {
                return BadRequest("User not found");
            }

            var role = profile.Role;
            return Ok(role);
        }
    }
}
