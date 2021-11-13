using Entities.Dto.AuthenticationDto.Outcoming;
using Entities.Dto.RefreshTokens.Incoming;
using LLS.Entities.Dto.AuthenticationDto.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Intefaces
{
    public interface IAccountRepository
    {
        Task<AuthResult> Register(RegisterReqDto registerReqDto);
        Task<AuthResult> Login(LoginReqDto loginReqDto);
        Task<AuthResult> RefreshToken(TokenRequest tokenRequest);
    }
}
