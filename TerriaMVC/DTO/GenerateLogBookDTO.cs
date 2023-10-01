namespace TerriaMVC.DTO
{
    public class GenerateLogBookDTO
    {
        public int IdInstrumento { get; set; }
        public List<SensorNote>? SensorNotes { get; set; } = new List<SensorNote?>();
        public List<Sensor>? Sensors { get; set; } = new List<Sensor?>();
        public bool? SaveNote { get; set; } = false;
        public int? IdSensor { get; set; }
        public string? Note { get; set; }
        public string? DateTimeString { get; set; }
        public int getIdInstrumento()
        {
            return this.IdInstrumento;
        }
    }
    public class SensorNote
    {
        public DateTime note_timestamp { get; set; }
        public string sensor_name { get; set; }
        public string sensor_note_name { get; set; }
    }
}
