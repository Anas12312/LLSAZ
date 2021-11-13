using Data.Services.Data;
using Data.Services.Intefaces;
using Entities.DbSet;
using Entities.Dto.AuthenticationDto.Outcoming;
using Entities.Dto.RefreshTokens.Incoming;
using LLS.Entities.Dto.AuthenticationDto.Incoming;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly AppDbContext _context;
        public AccountRepository(UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IOptionsMonitor<JwtConfig> optionsMonitor,
                                 IUserRepository userRepository,
                                 TokenValidationParameters tokenValidationParameters,
                                 AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _userRepository = userRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }


        public async Task<AuthResult> Login(LoginReqDto loginReqDto)
        {
            var userExist = await _userManager.FindByEmailAsync(loginReqDto.Email);
            if (userExist == null)
            {
                return new LoginResDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid authintaciton request"
                    }
                };
            }

            //check if user have valid password
            var isCorrect = await _userManager.CheckPasswordAsync(userExist, loginReqDto.Password);
            if (isCorrect)
            {
                //Generate Token
                var jwtToken = await GenerateJwtToken(userExist);

                return jwtToken;
            }
            else
            {
                //Wrong password
                return new LoginResDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid authintaciton request"
                    }
                };
            }
        }

        public async Task<AuthResult> Register(RegisterReqDto registerReqDto)
        {
            //Check if Email already Exisit
            var userExist = await _userManager.FindByEmailAsync(registerReqDto.Email);

            if (userExist != null) //Email exists
            {
                return new RegisterResDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Email already in use"
                    }
                };
            }
            //Add the user
            var newUser = new IdentityUser()
            {
                Email = registerReqDto.Email,
                UserName = registerReqDto.Email,
                EmailConfirmed = true //To Update to confirm email

            };

            var isCreated = await _userManager.CreateAsync(newUser, registerReqDto.Password);
            if (!isCreated.Succeeded)
            {
                return new RegisterResDto
                {
                    Success = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                };
            }
            //Add user to Db
            var user = new User();
            user.IdentityId = new Guid(newUser.Id);
            user.FirstName = registerReqDto.FirstName;
            user.Lastname = registerReqDto.LastName;
            user.Email = registerReqDto.Email;
            user.Role = "User";

            var result = await _userRepository.AddUser(user);

            //Add Role to User
            await _userManager.AddToRoleAsync(newUser, "User");

            //Create JWT token
            var token = await GenerateJwtToken(newUser);

            //Return the user
            return token;
        }

        public async Task<AuthResult> CreateUserByAdmin(RegisterReqDto registerReqDto)
        {
            //Check if Email already Exisit
            var userExist = await _userManager.FindByEmailAsync(registerReqDto.Email);

            if (userExist != null) //Email exists
            {
                return new RegisterResDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Email already in use"
                    }
                };
            }
            //Add the user
            var newUser = new IdentityUser()
            {
                Email = registerReqDto.Email,
                UserName = registerReqDto.Email,
                EmailConfirmed = true //To Update to confirm email

            };

            var isCreated = await _userManager.CreateAsync(newUser, registerReqDto.Password);
            if (!isCreated.Succeeded)
            {
                return new RegisterResDto
                {
                    Success = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                };
            }
            //Add user to Db
            var user = new User();
            user.IdentityId = new Guid(newUser.Id);
            user.FirstName = registerReqDto.FirstName;
            user.Lastname = registerReqDto.LastName;
            user.Email = registerReqDto.Email;
            user.Role = "User";

            var result = await _userRepository.AddUser(user);

            //Add Role to User
            await _userManager.AddToRoleAsync(newUser, "User");

            //Return the user
            return new RegisterResDto
            {
                Success = true
            };
        }

        public async Task<AuthResult> RefreshToken(TokenRequest tokenRequest)
        {
            var result = await VerifyAndGenerateToken(tokenRequest);

            if (result == null)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() {
                    "Invalid tokens"
                },
                    Success = false
                };
            }
            return result;
        }

        private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            // validation 1 - validate existence of the token
            if (storedToken == null)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() {
                            "Token does not exist"
                        }
                };
            }

            // Validation 2 - validate if used
            if (storedToken.IsUsed)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() {
                            "Token has been used"
                        }
                };
            }

            // Validation 3 - validate if revoked
            if (storedToken.IsRevorked)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() {
                            "Token has been revoked"
                        }
                };
            }

            try
            {
                // Validation 4 - Validation JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                // Validation 5 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validation 6 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has not yet expired"
                        }
                    };
                }


                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token doesn't match"
                        }
                    };
                }

                return null;
            }
            catch (SecurityTokenExpiredException)
            {
                // update current token 
                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                // Generate a new token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unable to decode the payload") ||
                    ex.Message.Contains("Signature validation failed") ||
                    ex.Message.Contains("Unable to decode the header"))
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Some thing went wrong"
                        }
                    };
                }

                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>() {
                            ex.Message
                        }
                };
            }
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        private async Task<AuthResult> GenerateJwtToken(IdentityUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetValidClaims(user);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            //Convert to string
            var jwtToken = jwtHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevorked = false,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpirayDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private async Task<List<Claim>> GetValidClaims(IdentityUser user)
        {
            var options = new IdentityOptions();

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Unique id
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // id for Token used for refresh token
            };

            // Getting the claims that we have assigned to user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                foreach(var userRole in userRoles)
                {
                    var _role = await _roleManager.FindByNameAsync(userRole);
                    if (_role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));

                        var roleClaims = await _roleManager.GetClaimsAsync(_role);
                        foreach (var roleClaim in roleClaims)
                        {
                            claims.Add(roleClaim);
                        }
                    }
                }
            }

            return claims;
        }

        private static string RandomString(int lenght)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJLKMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, lenght)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
