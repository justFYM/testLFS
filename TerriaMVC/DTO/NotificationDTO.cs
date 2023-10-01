namespace TerriaMVC.DTO
{
    public class NotificationDTO
    {
        public List<Notification> notifications { get; set; } = new List<Notification>();
    }
    public class Notification
    {
        public int notification_id { get; set; }
        public int color_id { get; set; }
        public DateTime notification_timestamp { get; set; }
        public int sensor_id { get; set; }
        public string notification_message { get; set; }
        public string sensor_name { get; set; }
    }
}
