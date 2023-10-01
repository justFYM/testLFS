namespace TerriaMVC.DTO
{
    public class TriggerDTO
    {
        public int idSensor { get; set; }
        public List<string> fechasCalendario { get; set; } = new List<string>();
        public List<string> eventos { get; set; } = new List<string>();
        public string selectedDate { get; set; }
        public string selectedEvent { get; set; }
        public List<Sensor> sensores { get; set; } = new List<Sensor>();
    }
}
