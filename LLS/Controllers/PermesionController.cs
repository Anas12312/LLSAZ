using Data.Services.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    public class PermesionController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermesionController(IUnitOfWork _unitOfWork,
                              UserManager<IdentityUser> userManager,
                              RoleManager<IdentityRole> roleManager)
        : base(_unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("add-claims-to-user")]
        public async Task<IActionResult> AddClaimToUser(string email, string claimName, string claimValue)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "User does not exist" });
            }

            var userClaim = new Claim(claimName, claimValue);

            var result = await _userManager.AddClaimAsync(user, userClaim);
            if (result.Succeeded)
            {
                return Ok(new { result = $"The claim {claimName} added to the user successfully" });
            }

            return BadRequest(new { error = $"The user was not able to be added to the claim" });
        }

        [HttpPost("add-claims-to-role")]
        public async Task<IActionResult> AddClaimToRole(string roleName, string claimName, string claimValue)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new { error = "Role does not exist" });
            }

            var userClaim = new Claim(claimName, claimValue);

            var result = await _roleManager.AddClaimAsync(role, userClaim);
            if (result.Succeeded)
            {
                return Ok(new { result = $"The claim {claimName} added to the role successfully" });
            }

            return BadRequest(new { error = $"The role was not able to be added to the claim" });
        }

        [HttpGet("get-all-claims-for-user")]
        public async Task<IActionResult> GetAllClaimsForUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "User does not exist" });
            }

            var result = await _userManager.GetClaimsAsync(user);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(new { error = $"There is No claims for that User" });
        }

        [HttpGet("get-all-claims-for-role")]
        public async Task<IActionResult> GetAllClaimsForRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new { error = "Role does not exist" });
            }

            var result = await _roleManager.GetClaimsAsync(role);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(new { error = $"There is No claims for that Role" });
        }

        [HttpDelete("delete-claim-from-user")]
        public async Task<IActionResult> DeleteClaimFormUser(string email, string claimName, string claimValue)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "User does not exist" });
            }

            var claim = new Claim(claimName, claimValue);

            var result = await _userManager.RemoveClaimAsync(user, claim);
            if (result != null)
            {
                return Ok(new { result = $"The CLaim {claimName} was deleted successfully" });
            }

            return BadRequest(new { error = $"The Claim {claimName} was not able to be deleted" });
        }

        [HttpDelete("delete-claim-from-role")]
        public async Task<IActionResult> DeleteClaimFormRole(string roleName, string claimName, string claimValue)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new { error = "Role does not exist" });
            }

            var claim = new Claim(claimName, claimValue);

            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (result != null)
            {
                return Ok(new { result = $"The CLaim {claimName} was deleted successfully" });
            }

            return BadRequest(new { error = $"The Claim {claimName} was not able to be deleted" });
        }
    }
}
