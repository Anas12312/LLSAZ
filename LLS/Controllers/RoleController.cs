using Data.Services.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(IUnitOfWork _unitOfWork,
                              UserManager<IdentityUser> userManager,
                              RoleManager<IdentityRole> roleManager)
        : base(_unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            //Check if role exist
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    return Ok(new { result = $"The Role {roleName} hase been added successfully" });
                }

                return BadRequest(new { error = $"The Role {roleName} hase not been added" });
            }

            return BadRequest(new { error = "Role already exists" });
        }

        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleManager.Roles.ToListAsync();

            return Ok(result);
        }

        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "User does not exist" });
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            
            if (!roleExist)
            {
                return BadRequest(new { error = "Role does not exist" });
            }

            

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var userRole in userRoles)
            {
                if(userRole.ToLower() == roleName.ToLower())
                {
                    return BadRequest(new { error = "User already got this role" });
                }
            }

            if(userRoles.Count == 2)
            {
                await _userManager.RemoveFromRoleAsync(user, userRoles[1].ToString());
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(new { error = $"The user was not able to be added to the role" });
            }

            //Add Role in User model Db
            var id = new Guid(user.Id);
            var userModel = await _unitOfWork.Users.GetUserByIdentityId(id);
            userModel.Role = roleName;
            var updateUserRole = await _unitOfWork.Users.UpdateIdentityId(id,userModel);
            await _unitOfWork.SaveAsync();
            if(!updateUserRole)
            {
                return BadRequest(new { error = $"The user was not able to be added to the role" });
            }
 
            return Ok(new { result = $"The role {roleName} added to the user successfully" });    
        }

        [HttpGet("get-user-roles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "User does not exist" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        [HttpPost("remove-role-from-user")]
        public async Task<IActionResult> ReomveRoleFromUser(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "User does not exist" });
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest(new { error = "Role does not exist" });
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            
            if (userRoles.Count == 1 && roleName.ToLower() != userRoles[0].ToLower())
            {
                return BadRequest(new { error = "User doesn't have this role" });
            }
            

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(new { error = $"Unable to reomove User form role {roleName}" });
            }

            //Add Role in User model Db
            var id = new Guid(user.Id);
            var userModel = await _unitOfWork.Users.GetUserByIdentityId(id);
            userModel.Role = "User";
            var updateUserRole = await _unitOfWork.Users.UpdateIdentityId(id, userModel);
            await _unitOfWork.SaveAsync();
            if (!updateUserRole)
            {
                return BadRequest(new { error = $"The user was not able to be added to the role" });
            }
            
            return Ok(new { result = $"The role {roleName} removed from the user successfully" });
        }

        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest(new { error = "Role does not exist" });
            }

            var role = await _roleManager.FindByNameAsync(roleName);

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(new { error = $"Unable to delete role {roleName}" });
            }

            var resultDb = await _unitOfWork.Users.DeleteRole(roleName);
            if(!resultDb)
            {
                return BadRequest(new { error = $"Unable to delete role {roleName}" });
            }
            await _unitOfWork.SaveAsync();

            return Ok(new { result = $"The role {roleName} have been deleted successfully" });
        }
    }
}
