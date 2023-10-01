using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TerriaMVC.DTO;

namespace TerriaMVC.Services
{
    public interface IInstrumentService_
    {
        Task<(SensorDataDTO, string)> GetSensorData(int id);
    }
    

    public class InstrumentService_ : IInstrumentService_
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InstrumentService_(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(SensorDataDTO, string)> GetSensorData(int id)
        {
            var url = $"http://localhost:7294/clinoextensometro/"+id;
            //var url = "http://localhost:7294/WeatherForecast/1";
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    SensorDataDTO sensorData = await DeserializateJSON(responseData);
                    return (sensorData, responseData);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {

                    return (null, null);
                }
                else
                {

                    return (null, null);
                }
            }
            catch (Exception ex)
            {

                return (null, null);
            }
        }
        public async Task<SensorDataDTO> DeserializateJSON(string response)
        {

            try
            {
                SensorDataDTO sensorData = JsonConvert.DeserializeObject<SensorDataDTO>(response);


                if (sensorData != null && sensorData.sensores != null && sensorData.sensores.Count > 0)
                {
                    //Sensor primerSensor = sensorData.sensores[0];
                    foreach (var sensor in sensorData.sensores)
                    {
                        // Sensor primerSensor = response.sensores[0];
                       
                        Console.WriteLine("Datos del sensor:");
                        Console.WriteLine("Sensor ID: " + sensor.sensor_id);
                        Console.WriteLine("Sensor Name: " + sensor.sensor_name);
                     
                        //Console.WriteLine(sensor);zz
                    }


                    //Console.WriteLine("Sensor Eje Y:");
                    /*
                    foreach (var sensor in sensorData.sensores)
                    {
                        Console.WriteLine(sensor.sensor_eje_x);
                    }
                    */
                    /*
                    Console.WriteLine("Sensor Eje X:");
                    foreach (var valor in primerSensor.sensor_eje_x)
                    {
                        Console.WriteLine(valor);
                    }
                    */

                }
                return sensorData;
            }
            catch (Exception ex)
            {

                return null;
            }
        }





    }

}
