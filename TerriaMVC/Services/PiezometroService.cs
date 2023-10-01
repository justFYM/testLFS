using Microsoft.AspNetCore.Hosting;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO.Pipelines;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public class PiezometroService : ServiceBaseMethods
    {
        private readonly PiezometroRepository piezometroRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PiezometroService(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, PiezometroRepository piezometroRepository, IWebHostEnvironment webHostEnvironment) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
            this.piezometroRepository = piezometroRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<ConsolidadoAOIDTO> generateDataAOIConsolidado(string identifiers)
        {
            ConsolidadoAOIDTO instrumentsData = await piezometroRepository.generateDataAOIConsolidado(identifiers);
            return instrumentsData;
        }
        public async Task<string> insertGraphDataType1ConsolidadoAOI(ConsolidadoAOIDTO instrumentsData)
        {

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var view = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Piezometro", "GraphType1.html");
             var content = await System.IO.File.ReadAllTextAsync(view);
            var startTag = "[[ITERAR]]";
            var endTag = "[[FIN_ITERAR]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;
            if (startIndex >= 0 && endIndex >= 0)
            {
                //Ver si generar funciones que iteren y que completen datos.
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);
                var iteratedContent = new StringBuilder();
                //Iterar: podría retornar iteratedContent
                foreach (var iteradorInstrumentos in instrumentsData.instrumentos)
                {
                    foreach (var iteradorSensores in iteradorInstrumentos.sensores)
                    {
                        var sensorEjeYFormatted = string.Join(",", iteradorSensores.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));
                       
                        var chartOptionsSeries = $@"{{name: '{iteradorSensores.sensor_name} {iteradorInstrumentos.instrument_name.ToString()}',data: [{sensorEjeYFormatted}]}},";
                        iteratedContent.AppendLine(chartOptionsSeries);
                    }
                    content = startContent + iteratedContent.ToString() + endContent;
                    Sensor sensorCategories = await findChartOptionsCategories(iteradorInstrumentos);
                    var chartOptionsCategories = $"{string.Join(",", sensorCategories.sensor_eje_x.Select(date => $"'{date}'"))}";

                    content = content.Replace("{{sensor.sensor_eje_x}}", chartOptionsCategories);
                    var chartOptionsTitle = "";
                  
                        chartOptionsTitle = $"Serie de Tiempo";
                        chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                        chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                  
                    content = content.Replace("{{title}}", chartOptionsTitle);
                }
               
               
                return content;
            }
            return content;

        }
    }

}
