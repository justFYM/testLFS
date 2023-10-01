using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Services;

namespace TerriaMVC.Repository
{
    public interface IRepositoryBaseMethods
    {
        Task<GenerateLogBookDTO> findLogBookByInstrumentId(GenerateLogBookDTO generateLogBookDTO);
        Task<SensorDataDTO> generateDataTypeGraph0(Instrument instrument, GenerateGraphDTO generateGraphDTO);
        Task<SensorDataDTO> generateDataTypeGraph1(Instrument instrument, GenerateGraphDTO generateGraphDTO);
        Task<IActionResult> SaveLog(GenerateLogBookDTO generateLogBookDTO);
    }
    public  class RepositoryBaseMethods : IRepositoryBaseMethods
    {
       // public async Task<string> getButtonsData(Instrument instrument);
        public async Task<SensorDataDTO> generateDataTypeGraph0(Instrument instrument, GenerateGraphDTO generateGraphDTO)
        {
            using (var httpClient = new HttpClient())
            {
                string url = "";
                if (instrument is GNSS)
                {
                     url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}";

                }
                if (instrument is Piezometro)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getIdSensor()}";

                }
                if (instrument is SensorHumedad)
                {
                     url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getIdSensor()}";

                }
                if (instrument is Clinoextensometro)
                {
                     url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";

                }
                if (instrument is Trigger)
                {
                    if (generateGraphDTO.getReDrawGraph())
                    {

                    }
                    else
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getIdSensor()}";

                    }

                }
                if (instrument is Prisma)
                {
                     url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}";

                }
                if (instrument is Radar01)
                {
                    if (generateGraphDTO.getHourToAverage() != -1)
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/ByHour/{instrument.getId()},{generateGraphDTO.getNombreSensor()},{generateGraphDTO.getHourToAverage()}";

                    }
                    else
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getNombreSensor()}";

                    }

                }
                if (instrument is InSAR)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getNombreSensor()}";

                }
                //Envía la id del instrumento
                Console.WriteLine("Preparando la solicitud....");
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    SensorDataDTO sensorDataDTO = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);
                    Console.WriteLine(url+" | Sensores recibidos: " + sensorDataDTO.sensores.Count());
                    return sensorDataDTO;
                }
                else
                {
                    return (new SensorDataDTO());
                }
            }
        }

        public async Task<SensorDataDTO> generateDataTypeGraph1(Instrument instrument, GenerateGraphDTO generateGraphDTO)
        {
            using (var httpClient = new HttpClient())
            {
                string url = "";
                if (instrument is GNSS)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}{(string.IsNullOrEmpty(generateGraphDTO.getNombreSensor()) ? "" : "," + generateGraphDTO.getNombreSensor())}";
                }
                if (instrument is Piezometro)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}";
                }
                if (instrument is SensorHumedad)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}";

                }
                if (instrument is Clinoextensometro)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";
                }
                if (instrument is Trigger)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}";
                }
                if (instrument is Prisma)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()}";
                }
                if (instrument is Radar01)
                {
                    if (generateGraphDTO.getHourToAverage() != -1)
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/ByHour/{instrument.getId()},{generateGraphDTO.getNombreSensor()},{generateGraphDTO.getHourToAverage()}";

                    }
                    else
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{instrument.getId()},{generateGraphDTO.getNombreSensor()}";

                    }
                    

                }
                Console.WriteLine("Preparando la solicitud....");
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    SensorDataDTO sensorDataDTO = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);
                    Console.WriteLine(url + " | Sensores recibidos: " + sensorDataDTO.sensores.Count());

                    return sensorDataDTO;
                }
                else
                {
                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                    // ...
                    return (new SensorDataDTO());
                }
            }
        }
        public async Task<SensorDataDTO> generateDataTypeButtons1(Instrument instrument, GenerateGraphDTO generateGraphDTO)
        {
            using (var httpClient = new HttpClient())
            {
                string url = "";
                if (instrument is GNSS)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreSensor()}";

                }
                if (instrument is Piezometro)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";

                }
                if (instrument is SensorHumedad)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";

                }
                if (instrument is Clinoextensometro)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";

                }
                if (instrument is Trigger)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";

                }
                if (instrument is Prisma)
                {
                    url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/{instrument.getId()},{generateGraphDTO.getNombreVariable()}";

                }
                //Envía la id del instrumento


                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    SensorDataDTO sensorDataDTO = JsonConvert.DeserializeObject<SensorDataDTO>(jsonString);
                    Console.WriteLine("Preparando la solicitud");
                    Console.WriteLine("Cantidad de sensores: " + sensorDataDTO.sensores.Count());

                    return sensorDataDTO;
                }
                else
                {
                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                    // ...
                    return (new SensorDataDTO());
                }
            }
        }
        public async Task<GenerateLogBookDTO> findLogBookByInstrumentId(GenerateLogBookDTO generateLogBookDTO)
        {
            using (var httpClient = new HttpClient())
            {
                //Envía la id del instrumento
                string url = $"http://localhost:7294/api/LogBook/{generateLogBookDTO.getIdInstrumento()}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    generateLogBookDTO = JsonConvert.DeserializeObject<GenerateLogBookDTO>(jsonString);
                    Console.WriteLine("Preparando la solicitud");
                 //   Console.WriteLine("Cantidad de sensores: " + sensorDataDTO.sensores.Count());

                    return generateLogBookDTO;
                }
                else
                {
                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                    // ...
                    return (new GenerateLogBookDTO());
                }
            }
        }

        public async Task<IActionResult> SaveLog(GenerateLogBookDTO generateLogBookDTO)
        {
            using (var httpClient = new HttpClient())
            {
                // Construir el objeto que se enviará en la solicitud POST
                var data = new
                {
                    IdSensor = (int)generateLogBookDTO.IdSensor,
                    Note = (string)generateLogBookDTO.Note,
                    DateTimeString = (string)generateLogBookDTO.DateTimeString
                };

                string url = "http://localhost:7294/api/LogBook/SaveLog";
                // Serializar el objeto 'data' a JSON
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // La inserción fue exitosa, devolver un código 200
                    return new StatusCodeResult(200);
                }
                else
                {
                    // No se pudo realizar la inserción, devolver el código de error correspondiente
                    return new StatusCodeResult((int)response.StatusCode);
                }
            }
        }

    }
}
