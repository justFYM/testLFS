namespace TerriaMVC.DTO
{
    public class InstrumentPopUpDTO
    {
        public int IdInstrumento { get; set; }
        public int IdServicio { get; set; }
        public int IdSensor { get; set; }
        public string NombreTipoInstrumento { get; set; }
        public string? NombreSensor { get; set; }
        public string? NombreVariable { get; set; }
    }
}
