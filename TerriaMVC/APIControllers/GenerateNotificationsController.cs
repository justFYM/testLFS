using Microsoft.AspNetCore.Mvc;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Services;

namespace TerriaMVC.APIControllers
{
    public class GenerateNotificationsController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public GenerateNotificationsController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet("api/GenerateNotificationsController", Name = "NotificationsController")]
        public async Task<IActionResult> NotificationsController()
        {
            string notifications = await notificationService.findTotalNotifications();
            /*
            foreach(var iterador in notifications.notifications)
            {
                Console.WriteLine(iterador.notification_id);
                Console.WriteLine(iterador.notification_message);
                Console.WriteLine(iterador.notification_timestamp);
                Console.WriteLine("--------------");
            }
            */
            return Ok(notifications);
        }
    }
}
