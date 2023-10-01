using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public class Radar01Service : ServiceBaseMethods
    {
        private readonly IInstrumentService instrumentService;
        private readonly IRepositoryBaseMethods repositoryBaseMethods;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Radar01Repository radar01Repository;

        public Radar01Service(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, IWebHostEnvironment webHostEnvironment, Radar01Repository radar01Repository) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
            this.instrumentService = instrumentService;
            this.repositoryBaseMethods = repositoryBaseMethods;
            this.webHostEnvironment = webHostEnvironment;
            this.radar01Repository = radar01Repository;
        }

        public async Task<string> generateRadarGraph(Instrument instrument, GenerateGraphDTO dataRequest)
        {
            string archivo = $"GraphType{dataRequest.getIdTypeGraph()}.html";
            //Console.WriteLine("Entró acá.");
            string vista = await instrumentService.selectView(instrument, archivo);
            string graph = "";
            if (dataRequest.getIdTypeGraph() == 0)
            {
                SensorDataDTO graphData = await repositoryBaseMethods.generateDataTypeGraph0(instrument, dataRequest);
                graph = await insertGraphDataType0(instrument, graphData, dataRequest, vista);
            }
            else if (dataRequest.getIdTypeGraph() == 1)
            {
                SensorDataDTO graphData = await repositoryBaseMethods.generateDataTypeGraph1(instrument, dataRequest);
                graph = await insertGraphDataType1Radar(instrument, graphData, dataRequest, vista);
            }
            return graph;
        }

        public async Task<string> insertGraphDataType1Radar(Instrument instrument, SensorDataDTO sensorData, GenerateGraphDTO dataRequest, string vista)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var content = await System.IO.File.ReadAllTextAsync(vista);
            if (dataRequest.getAverageRequired())
            {
                //Se sacan solamente los identificadores.
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
                    foreach (var sensor in sensorData.sensores)
                    {
                        var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));
                        var chartOptionsSeries = "";
                        chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                        //if (instrument is Trigger)
                        //if (instrument is Piezometro)
                        //if (instrument is SensorHumedad)
                        //if (instrument is Prisma)
                        iteratedContent.AppendLine(chartOptionsSeries);
                    }
                    //Completar datos:
                    content = startContent + iteratedContent.ToString() + endContent;
                    //Encontrar sensor que contenga la menor y mayor fecha.
                    Sensor sensorCategories = await findChartOptionsCategories(sensorData);
                    var chartOptionsCategories = $"{string.Join(",", sensorCategories.sensor_eje_x.Select(date => $"'{date}'"))}";

                    content = content.Replace("{{sensor.sensor_eje_x}}", chartOptionsCategories);
                    var chartOptionsTitle = "";
                  
                    if (instrument is Radar01)
                    {
                        if (dataRequest.getNombreSensor().Equals("Velocidad%20en"))
                        {
                            chartOptionsTitle = $"Velocidades {instrument.getInstrumentTypeName()} {(dataRequest.getAverageRequired() ? "Promedios" : "")}";
                        }
                        else
                        {
                            chartOptionsTitle = $"Desplazamientos {instrument.getInstrumentTypeName()} {(dataRequest.getAverageRequired() ? "Promedios" : "")}";
                        }

                    }
                    content = content.Replace("{{title}}", chartOptionsTitle);
                    content = content.Replace("[[CHECK_SELECT]]", "");
                    content = content.Replace("[[END_CHECK_SELECT]]", "");
                    content = content.Replace("[[CHECK_SCRIPT_SELECT]]", "");
                    content = content.Replace("[[END_CHECK_SCRIPT_SELECT]]", "");

                    content = content.Replace("{{idInstrumento}}", dataRequest.getIdInstrumento().ToString());
                    content = content.Replace("{{idTypeGraph}}", dataRequest.getIdTypeGraph().ToString());
                    content = content.Replace("{{averageRequired}}", dataRequest.getAverageRequired().ToString().ToLower());
                    content = content.Replace("{{nombreSensor}}", dataRequest.getNombreSensor().ToString());
                    content = content.Replace("{{reDrawGraph}}", "true");

                    return content;
                }
                //Completar algunos datos.
            }
            else
            {
                var startTag = "[[ITERAR]]";
                var endTag = "[[FIN_ITERAR]]";
                var startIndex = content.IndexOf(startTag);
                var endIndex = content.IndexOf(endTag) + endTag.Length;
                //Se saca el contenido entremedio de los identificadores e identificadores.
                

                if (startIndex >= 0 && endIndex >= 0)
                {
                    var startContent = content.Substring(0, startIndex);
                    var endContent = content.Substring(endIndex);
                    var iteratedContent = new StringBuilder();
                    //Iterar: podría retornar iteratedContent
                    foreach (var sensor in sensorData.sensores)
                    {
                        var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));
                        var chartOptionsSeries = "";
                        chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                        iteratedContent.AppendLine(chartOptionsSeries);
                    }
                    //Completar datos:
                    content = startContent + iteratedContent.ToString() + endContent;
                    //Encontrar sensor que contenga la menor y mayor fecha.
                    Sensor sensorCategories = await findChartOptionsCategories(sensorData);
                    var chartOptionsCategories = $"{string.Join(",", sensorCategories.sensor_eje_x.Select(date => $"'{date}'"))}";

                    content = content.Replace("{{sensor.sensor_eje_x}}", chartOptionsCategories);
                    var chartOptionsTitle = "";

                    if (instrument is Radar01)
                    {
                        if (dataRequest.getNombreSensor().Equals("Velocidad%20en"))
                        {
                            chartOptionsTitle = $"Velocidades {instrument.getInstrumentTypeName()} {(dataRequest.getAverageRequired() ? "Promedios" : "")}";
                        }
                        else
                        {
                            chartOptionsTitle = $"Desplazamientos {instrument.getInstrumentTypeName()} {(dataRequest.getAverageRequired() ? "Promedios" : "")}";
                        }

                    }
                    content = content.Replace("{{title}}", chartOptionsTitle);
                
                    var startTagSelect = "[[CHECK_SELECT]]";
                    var endTagSelect = "[[END_CHECK_SELECT]]";
                    var startIndexSelect = content.IndexOf(startTagSelect);
                    var endIndexSelect = content.IndexOf(endTagSelect) + endTagSelect.Length;
                    string extractedContentSelect = content.Substring(startIndexSelect + startTagSelect.Length, endIndexSelect - startIndexSelect - startTagSelect.Length - endTagSelect.Length);
                    content = content.Replace(extractedContentSelect, "");
                    var startTagScriptSelect = "[[CHECK_SCRIPT_SELECT]]";
                    var endTagScriptSelect = "[[END_CHECK_SCRIPT_SELECT]]";
                    var startIndexScriptSelect = content.IndexOf(startTagScriptSelect);
                    var endIndexScriptSelect = content.IndexOf(endTagScriptSelect) + endTagScriptSelect.Length;
                    string extractedContentScriptSelect = content.Substring(startIndexScriptSelect + startTagScriptSelect.Length, endIndexScriptSelect - startIndexScriptSelect - startTagScriptSelect.Length - endTagScriptSelect.Length);
                    content = content.Replace(extractedContentScriptSelect, "");
                    content = content.Replace("[[CHECK_SELECT]]", "");
                    content = content.Replace("[[END_CHECK_SELECT]]", "");
                    content = content.Replace("[[CHECK_SCRIPT_SELECT]]", "");
                    content = content.Replace("[[END_CHECK_SCRIPT_SELECT]]", "");
                    return content;
                }
            }
            return "";
        }

        /*
        public async Task<string> generateLogBookCoordinador(GenerateLogBookDTO generateLogBookDTO)
        {
            string view = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "LogBook", "LogBook.html");
            string content = await System.IO.File.ReadAllTextAsync(view);

            var startTag = "[[ITERAR_TR]]";
            var endTag = "[[FIN_ITERAR_TR]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;
            var iteratedContent = new StringBuilder();
            if (startIndex >= 0 && endIndex >= 0)
            {
                //Ver si generar funciones que iteren y que completen datos.
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);
                var sortedSensorNotes = generateLogBookDTO.SensorNotes.OrderByDescending(note => note.note_timestamp);
                foreach (var iterador in sortedSensorNotes)
                {
                    string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);
                    extractedContent = extractedContent.Replace("{{note_timestamp}}", iterador.note_timestamp.ToString());
                    extractedContent = extractedContent.Replace("{{sensor_name}}", iterador.sensor_name.ToString());
                    extractedContent = extractedContent.Replace("{{sensor_note_name}}", iterador.sensor_note_name.ToString());
                    iteratedContent.AppendLine(extractedContent);
                }
                content = startContent + iteratedContent.ToString() + endContent;
            }
            content = content.Replace("{{sensors.Count()}}", generateLogBookDTO.Sensors.Count().ToString());
            var startTagOptions = "[[OPTIONS]]";
            var endTagOptions = "[[END_OPTIONS]]";
            var startIndexOptions = content.IndexOf(startTagOptions);
            var endIndexOptions = content.IndexOf(endTagOptions) + endTagOptions.Length;
            if (startIndexOptions >= 0 && endIndexOptions >= 0)
            {
                var startContentOptions = content.Substring(0, startIndexOptions);
                var endContentOptions = content.Substring(endIndexOptions);
                StringBuilder selectBuilder = new StringBuilder();
                foreach (var iterador in generateLogBookDTO.Sensors)
                {
                    string extractedContentOptions = content.Substring(startIndexOptions + startTagOptions.Length, endIndexOptions - startIndexOptions - startTagOptions.Length - endTagOptions.Length);
                    extractedContentOptions = extractedContentOptions.Replace("{{sensor_id}}", iterador.sensor_id.ToString());
                    extractedContentOptions = extractedContentOptions.Replace("{{sensor_name}}", iterador.sensor_name.ToString());
                    selectBuilder.AppendLine(extractedContentOptions);
                }
                content = startContentOptions + selectBuilder.ToString() + endContentOptions;
            }
            content = content.Replace("{{idInstrumento}}", generateLogBookDTO.IdInstrumento.ToString());
            return content;
        }
        */
        public async Task<GenerateLogBookDTO> FindLogBookCoordinadorByInstrumentId(GenerateLogBookDTO generateLogBookDTO)
        {
            generateLogBookDTO = await radar01Repository.FindLogBookCoordinadorByInstrumentId(generateLogBookDTO);
            return generateLogBookDTO;
        }

    }
}
