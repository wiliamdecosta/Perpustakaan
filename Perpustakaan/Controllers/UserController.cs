using JustclickCoreModules.Filters;
using JustclickCoreModules.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perpustakaan.Data.Entities;
using Perpustakaan.Models.Requests;
using Perpustakaan.Models.Responses;
using Perpustakaan.Services;
using Perpustakaan.Utils;

namespace Perpustakaan.Controllers
{
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IConfiguration _configuration;
        public UserController(UserService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<BaseResponse<List<User>>> GetAllUsers([FromBody] SearchRequest request)
        {
            Paginated<User> paginatedItem = _service.FetchAll(request);

            var responseData = BaseResponse<List<User>>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("FETCH_ALL_USER_LIST")
                .Data(paginatedItem.Data.ToList())
                .Page(new PageResponse()
                {
                    Total = paginatedItem.TotalCount,
                    Size = paginatedItem.PageSize,
                    TotalPage = paginatedItem.TotalPages,
                    Current = paginatedItem.PageNumber,
                })
                .Build();

            return Ok(responseData);
        }

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public ActionResult<BaseResponse<List<User>>> ListUser()
        {
            List<User> user = _service.FetchAll();

            var responseData = BaseResponse<List<User>>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("REGISTER_USER_SUCCESS")
                .Data(user)
                .Build();

            return Ok(responseData);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BaseResponse<User>> ReqisterUser([FromBody] UserRequest request)
        {
            User user = _service.Register(request);

            var responseData = BaseResponse<User>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("REGISTER_USER_SUCCESS")
                .Data(user)
                .Build();

            return Ok(responseData);
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BaseResponse<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var loginResponse = new LoginResponse()
            {
                AccessToken = "",
                Email = "",
                IdUser = 0,
                RefreshToken = "",
                RefreshTokenExpiredTime = "",
            };

            var responseData = BaseResponse<LoginResponse>.Builder()
                        .Code(StatusCodes.Status200OK)
                        .Message("Username atau password salah");

            User? user = _service.FetchByEmail(request.Email);
            if(user != null)
            {
                if(!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return BadRequest(responseData.Data(loginResponse).Code(StatusCodes.Status400BadRequest).Build());
                }else
                {
                    var token = JwtUtil.GenerateJwtToken(user, _configuration);
                    var refreshToken = JwtUtil.GenerateRefreshToken();
                    var refreshTokenExpiryTime = DateTime.Now.AddDays(7); //1 minggu

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                    User updatedUser = _service.UpdateRefreshToken(user);
                    loginResponse.AccessToken = token;
                    loginResponse.RefreshToken = refreshToken;
                    loginResponse.RefreshTokenExpiredTime = refreshTokenExpiryTime.ToString("o");
                    loginResponse.Email = user.Email;
                    loginResponse.IdUser = user.Id;

                    return Ok(responseData.Data(loginResponse).Message("Login berhasil").Build());
                }
            }else
            {
                return BadRequest(responseData);
            }

        }
    }
}
