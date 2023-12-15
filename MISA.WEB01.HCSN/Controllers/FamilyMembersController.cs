using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.FamilyMemberBL;
using MEDITRACK.COMMON.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyMembersController : BasesController<FamilyMembersEntity>
    {
        #region Field
        private IFamilyMemberBL _familyMemberBL;
        #endregion

        public FamilyMembersController(IFamilyMemberBL familyBL) : base(familyBL)
        {
            _familyMemberBL = familyBL;
        }
    }
}
