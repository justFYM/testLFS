using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public class TriggerService : ServiceBaseMethods
    {
        private readonly IInstrumentService instrumentService;
        private readonly TriggerRepository triggerRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TriggerService(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, TriggerRepository triggerRepository, IWebHostEnvironment webHostEnvironment) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
            this.instrumentService = instrumentService;
            this.triggerRepository = triggerRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> testInstrument()
        {
            var vista = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Trigger", "test2.html");
            var content = await System.IO.File.ReadAllTextAsync(vista);
            return content;
        }

        public async Task<string> generateGraphTrigger(Instrument instrument, GenerateGraphDTO dataRequest)
        {
            string archivo = $"GraphType{dataRequest.getIdTypeGraph()}.html";
            var vista = await instrumentService.selectView(instrument, archivo);
            if (dataRequest.getReDrawGraph())
            {
                TriggerDTO triggerDTO = await triggerRepository.TriggerTypeGraph1(instrument, dataRequest);
                Console.WriteLine("Cantidad de sensores: " + triggerDTO.sensores.Count());

                var content = await System.IO.File.ReadAllTextAsync(vista);
                var startTag = "[[CHECK]]";
                var endTag = "[[END_CHECK]]";
                var startIndex = content.IndexOf(startTag);
                var endIndex = content.IndexOf(endTag) + endTag.Length;
                string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);

                if (startIndex >= 0 && endIndex >= 0)
                {
                    Console.WriteLine("Contenido extraído:");
                    Console.WriteLine(extractedContent);
                }



                /**/
                // Generar contenido dinámico para yAxis
                int[] medidas;
                if (triggerDTO.sensores.Count() == 3)
                {
                    medidas = new int[] { 0, 35, 70 };
                }
                else if (triggerDTO.sensores.Count() == 2)
                {
                    medidas = new int[] { 0, 70 };
                }
                else
                {
                    // Handle other cases if needed
                    medidas = new int[] { 0 };
                }

                StringBuilder yAxisBuilder = new StringBuilder();
                for (int i = 0; i < triggerDTO.sensores.Count(); i++)
                {
                    yAxisBuilder.AppendLine($@"
        {{
            title: {{
                text: '{triggerDTO.sensores[i].sensor_name}'
            }},
            top: '{medidas[i]}%',
            height: '30%',
            offset: 0
        }},");
                }
                string yAxisContent = yAxisBuilder.ToString();
                extractedContent = extractedContent.Replace("[[ITERAR_yAxis]]", yAxisContent);
                extractedContent = extractedContent.Replace("[[FIN_ITERAR_yAxis]]", "");  // Eliminar contenido original



                // Generar contenido dinámico para series
                StringBuilder seriesBuilder = new StringBuilder();
                foreach (var sensor in triggerDTO.sensores)
                {
                    string dataY = string.Join(",", sensor.sensor_eje_y.Select(y => y?.ToString().Replace(',', '.') ?? "null"));

                    seriesBuilder.AppendLine(@"
                        {
                            name: '" + sensor.sensor_name + @"',
                            data: [" + string.Join(",", dataY) + @"],
                            yAxis: " + triggerDTO.sensores.IndexOf(sensor) + @",
                        },");
                }
                string seriesContent = seriesBuilder.ToString();
                extractedContent = extractedContent.Replace("[[ITERAR_SERIES]]", seriesContent);
                extractedContent = extractedContent.Replace("[[FIN_ITERAR_SERIES]]", "");  // Eliminar contenido original
                extractedContent = extractedContent.Replace("{{fechasEjeX}}", "[" + string.Join(",", triggerDTO.sensores.Last().sensor_eje_x.Select(x => $"'{x}'")) + "]");
                extractedContent = extractedContent.Replace("{{cantidadDeSensores}}", triggerDTO.sensores.Count().ToString());
                extractedContent = extractedContent.Replace("{{title}}", triggerDTO.selectedDate.ToString() + " " + "[" + triggerDTO.selectedEvent.ToString() + "]");
          
                return extractedContent;
            }
            else
            {
                TriggerDTO triggerDTO = await triggerRepository.TriggerTypeGraph1(instrument, dataRequest);
                Console.WriteLine("Cantidad de sensores: " + triggerDTO.sensores.Count());

                // Console.WriteLine("Evento seleccionado: " + triggerDTO.selectedEvent);
                string fechasCalendarioString = string.Join(",", triggerDTO.fechasCalendario.Select(fecha => $"{fecha}"));
                var content = await System.IO.File.ReadAllTextAsync(vista);
                content = content.Replace("[[CHECK]]", "");
                content = content.Replace("[[END_CHECK]]", "");
                content = content.Replace("{{fechasCalendario}}", fechasCalendarioString);
                string fechaSeleccionadaFormateada = triggerDTO.selectedDate.Replace("-", "/");
                content = content.Replace("{{fechaSeleccionada}}", fechaSeleccionadaFormateada);
                string eventosString = string.Join(",", triggerDTO.eventos.Select(evento => $"{evento}"));
                content = content.Replace("{{eventos}}", eventosString);
                content = content.Replace("{{idInstrumento}}", instrument.getId().ToString());
                string nombresSensores = string.Join("-", triggerDTO.sensores.Select(sensor => sensor.sensor_name));
                content = content.Replace("{{nombreSensores}}", nombresSensores);
                content = content.Replace("{{idSensor}}", triggerDTO.sensores[triggerDTO.sensores.Count() - 1].sensor_id.ToString());

                //


                /**/
                // Generar contenido dinámico para yAxis
                int[] medidas;
                if (triggerDTO.sensores.Count() == 3)
                {
                    medidas = new int[] { 0, 35, 70 };
                }
                else if (triggerDTO.sensores.Count() == 2)
                {
                    medidas = new int[] { 0, 70 };
                }
                else
                {
                    // Handle other cases if needed
                    medidas = new int[] { 0 };
                }

                StringBuilder yAxisBuilder = new StringBuilder();
                for (int i = 0; i < triggerDTO.sensores.Count(); i++)
                {
                    yAxisBuilder.AppendLine($@"
        {{
            title: {{
                text: '{triggerDTO.sensores[i].sensor_name}'
            }},
            top: '{medidas[i]}%',
            height: '30%',
            offset: 0
        }},");
                }
                string yAxisContent = yAxisBuilder.ToString();
                content = content.Replace("[[ITERAR_yAxis]]", yAxisContent);
                content = content.Replace("[[FIN_ITERAR_yAxis]]", "");  // Eliminar contenido original



                // Generar contenido dinámico para series
                StringBuilder seriesBuilder = new StringBuilder();
                foreach (var sensor in triggerDTO.sensores)
                {
                    string dataY = string.Join(",", sensor.sensor_eje_y.Select(y => y?.ToString().Replace(',', '.') ?? "null"));

                    seriesBuilder.AppendLine(@"
                        {
                            name: '" + sensor.sensor_name + @"',
                            data: [" + string.Join(",", dataY) + @"],
                            yAxis: " + triggerDTO.sensores.IndexOf(sensor) + @",
                        },");
                }
                string seriesContent = seriesBuilder.ToString();
                content = content.Replace("[[ITERAR_SERIES]]", seriesContent);
                content = content.Replace("[[FIN_ITERAR_SERIES]]", "");  // Eliminar contenido original
                content = content.Replace("{{fechasEjeX}}", "[" + string.Join(",", triggerDTO.sensores.Last().sensor_eje_x.Select(x => $"'{x}'")) + "]");
                content = content.Replace("{{cantidadDeSensores}}", triggerDTO.sensores.Count().ToString());
                content = content.Replace("{{title}}", triggerDTO.selectedDate.ToString() + " " + "[" + triggerDTO.selectedEvent.ToString() + "]");

                Console.WriteLine(content);
                return content;
                /**/
            }



            return "";


        }
        public async Task<string> generateSelectTrigger(Instrument instrument, GenerateGraphDTO dataRequest)
        {
            //SelectDTO
            TriggerDTO triggerDTO = await triggerRepository.TriggerTypeGraph1(instrument, dataRequest);
            // Suponiendo que triggerDTO.eventos es una lista de strings
            List<string> eventosOrdenados = triggerDTO.eventos.OrderBy(e => e).ToList();

            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendLine("<select style=\"padding: 0.5rem 0.5rem; font-size: 0.875rem;\" id=\"selectFechas\" class=\"form-control text-center\">");
            selectBuilder.AppendLine($"<option value=\"0\" disabled selected>{triggerDTO.eventos.Count()} eventos encontrados</option>");

            foreach (var evento in eventosOrdenados)
            {
                selectBuilder.AppendLine($"<option value=\"{evento}\">{evento}</option>");
            }

            selectBuilder.AppendLine("</select>");
            string select = selectBuilder.ToString();
            return select;

        }

    }

}
