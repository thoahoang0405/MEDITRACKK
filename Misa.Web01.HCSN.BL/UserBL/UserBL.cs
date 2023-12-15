using MEDITRACK.COMMON.Entities;
using MEDITRACK.DL.AccountDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.AccountBL
{
    public class UserBL:BaseBL<UsersEntity>, IUserBL
    {
        private IUserDL _userDL;
        public UserBL(IUserDL userDL):base(userDL)
        {
            _userDL = userDL;
        }
    }
}
