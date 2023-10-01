using Newtonsoft.Json;

namespace TerriaMVC.DTO
{

    public class SensorDataDTO
    {
        public double? instrument_id { get; set; }
        public double? instrument_type_id { get; set; }
        public string? instrument_name { get; set; }
        public List<Sensor> sensores { get; set; } 
    }


    public class Sensor
    {
        public double sensor_id { get; set; }
        public string sensor_name { get; set; }

        public string instrument_name { get; set; }
        public List<double?> sensor_eje_y = new List<double?>();
        public List<string?> sensor_eje_x = new List<string?>();

    }

}
