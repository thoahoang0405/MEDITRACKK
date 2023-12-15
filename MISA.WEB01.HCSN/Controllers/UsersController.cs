using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.AccountBL;
using MEDITRACK.COMMON.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
