

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

using System.Globalization;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public interface IServiceBaseMethods
    {
        Task<string> generateButtons(Instrument instrument, GenerateButtonsDTO dataRequest);
        Task<string> generateGraph(Instrument instrument, GenerateGraphDTO generateGraphDTO);
        Task<string> generateLogBook(GenerateLogBookDTO generateLogBookDTO);
        Task<IActionResult> SaveLog(GenerateLogBookDTO generateLogBookDTO);
    }
    public class ServiceBaseMethods : IServiceBaseMethods
    {
        private readonly IInstrumentService instrumentService;
        private readonly IRepositoryBaseMethods repositoryBaseMethods;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ServiceBaseMethods(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, IWebHostEnvironment webHostEnvironment)
        {
            this.instrumentService = instrumentService;
            this.repositoryBaseMethods = repositoryBaseMethods;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> generateButtons(Instrument instrument, GenerateButtonsDTO dataRequest)
        {
            string archivo = $"ButtonsType{dataRequest.getIdTypeButtons()}.html";
            string vista = await instrumentService.selectView(instrument, archivo);
            string buttons = "";

            if (dataRequest.getIdTypeButtons() == 0)
            {
                buttons = await insertTypeButtons0(instrument, vista);
                return buttons;
            }
            else if (dataRequest.getIdTypeButtons() == 1)
            {
                buttons = await insertTypeButtons1(instrument, vista);
                return buttons;
            }

            return "";
        }
        public async Task<string> generateGraph(Instrument instrument, GenerateGraphDTO dataRequest)
        {
            string archivo = $"GraphType{dataRequest.getIdTypeGraph()}.html";
            //Console.WriteLine("Entró acá.");
            string vista = await instrumentService.selectView(instrument, archivo);
            string graph = "";
            if (dataRequest.getIdTypeGraph() == 0)
            {
                SensorDataDTO graphData = await repositoryBaseMethods.generateDataTypeGraph0(instrument, dataRequest);

                if (dataRequest.getAverageRequired())
                {
                    graphData = await getAverage(graphData);
                }
                graph = await insertGraphDataType0(instrument, graphData, dataRequest, vista);
            }
            else if (dataRequest.getIdTypeGraph() == 1)
            {
                SensorDataDTO graphData = await repositoryBaseMethods.generateDataTypeGraph1(instrument, dataRequest);
                if (dataRequest.getAverageRequired())
                {
                    graphData = await getAverage(graphData);
                }
                graph = await insertGraphDataType1(instrument, graphData, dataRequest, vista);
            }
            return graph;
        }

        public async Task<string> insertGraphDataType1(Instrument instrument, SensorDataDTO sensorData, GenerateGraphDTO dataRequest, string vista)
        {

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var content = await System.IO.File.ReadAllTextAsync(vista);
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
                foreach (var sensor in sensorData.sensores)
                {
                    var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));
                    var chartOptionsSeries = "";
                    if (instrument is GNSS) chartOptionsSeries = $@"{{name: '{(string.IsNullOrEmpty(dataRequest.getNombreSensor()) ? sensor.sensor_name : sensor.instrument_name)}',data: [{sensorEjeYFormatted}]}},";
                    if (instrument is Clinoextensometro) chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                    if (instrument is SensorHumedad) chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                    if (instrument is Prisma) chartOptionsSeries = $@"{{name: '{sensor.instrument_name}',data: [{sensorEjeYFormatted}]}},";
                    if (instrument is Trigger) chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                    if (instrument is Piezometro) chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                    if (instrument is Radar01) chartOptionsSeries = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";

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
                if (instrument is GNSS)
                {
                    chartOptionsTitle = $"{(string.IsNullOrEmpty(dataRequest.getNombreSensor()) ? "Serie de Tiempo " + instrument.getInstrumentName() : "Consolidado " + dataRequest.getNombreSensor())} {(dataRequest.getAverageRequired() ? "Promedios" : "")}";
                }
                if (instrument is Piezometro)
                {
                    chartOptionsTitle = $"Serie de Tiempo {instrument.getInstrumentName()}";
                    chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                    chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                }
                if (instrument is SensorHumedad)
                {
                    chartOptionsTitle = $"Serie de Tiempo {instrument.getInstrumentName()}";
                }
                if (instrument is Clinoextensometro)
                {
                    chartOptionsTitle = $"{dataRequest.getNombreVariable()} {instrument.getInstrumentName()}";
                    chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                    chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                }
                if (instrument is Trigger)
                {
                    chartOptionsTitle = $"Serie de Tiempo {instrument.getInstrumentName()}";
                }
                if (instrument is Prisma)
                {
                    chartOptionsTitle = $"Consolidado {(dataRequest.getAverageRequired() ? "Promedios" : "")}";
                }
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
                return content;
            }
            return content;

        }
        public async Task<string> insertGraphDataType0(Instrument instrument, SensorDataDTO sensorData, GenerateGraphDTO generateGraphDTO, string vista)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var content = await System.IO.File.ReadAllTextAsync(vista);
            Sensor sensor = sensorData.sensores[0];
            var chartOptionsTitle = "";
            var chartOptionsName = "";
            if (instrument is GNSS)
            {
                chartOptionsTitle = $"Consolidado {generateGraphDTO.getNombreSensor()} {((generateGraphDTO.getAverageRequired()) ? "Promedios" : "")}";
            }
            if (instrument is Piezometro)
            {
                chartOptionsTitle = $"{instrument.getInstrumentName()} ({generateGraphDTO.getNombreSensor()})";
                chartOptionsTitle = chartOptionsTitle.Replace("_", "");
                chartOptionsTitle = chartOptionsTitle.Replace("-", "");
                chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                chartOptionsName = sensor.sensor_name;
            }
            if (instrument is SensorHumedad)
            {
                sensorData.sensores.FirstOrDefault(t => t.sensor_id == generateGraphDTO.getIdSensor());
                chartOptionsTitle = $"{instrument.getInstrumentName()} ({generateGraphDTO.getNombreSensor()})";
            }
            if (instrument is Clinoextensometro)
            {
                chartOptionsTitle = $"{generateGraphDTO.getNombreVariable()} {instrument.getInstrumentName()}";
                chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
            }
            if (instrument is Trigger)
            {
                chartOptionsTitle = $"{instrument.getInstrumentName()} ({generateGraphDTO.getNombreSensor()})";
                chartOptionsName = sensor.sensor_name;
            }
            if (instrument is Prisma)
            {
                chartOptionsTitle = $"{sensor.sensor_name} {instrument.getInstrumentName()}";
                chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                chartOptionsName = sensor.sensor_name;
            }
            if (instrument is Radar01)
            {
                chartOptionsTitle = $"{sensor.sensor_name}";
                // chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                chartOptionsName = sensor.sensor_name;
                if (generateGraphDTO.getAverageRequired())
                {
                    content = content.Replace("[[CHECK_SELECT]]", "");
                    content = content.Replace("[[END_CHECK_SELECT]]", "");
                    content = content.Replace("[[CHECK_SCRIPT_SELECT]]", "");
                    content = content.Replace("[[END_CHECK_SCRIPT_SELECT]]", "");
                    //
                    content = content.Replace("{{idInstrumento}}", generateGraphDTO.getIdInstrumento().ToString());
                    content = content.Replace("{{idTypeGraph}}", generateGraphDTO.getIdTypeGraph().ToString());
                    content = content.Replace("{{averageRequired}}", generateGraphDTO.getAverageRequired().ToString().ToLower());
                    //content = content.Replace("{{nombreVariable}}", "");
                    content = content.Replace("{{nombreSensor}}", generateGraphDTO.getNombreSensor().ToString());
                    // content = content.Replace("{{idSensor}}", "");
                    content = content.Replace("{{reDrawGraph}}", "true");
                    //   content = content.Replace("{{reDrawSelect}}", "");
                    //  content = content.Replace("{{reDrawSelect}}", "");

                }
                else
                {
                    var startTag = "[[CHECK_SELECT]]";
                    var endTag = "[[END_CHECK_SELECT]]";
                    var startIndex = content.IndexOf(startTag);
                    var endIndex = content.IndexOf(endTag) + endTag.Length;
                    string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);


                    var startTagScript = "[[CHECK_SCRIPT_SELECT]]";
                    var endTagScript = "[[END_CHECK_SCRIPT_SELECT]]";
                    var startScriptIndex = content.IndexOf(startTagScript);
                    var endScriptIndex = content.IndexOf(endTagScript) + endTagScript.Length;

                    string extractedScriptContent = content.Substring(startScriptIndex + startTagScript.Length, endScriptIndex - startScriptIndex - startTagScript.Length - endTagScript.Length);

                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        content = content.Replace(extractedContent, "");
                        content = content.Replace("[[CHECK_SELECT]]", "");
                        content = content.Replace("[[END_CHECK_SELECT]]", "");
                    }

                    if (startScriptIndex >= 0 && endScriptIndex >= 0)
                    {
                        content = content.Replace(extractedScriptContent, "");
                        content = content.Replace("[[CHECK_SCRIPT_SELECT]]", "");
                        content = content.Replace("[[END_CHECK_SCRIPT_SELECT]]", "");
                    }
                }
            }
            if (instrument is InSAR)
            {
                chartOptionsTitle = $"{sensor.sensor_name} {instrument.getInstrumentName()}";
                chartOptionsTitle = chartOptionsTitle.Replace("_", " ");
                chartOptionsTitle = textInfo.ToTitleCase(chartOptionsTitle);
                chartOptionsName = sensor.sensor_name;
            }


            var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));
            content = content.Replace("{sensor.sensor_name}", sensor.sensor_name);
            content = content.Replace("{{sensor.sensor_eje_x}}", "[" + string.Join(",", sensor.sensor_eje_x.Select(date => $"'{date}'")) + "]");
            content = content.Replace("{{sensor.sensor_eje_y}}", "[" + sensorEjeYFormatted + "]");
            content = content.Replace("{{name}}", chartOptionsName);

            content = content.Replace("{{title}}", chartOptionsTitle);
            return content;
        }
        public async Task<string> insertTypeButtons0(Instrument instrument, string vista)
        {
            if (instrument is GNSS)
            {
            }
            if (instrument is Piezometro)
            {
            }
            if (instrument is SensorHumedad)
            {
            }
            if (instrument is Clinoextensometro)
            {

            }
            if (instrument is Trigger)
            {
            }
            if (instrument is Prisma)
            {
            }

            var content = await System.IO.File.ReadAllTextAsync(vista);
            content = content.Replace("{{idInstrumento}}", instrument.Id.ToString());
            return content;
        }
        public async Task<string> insertTypeButtons1(Instrument instrument, string vista)
        {
            var content = await System.IO.File.ReadAllTextAsync(vista);
            var startTag = "[[INSERTAR_BOTONES]]";
            var endTag = "[[FIN_INSERTAR_BOTONES]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;

            var startContent = content.Substring(0, startIndex);
            var endContent = content.Substring(endIndex);
            var sensorTemplate = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
            var iteratedContent = new StringBuilder();
            foreach (var sensor in instrument.Sensors)
            {
                var replacedContent = sensorTemplate.Replace("{{sensor.sensor_name}}", sensor.sensor_name);
                replacedContent = replacedContent.Replace("{{sensor.sensor_id}}", sensor.sensor_id.ToString());
                replacedContent = replacedContent.Replace("{{idInstrumento}}", instrument.Id.ToString());
                iteratedContent.Append(replacedContent);
            }
            content = startContent + iteratedContent.ToString() + endContent;
            content = content.Replace("[[INSERTAR_BOTONES]]", "");
            content = content.Replace("[[FIN_INSERTAR_BOTONES]]", "");
            content = content.Replace("{{idInstrumento}}", instrument.Id.ToString());
            return content;
        }


        public async Task<Sensor> findChartOptionsCategories(SensorDataDTO sensorCategories)
        {
            int lengthMayor = sensorCategories.sensores.Max(sensor => sensor.sensor_eje_x.Count());
            Sensor sensorMayor = sensorCategories.sensores.FirstOrDefault(sensor => sensor.sensor_eje_x.Count() == lengthMayor);
            return sensorMayor;
        }
        public async Task<Sensor> findChartOptionsCategoriesConsolidado(ConsolidadoAOIDTO instrumentsCategories)
        {
            Sensor sensorMayor = null;
            int lengthMayor = 0;

            foreach (var iteradorInstrumentos in instrumentsCategories.instrumentos)
            {
                foreach (var iteradorSensores in iteradorInstrumentos.sensores)
                {
                    int sensorEjeXCount = iteradorSensores.sensor_eje_x.Count();

                    // Verificar si este sensor tiene más elementos en el eje X que el anterior máximo.
                    if (sensorEjeXCount > lengthMayor)
                    {
                        lengthMayor = sensorEjeXCount;
                        sensorMayor = iteradorSensores;
                    }
                }
            }
            return sensorMayor;
        }

        public async Task<SensorDataDTO> getAverage(SensorDataDTO graphData)
        {
            SensorDataDTO graphDataPromedios = new SensorDataDTO();
            graphDataPromedios.sensores = new List<Sensor>();

            foreach (var sensor in graphData.sensores)
            {
                // Agrupar los valores del sensor por fecha
                var valoresPorFecha = new Dictionary<string, List<double?>>();
                for (int i = 0; i < sensor.sensor_eje_x.Count; i++)
                {
                    string fecha = sensor.sensor_eje_x[i];
                    double? valor = sensor.sensor_eje_y[i];

                    if (valoresPorFecha.ContainsKey(fecha))
                    {
                        valoresPorFecha[fecha].Add(valor);
                    }
                    else
                    {
                        valoresPorFecha.Add(fecha, new List<double?> { valor });
                    }
                }

                // Calcular los promedios por fecha para el sensor actual
                var promediosEjeY = new List<double?>();
                var fechas = new List<string>();
                foreach (var kvp in valoresPorFecha)
                {
                    double? sum = 0;
                    int count = 0;
                    foreach (var valor in kvp.Value)
                    {
                        if (valor.HasValue)
                        {
                            sum += valor;
                            count++;
                        }
                    }
                    double? promedio = count > 0 ? sum / count : (double?)null;
                    promediosEjeY.Add(promedio);
                    fechas.Add(kvp.Key);
                }

                // Agregar los promedios por fecha al nuevo sensor en sensorDataPromedios
                graphDataPromedios.sensores.Add(new Sensor
                {
                    sensor_id = sensor.sensor_id,
                    sensor_name = sensor.sensor_name,
                    instrument_name = sensor.instrument_name,
                    sensor_eje_y = promediosEjeY,
                    sensor_eje_x = fechas.Select(date => date).ToList()
                });
            }

            return graphDataPromedios;
        }

        public async Task<GenerateLogBookDTO> findLogBookByInstrumentId(GenerateLogBookDTO generateLogBookDTO)
        {
            generateLogBookDTO = await repositoryBaseMethods.findLogBookByInstrumentId(generateLogBookDTO);
            return generateLogBookDTO;
        }

        public async Task<string> generateLogBook(GenerateLogBookDTO generateLogBookDTO)
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
            /*


            */
            content = content.Replace("{{idInstrumento}}", generateLogBookDTO.IdInstrumento.ToString());

            return content;
        }
        public async Task<IActionResult> SaveLog(GenerateLogBookDTO generateLogBookDTO)
        {
            IActionResult statusCodeResult = await repositoryBaseMethods.SaveLog(generateLogBookDTO);
            return statusCodeResult;

          
        }

    }
}
