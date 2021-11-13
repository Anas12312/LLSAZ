using Data.Services.Intefaces;
using Entities.DbSet;
using Entities.Dto.ExpermintDto;
using LLS.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpirmentController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ExpirmentController(IUnitOfWork unitOfWork,
                                    UserManager<IdentityUser> userManager)
        : base(unitOfWork)
        {
            _userManager = userManager;
        }

        [HttpPost("Add-New-Exp")]
        public async Task<IActionResult> CreatExpirment(string name)
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
            var expriment = new Expirment()
            {
                Name =name,
                //AuthorId = loggedInUser.Id,
                //AuthorName = loggedInUser.Email
            };

            var result = await _unitOfWork.ExpirmentRepository.AddExpirment(expriment);
            if(!result)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Something went wrong"
                });
            }
            await _unitOfWork.SaveAsync();
            return Ok(new Result()
            {
                Status = true,
                Data = expriment
            });
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<IActionResult> GetExpirment(string name, string id)
        //{
        //    if(name != null)
        //    {
        //        var epriment = await _unitOfWork.ExpirmentRepository.GetExpByName(name);
        //        if (epriment == null)
        //        {
        //            return BadRequest(new Result()
        //            {
        //                Status = false,
        //                Error = "There is no Expirment with this name"
        //            });
        //        }

        //        var expirmentDto = new ExpirmentDto()
        //        {
        //            Name = epriment.Name,
        //            AuthorName = epriment.AuthorName,
        //            AddedTime = epriment.AddedDate,
        //            UpdatedTime = epriment.UpdateDate
        //        };

        //        return Ok(new Result()
        //        {
        //            Status = true,
        //            Data = expirmentDto
        //        });
        //    }
            
        //    if(id != null)
        //    {
        //        var guidId = new Guid(id);
        //        var epriment = await _unitOfWork.ExpirmentRepository.GetExpById(guidId);
        //        if (epriment == null)
        //        {
        //            return BadRequest(new Result()
        //            {
        //                Status = false,
        //                Error = "There is no Expirment with this name"
        //            });
        //        }

        //        var expirmentDto = new ExpirmentDto()
        //        {
        //            Name = epriment.Name,
        //            AuthorName = epriment.AuthorName,
        //            AddedTime = epriment.AddedDate,
        //            UpdatedTime = epriment.UpdateDate
        //        };

        //        return Ok(new Result()
        //        {
        //            Status = true,
        //            Data = expirmentDto
        //        });
        //    }

        //    var expList = await _unitOfWork.ExpirmentRepository.GetAllExpirments();

        //    if (!expList.Any())
        //    {
        //        return BadRequest(new Result()
        //        {
        //            Status = false,
        //            Error = "There is no Expirment"
        //        });
        //    }

        //    var expListDto = new List<ExpirmentDto>();
        //    foreach (var exp in expList)
        //    {
        //        var expDto = new ExpirmentDto()
        //        {
        //            Name = exp.Name,
        //            AuthorName = exp.AuthorName,
        //            AddedTime = exp.AddedDate,
        //            UpdatedTime = exp.UpdateDate
        //        };
        //        expListDto.Add(expDto);
        //    }

            

        //    return Ok(new Result()
        //    {
        //        Status = true,
        //        Data = expListDto
        //    });
        //}

        [AllowAnonymous]
        [HttpGet("Get-Expermint-By-Name")]
        public async Task<IActionResult> GetExpByName(string ExpName)
        {
            var epriment = await _unitOfWork.ExpirmentRepository.GetExpByName(ExpName);
            if (epriment == null)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Expirment with this name"
                });
            }

            var expirmentDto = new ExpirmentDto()
            {
                Name = epriment.Name,
                AuthorName = epriment.AuthorName,
                AddedTime = epriment.AddedDate,
                UpdatedTime = epriment.UpdateDate
            };

            return Ok(new Result()
            {
                Status = true,
                Data = expirmentDto
            });
        }

        [HttpDelete("delete-Expirment")]
        public async Task<IActionResult> DeleteExpirment(string id)
        {
            var guidId = new Guid(id);
            var result = await _unitOfWork.ExpirmentRepository.Delete(guidId);
            if (!result)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Expirment with this name"
                });
            }

            await _unitOfWork.SaveAsync();

            return Ok(new Result()
            {
                Status = true
            });
        }

        [AllowAnonymous]
        [HttpGet("get-all-Exp")]
        public async Task<IActionResult> GetAllExpirment()
        {
            var expList = await _unitOfWork.ExpirmentRepository.GetAllExpirments();
            if (!expList.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Expirments"
                });
            }

            var listDto = new List<ExperimentByCourse>();
            foreach (var x in expList)
            {
                var dto = new ExperimentByCourse()
                {
                    ExperimentName = x.Name
                };
                listDto.Add(dto);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = listDto
            });
        }

        [HttpGet("Author")]
        public async Task<IActionResult> GetAllExpFromAuthor()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _unitOfWork.ExpirmentRepository.GetAllExpFromAuthor(loggedInUser.Id);

            if(!result.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Expirment from this user"
                });
            }

            return Ok(new Result()
            {
                Status = true,
                Data = result
            });

        }

        [AllowAnonymous]
        [HttpGet("Author/email={email}")]
        public async Task<IActionResult> GetAllExpfromAuthorId(string email)
        {
            var expList = await _unitOfWork.ExpirmentRepository.GetAllExpFromAuthor(email);

            if (!expList.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Expirment from this user"
                });
            }

            return Ok(new Result()
            {
                Status = true,
                Data = expList
            });
        }


    }
}
