namespace TerriaMVC.DTO
{
    public class GenerateAOIDTO
    {
        public int[] idInstrumentos { get; set; }
        public bool defaultContent { get; set; } //Mostrar la primera pestaña con el gráfico y los botónes de los instrumentos del tipo de instrumento.
        public bool? isForTop { get; set; }
        public bool? isForTopConsolidado { get; set; }
        public bool? isForBotConsolidado { get; set; }
    }
}
