using System.Diagnostics.Metrics;
using System;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using Newtonsoft.Json;
using System.Net.Http;

namespace TerriaMVC.Repository
{
    public interface INotificationRepository
    {
        Task<NotificationDTO> findTotalNotifications();
    }
    public class NotificationRepository : INotificationRepository
    {
        public NotificationRepository()
        {
            
        }
        public async Task<NotificationDTO> findTotalNotifications()
        {
          
            using (var httpClient = new HttpClient())
            {

            
                   var url = $"http://localhost:7294/api/Notifications";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    SensorDataDTO sensorDataDTO = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);
                    NotificationDTO notificationsDTO = JsonConvert.DeserializeObject<NotificationDTO>(jsonString);
                    return notificationsDTO;




                }
            else
            {
                return new NotificationDTO();
            }
            }
            
        }
    }
}
