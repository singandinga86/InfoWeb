using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Interfaces;
using InfoWeb.Domain.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;

        public NotificationController(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            this.notificationRepository = notificationRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public IEnumerable<Notification> GetNotificationsByIdUser([FromRoute]int id)
        {
            return notificationRepository.GetNotificationByUserId(id);
        }

        [HttpPost("{id}")]
        public void SetNotificationsVisibility([FromRoute]int id)
        {
            notificationRepository.SetVisibilityNotification(id);
            unitOfWork.Commit();
        }
    }
}
