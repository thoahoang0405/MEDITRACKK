using MEDITRACK.COMMON.Entities;
using MEDITRACK.DL.FamilyMemberDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.FamilyMemberBL
{
    public class FamilyMemberBL:BaseBL<FamilyMembersEntity>, IFamilyMemberBL
    {
        #region contructor
        private IFamilyMemberDL _FamilyMemberDL;
        public FamilyMemberBL(IFamilyMemberDL FamilyMemberDL) : base(FamilyMemberDL)
        {
            _FamilyMemberDL = FamilyMemberDL;
        }

        #endregion
    }
}
