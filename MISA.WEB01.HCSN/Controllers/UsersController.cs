using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.AccountBL;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Resource;
using MEDITRACK.DL.AccountDL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BasesController<UsersEntity>
    {
        private IUserBL _userBL;
        public UsersController(IUserBL userBL) : base(userBL)
        {
            _userBL = userBL;
        }
        public static WebApplicationBuilder? builder;

        public static IConfiguration configuration;
        /// <summary>
        /// đăng nhập hệ thống
        /// </summary>
        /// <param name="user">Thông tin đăng nhập</param>
        /// <returns>token </returns>
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UsersEntity user)
        {
            var users = _userBL.GetAllRecords();
            var check = users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

            
            if (check == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Đăng nhập thất bại!");
            }


            var key = Encoding.ASCII.GetBytes
              (configuration["Jwt:Key"]);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return StatusCode(StatusCodes.Status200OK, stringToken);


        }

        /// <summary>
        /// check đăng nhập
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            var id = User.Identity.Name;
            // Thực hiện các thao tác khác để lấy thông tin người dùng
            UsersEntity users = _userBL.GetUsers(id);
            return Ok(users);
        }
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPost("multiple")]
        public IActionResult InsertUser([FromBody] UsersEntity record)
        {
            var result= _userBL.InsertUser(record);
            return Ok(result);
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet("userDetail")]
        public IActionResult GetUserDetail(Guid id)
        {
            var result = _userBL.GetUserDetail(id);
            return Ok(result);
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPut("userDetail")]
        public IActionResult UpdateUserDetail([FromBody] UserDetailsEntity entity, [FromRoute] Guid id)
        {

            try
            {
                var numberOfAffectedRows = _userBL.UpdateUserDetail(entity, id);


                return StatusCode(StatusCodes.Status200OK, numberOfAffectedRows);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }
        }

    }
}
