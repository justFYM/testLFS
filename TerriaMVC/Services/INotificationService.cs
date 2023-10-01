using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public interface INotificationService
    {
        Task<string> findTotalNotifications();
    }
    public class NotificationService : INotificationService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly INotificationRepository notificationRepository;
        private readonly IInstrumentService instrumentService;

        public NotificationService(IWebHostEnvironment webHostEnvironment, INotificationRepository notificationRepository, IInstrumentService instrumentService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.notificationRepository = notificationRepository;
            this.instrumentService = instrumentService;
        }

        public async Task<string> findTotalNotifications()
        {
            var view = await selectView();
            NotificationDTO notifications = await notificationRepository.findTotalNotifications();

            var content = await insertNotifications(view, notifications);
            return content;
        }



        public async Task<string> selectView()
        {
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Notifications", "Notifications.html");
            return filePath;
        }
        public async Task<string> insertNotifications(string view, NotificationDTO notifications)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var content = await System.IO.File.ReadAllTextAsync(view);
            var startTag = "[[ITERAR]]";
            var endTag = "[[FIN_ITERAR]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;
            if (startIndex >= 0 && endIndex >= 0)
            {
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);
                
                var iteratedContent = new StringBuilder();
                //Iterar: podría retornar iteratedContent
                var i = 1;
                foreach (var iterador in notifications.notifications)
                {
                    Instrument instrument = await instrumentService.findBySensorId(iterador.sensor_id);
                    string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);
                    string color = "";
                    if (iterador.color_id == 1)
                    {
                        color = "bg-success";
                    }
                    else if (iterador.color_id == 2)
                    {
                        color = " bg-warning";
                    }
                    else if (iterador.color_id == 4)
                    {
                        color = "bg-danger";
                    }
                    else if (iterador.color_id == 3)
                    {
                        color = "bg-primary";
                    }
                    extractedContent = extractedContent.Replace("{{color}}", color);
                    extractedContent = extractedContent.Replace("{{datetime}}", iterador.notification_timestamp.ToString());
                    extractedContent = extractedContent.Replace("{{nombreInstrumento}}", instrument.getInstrumentName()); 
                    extractedContent = extractedContent.Replace("{{message}}", "El sensor "+iterador.sensor_name+" obtuvo el valor "+iterador.notification_message);
                    extractedContent = extractedContent.Replace("{{i}}", i.ToString());
                    extractedContent = extractedContent.Replace("{{nombreTipoInstrumento}}", instrument.getInstrumentTypeName());
                    extractedContent = extractedContent.Replace("{{idInstrumento}}", instrument.getId().ToString());
                    extractedContent = extractedContent.Replace("{{idSensor}}", iterador.sensor_id.ToString());
                    extractedContent = extractedContent.Replace("{{nombreSensor}}", iterador.sensor_name.ToString());
                    iteratedContent.AppendLine(extractedContent);
                    i++;
                }
                //Completar datos:
                content = startContent + iteratedContent.ToString() + endContent;

                return content;
            }
            return "";
        }
    }
}
