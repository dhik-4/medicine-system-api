using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using MedicineSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicineSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserController(IConfiguration config, IUserRepository userRepository, JwtService jwtService)
        {
            _config = config;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTrial([FromBody] User_Input input, CancellationToken cancellationToken)
        {
            return Ok(input);
        }

            [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<List<User>>> Login([FromBody] User_Input input, CancellationToken cancellationToken)
        {
            try
            {
                var datas = await _userRepository.ValidateUsers(input.UserName, input.Password, cancellationToken);
                if (datas is null)
                {
                    return Unauthorized(
                        "incorrect username or password"
                    );
                }

                var accessToken = _jwtService.GenerateAccessToken(datas);
                //Guid.NewGuid().ToString();

                //await _refreshTokenRepository.CreateAccessAsync(datas.Id, accessToken);
                //await _refreshTokenRepository.CreateRefreshAsync(datas.Id, refreshToken);

                return Ok(Helpers.Success(new
                {
                    user = new { datas.Userid, datas.UserName, datas.RoleId },
                    access_token = accessToken
                }));
            }
            catch (Exception ex)
            {
            }
            return BadRequest(Helpers.Fail(
                        "ERR_INVALID_CREDS",
                        "Error in generate token"
                    ));
        }

        private string GenerateAccessToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            
            //string tempTokenLimit = jwt["AccessTokenSeconds"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim("UserId", user.Userid.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"])
            );

            var creds = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(
                    int.Parse(jwt["AccessTokenSeconds"])), // seconds
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
