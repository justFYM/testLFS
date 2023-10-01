using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TerriaMVC.DTO;

namespace TerriaMVC.Services
{
    public interface IViewService
    {
      //  Task<string> GetView(SensorDataDTO sensorData, int idServicio, int idInstrumento, int idSensor, string nombreTipoInstrumento, string? nombreSensor, string? nombreVariable);
    }

    public class ViewService : IViewService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ViewService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        /*
        public async Task<string> GetView(SensorDataDTO sensorData, int idServicio, int idInstrumento, int idSensor, string nombreTipoInstrumento, string? nombreSensor, string? nombreVariable)
        {
            var sensor_name = sensorData.sensores[0].sensor_name;
            if (idServicio == 1)
            {
                var filePath = "";
                if (nombreTipoInstrumento.Equals("Piezometro"))
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Piezometro", "Buttons.html");
                }
                else if (nombreTipoInstrumento.Equals("Sensor_Humedad"))
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Sensor_Humedad", "Buttons.html");
                }
                else if (nombreTipoInstrumento.Equals("Prisma"))
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Prisma", "Buttons.html");
                }
                else if (nombreTipoInstrumento.Equals("GNSS"))
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "GNSS", "Buttons.html");
                }
                else if (nombreTipoInstrumento.Equals("Clinoextensometro"))
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Clinoextensómetro", "Buttons.html");
                }
                else
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Buttons.html");
                }













                if (System.IO.File.Exists(filePath))
                {
                    var content = await System.IO.File.ReadAllTextAsync(filePath);
                    if (nombreTipoInstrumento.Equals("Prisma")){
                        content = content.Replace("{{idInstrumento}}", idInstrumento.ToString());
                        content = content.Replace("{{nombreTipoInstrumento}}", nombreTipoInstrumento);
                        content= content.Replace("{{sensor.sensor_name}}", sensorData.sensores[0].sensor_name);
                        content= content.Replace("{{sensor.sensor_id}}", sensorData.sensores[0].sensor_id.ToString());
                        return content;
                    }
                    else
                    {
                        
                        var startTag = "[[ITERAR]]";
                        var endTag = "[[FIN_ITERAR]]";

                        var startIndex = content.IndexOf(startTag);
                        var endIndex = content.IndexOf(endTag) + endTag.Length;

                        if (startIndex >= 0 && endIndex >= 0)
                        {
                            var startContent = content.Substring(0, startIndex);
                            var endContent = content.Substring(endIndex);

                            // Se obtiene la porción de código para modificar.
                            var sensorTemplate = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
                            Console.WriteLine(sensorTemplate);
                            var iteratedContent = new StringBuilder();

                            foreach (var sensor in sensorData.sensores)
                            {
                                // Reemplaza {sensor.sensor_name} con el nombre del sensor actual
                                var replacedContent = sensorTemplate.Replace("{{sensor.sensor_name}}", sensor.sensor_name);

                                // Reemplaza {sensor.sensor_id} con el id del sensor actual
                                replacedContent = replacedContent.Replace("{{sensor.sensor_id}}", sensor.sensor_id.ToString());

                                // Reemplaza {idInstrumento} con el id del instrumento actual
                                replacedContent = replacedContent.Replace("{{idInstrumento}}", idInstrumento.ToString());
                                replacedContent = replacedContent.Replace("{{nombreTipoInstrumento}}", nombreTipoInstrumento);

                                // Se agrega "cada botón" a una variable StringBuilder();
                                iteratedContent.Append(replacedContent);
                                // Console.WriteLine(replacedContent);
                            }

                            // Junta los "botónes totales"
                            content = startContent + iteratedContent.ToString() + endContent;
                            Console.WriteLine(content);
                        }


                        // Elimina el identificador [ITERAR] o [FIN_ITERAR] que pueda quedar en el contenido
                        content = content.Replace("[[ITERAR]]", "");
                        content = content.Replace("[[FIN_ITERAR]]", "");

                        if (content.Contains("{{idInstrumento}}"))
                        {
                            content = content.Replace("{{idInstrumento}}", idInstrumento.ToString());
                        }
                        if (content.Contains("{{nombreTipoInstrumento}}"))
                        {
                            content = content.Replace("{{nombreTipoInstrumento}}", nombreTipoInstrumento);
                        }
                       


                        return content;
                    }
                }
                  
                else
                {
                    // El archivo no existe
                    return "Archivo no encontrado";
                }
            }
            if (idServicio == 2)
            {
                var filePath = "";


                if (nombreTipoInstrumento.Equals("Piezometro") && idSensor < 0)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Piezometro", "Total_Sensors_Graph.html");
                }else if (nombreTipoInstrumento.Equals("Sensor_Humedad") && idSensor < 0)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Sensor_Humedad", "Total_Sensors_Graph.html");
                }
                else if (nombreTipoInstrumento.Equals("Prisma") && idSensor == -1)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Prisma", "Consolidado.html");
                }
                else if (nombreTipoInstrumento.Equals("Prisma") && idSensor == -2)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Prisma", "ConsolidadoPromedios.html");
                }
                else if (nombreTipoInstrumento.Equals("GNSS") && idSensor == -1)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "GNSS", "Total_Sensors_Graph.html");
                }
                else if (nombreTipoInstrumento.Equals("GNSS") && idSensor == -2)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "GNSS", "ConsolidadoGNSS.html");
                   

                }
                else if (nombreTipoInstrumento.Equals("GNSS") && idSensor == -3)
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "GNSS", "ConsolidadoPromediosGNSS.html");
                    Console.WriteLine("Con");

                }
                else if (nombreTipoInstrumento.Equals("Clinoextensometro") )
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Clinoextensómetro", "Graph.html");
                    Console.WriteLine("Con");
                }
                
                else
                {
                    filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "Graph.html");
                }


                //Acceder a Prisma/Consolidado.html
                int fileNameConsolidado = filePath.IndexOf("Consolidado", StringComparison.OrdinalIgnoreCase);
                int fileNameConsolidadoPromedios = filePath.IndexOf("ConsolidadoPromedios", StringComparison.OrdinalIgnoreCase);
                int fileNameConsolidadoGNSS = filePath.IndexOf("ConsolidadoGNSS", StringComparison.OrdinalIgnoreCase);
                int fileNameConsolidadoPromediosGNSS = filePath.IndexOf("ConsolidadoPromediosGNSS", StringComparison.OrdinalIgnoreCase);

                if (fileNameConsolidadoGNSS != -1)
                {
                    Console.WriteLine("¡Consolidado Altura!");

                }
             
                if (System.IO.File.Exists(filePath))
                {
                    var content = await System.IO.File.ReadAllTextAsync(filePath);
                    if (idSensor > 0)
                    {

                    var sensor = sensorData.sensores.FirstOrDefault(t => t.sensor_id == idSensor);
                    if (sensor != null)
                    {
                        // Se cambia la coma "," por el punto "." en los valores de sensor.sensor_eje_y
                        var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));

                            content = content.Replace("{sensor.sensor_name}", sensor.sensor_name);

                            // Se cambia "{eje_x}" con los valores de sensor.sensor_eje_x como un array JSON
                            content = content.Replace("{eje_x}", "[" + string.Join(",", sensor.sensor_eje_x.Select(date => $"'{date}'")) + "]");

                        // Se cambia "{eje_y}" con los valores de sensor.sensor_eje_y (double) como un array JSON con punto en lugar de coma (verificando si son "null")
                        content = content.Replace("{eje_y}", "[" + sensorEjeYFormatted + "]");

                        return content;
                    }

                    }
                    else
                    {
                        var startTag = "[[ITERAR]]";
                        var endTag = "[[FIN_ITERAR]]";

                        var startIndex = content.IndexOf(startTag);
                        var endIndex = content.IndexOf(endTag) + endTag.Length;
                        if (nombreTipoInstrumento.Equals("Prisma"))
                        {
                            using (var httpClient = new HttpClient())
                            {
                                //Envía la id del instrumento
                                string url = $"http://localhost:7294/test/{idInstrumento}";
                                HttpResponseMessage response = await httpClient.GetAsync(url);
                                if (response.IsSuccessStatusCode)
                                {
                                    string jsonString = await response.Content.ReadAsStringAsync();
                                    sensorData = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);

                                    // Calcula los promedios por día para cada sensor
                                    if(fileNameConsolidadoPromedios != -1)
                                    {
                                        SensorDataDTO sensorDataPromedios = CalcularPromedioPorDia(sensorData);
                                        sensorData = sensorDataPromedios;
                                    }
                                  
                                       
                                 
                                   


                                }
                                else
                                {
                                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                                    // ...
                                }
                            }

                            
                        }else if (nombreTipoInstrumento.Equals("GNSS"))
                        {
                            Console.WriteLine("Nombre de sensor: 'Valor Altura'");
                            using (var httpClient = new HttpClient())
                            {
                                //Envía la id del instrumento
                                //string sensor = "Valor Altura";
                                string url = $"http://localhost:7294/test2/{idInstrumento},{nombreSensor}";
                                HttpResponseMessage response = await httpClient.GetAsync(url);
                                if (response.IsSuccessStatusCode)
                                {
                                    string jsonString = await response.Content.ReadAsStringAsync();
                                    sensorData = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);

                                    // Calcula los promedios por día para cada sensor
                                    
                                    if (fileNameConsolidadoPromediosGNSS != -1)
                                    {
                                        SensorDataDTO sensorDataPromedios = CalcularPromedioPorDia(sensorData);
                                        sensorData = sensorDataPromedios;
                                    }
                                    






                                }
                                else
                                {
                                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                                    // ...
                                }
                            }
                        }
                        else if (nombreTipoInstrumento.Equals("Clinoextensometro"))
                        {
                            Console.WriteLine("Nombre de variable:"+ nombreVariable);
                            using (var httpClient = new HttpClient())
                            {
                                //Envía la id del instrumento
                                //string sensor = "Valor Altura";
                                string url = $"http://localhost:7294/clino/{idInstrumento},{nombreVariable}";
                                HttpResponseMessage response = await httpClient.GetAsync(url);
                                if (response.IsSuccessStatusCode)
                                {
                                    string jsonString = await response.Content.ReadAsStringAsync();
                                    sensorData = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);

                                    // Calcula los promedios por día para cada sensor







                                }
                                else
                                {
                                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                                    // ...
                                }
                            }
                        }



                        Console.WriteLine("Entró acá");
               

                        if (startIndex >= 0 && endIndex >= 0)
                        {
                            var startContent = content.Substring(0, startIndex);
                            var endContent = content.Substring(endIndex);

                            var iteratedContent = new StringBuilder();
                            Sensor sensorMayor = new Sensor();
                            foreach (var sensor in sensorData.sensores)
                            {
                             
                                var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));
                                
                                var sensorDataString = "";
                                if (fileNameConsolidado != -1)
                                {
                                    // Obtenemos la cantidad mayor de elementos entre todas las propiedades sensor_eje_x de los sensores
                                    int lengthMayor = sensorData.sensores.Max(sensor => sensor.sensor_eje_x.Count());

                                    // Obtenemos el sensor que tiene la mayor cantidad de elementos en sensor_eje_x
                                    sensorMayor = sensorData.sensores.FirstOrDefault(sensor => sensor.sensor_eje_x.Count() == lengthMayor);
                                    
                                    sensorDataString = $@"{{name: '{sensor.instrument_name}',data: [{sensorEjeYFormatted}]}},";
                                }
                                else
                                {
                                    sensorDataString = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";
                                }

                                iteratedContent.AppendLine(sensorDataString);
                            }
                           
                            // Reemplaza la sección [[ITERAR]] y [[FIN_ITERAR]] con el contenido iterado para cada sensor
                            content = startContent + iteratedContent.ToString() + endContent;
                            if (fileNameConsolidado != -1)
                            {
                                content = content.Replace("{{sensor.sensor_eje_x}}", $"{string.Join(",", sensorMayor.sensor_eje_x.Select(date => $"'{date}'"))}");
                            }
                            else
                            {
                                content = content.Replace("{{sensor.sensor_eje_x}}", $"{string.Join(",", sensorData.sensores[0].sensor_eje_x.Select(date => $"'{date}'"))}");
                            }
                            content = content.Replace("{{nombreTipoInstrumento}}", nombreTipoInstrumento);
                            content = content.Replace("{{nombreSensor}}", nombreSensor);
                            if (nombreVariable.Equals("tasa_mensual_de_asentamiento"))
                            {
                                content = content.Replace("{{nombreVariable}}", "Tasa Mensual de Asentamiento");
                            } else if (nombreVariable.Equals("tasa_semanal_de_asentamiento"))
                            {
                                content = content.Replace("{{nombreVariable}}", "Tasa Semanal de Asentamiento"); 
                            }
                            else if (nombreVariable.Equals("deformación_acumulada_vertical"))
                            {
                                content = content.Replace("{{nombreVariable}}", "Deformación Acumulada Vertical");
                            }

                                // Console.WriteLine(content);
                                return content;
                        }
                    }
                }

                return "";
            }
            else
            {
                return "Otro contenido";
            }
        }


        // Método para calcular los promedios por día para cada sensor
        private SensorDataDTO CalcularPromedioPorDia(SensorDataDTO sensorData)
        {
            SensorDataDTO sensorDataPromedios = new SensorDataDTO();
            sensorDataPromedios.sensores = new List<Sensor>();

            foreach (var sensor in sensorData.sensores)
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
                sensorDataPromedios.sensores.Add(new Sensor
                {
                    sensor_id = sensor.sensor_id,
                    sensor_name = sensor.sensor_name,
                    instrument_name = sensor.instrument_name,
                    sensor_eje_y = promediosEjeY,
                    sensor_eje_x = fechas.Select(date => date).ToList()
                });
            }

            return sensorDataPromedios;
        }

        */






    }
  


}
