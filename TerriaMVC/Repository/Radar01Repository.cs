using Newtonsoft.Json;
using TerriaMVC.DTO;

namespace TerriaMVC.Repository
{
    public class Radar01Repository : RepositoryBaseMethods
    {


        public async Task<GenerateLogBookDTO> FindLogBookCoordinadorByInstrumentId(GenerateLogBookDTO generateLogBookDTO)
        {
            using (var httpClient = new HttpClient())
            {
                //Envía la id del instrumento
                string url = $"http://localhost:7294/api/LogBook/Coordinador/{generateLogBookDTO.getIdInstrumento()}";
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
        
    }
}
