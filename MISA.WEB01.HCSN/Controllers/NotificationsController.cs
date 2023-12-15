using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.NoticeBL;
using MEDITRACK.COMMON.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BasesController<NotificationsEntity>
    {
        private INotificationBL _notificationBL;
        public NotificationsController(INotificationBL notification) : base(notification)
        {
            _notificationBL = notification;
        }
    }
}
