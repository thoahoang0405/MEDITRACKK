using MEDITRACK.COMMON.Entities;
using MEDITRACK.DL.AccountDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        public UsersEntity GetUsers(string userName)
        {
            return _userDL.GetUsers(userName);
        }
        public UsersEntity InsertUser(UsersEntity record)
        {
            return _userDL.InsertUser(record);
        }
        public UserDetailsEntity GetUserDetail(Guid id)
        {
            return _userDL.GetUserDetail(id);
        }
        public Guid UpdateUserDetail(UserDetailsEntity entity, Guid id)
        {
            return _userDL.UpdateUserDetail(entity, id);
        }
    }
}
