namespace TerriaMVC.DTO
{
    public class GenerateGraphDTO
    {
        public int IdInstrumento { get; set; }
        public int IdTypeGraph { get; set; } /* 0: Una línea, 1: Más de una línea.*/
        public bool? AverageRequired { get; set; }
        public string? NombreVariable { get; set; }
        public string? NombreSensor { get; set; }
        public int? IdSensor { get; set; }
        public bool? ReDrawGraph { get; set; }
        public bool? ReDrawSelect { get; set; } = false;
        public string? FechaCalendario { get; set; }
        public int? HourToAverage { get; set; } = -1;
        public int getIdInstrumento()
        {
            return this.IdInstrumento;
        }
        public string getNombreVariable()
        {
            return this.NombreVariable;
        }
        public string getNombreSensor()
        {
            return this.NombreSensor;
        }
        public int getIdTypeGraph()
        {
            return this.IdTypeGraph;
        }
        public bool getAverageRequired()
        {
            return (bool)this.AverageRequired;
        }
        public int getIdSensor()
        {
            return (int)this.IdSensor;
        }
        public bool getReDrawGraph()
        {
            return (bool)this.ReDrawGraph;
        }
        public bool getReDrawSelect()
        {
            return (bool)this.ReDrawSelect;
        }
        public string getFechaCalendario()
        {
            return (string)this.FechaCalendario;
        }
        public int getHourToAverage()
        {
            return (int)this.HourToAverage;
        }


    }
}
