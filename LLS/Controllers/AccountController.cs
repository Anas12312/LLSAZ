using Data.Services.Intefaces;
using Entities.Dto.AuthenticationDto.Outcoming;
using Entities.Dto.RefreshTokens.Incoming;
using LLS.Entities.Dto.AuthenticationDto.Incoming;
using LLS.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterReqDto registerReqDto)
        {
            // Check the model is valid
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.AccountRepository.Register(registerReqDto);
                await _unitOfWork.SaveAsync();
                return Ok(result);
            }
            else // Invalid object
            {
                return BadRequest(new RegisterResDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDto loginReqDto)
        {
            // check if eamil exist
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.AccountRepository.Login(loginReqDto);
                await _unitOfWork.SaveAsync();
                return Ok(result);
            }
            else // Invalid object
            {
                return BadRequest(new LoginResDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.AccountRepository.RefreshToken(tokenRequest);
                return Ok(result);
            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }
    }
}
