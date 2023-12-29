using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.AccountBL
{
    public interface IUserBL : IBaseBL<UsersEntity>
    {
        public UsersEntity GetUsers(string userName);
        public UsersEntity InsertUser(UsersEntity record);
        public UserDetailsEntity GetUserDetail(Guid id);
        public Guid UpdateUserDetail(UserDetailsEntity entity, Guid id);
    }
}
