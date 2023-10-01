using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using TerriaMVC.DTO;
using TerriaMVC.Entities;

namespace TerriaMVC.Repository
{
    public class ClinoextensometroRepository : RepositoryBaseMethods
    {
        public async Task<ConsolidadoAOIDTO> generateDataAOIConsolidado(string identifiers)
        {

            using (var httpClient = new HttpClient())
            {
                string url = $"http://localhost:7294/api/Clinoextensometro/TypeGraph1/Consolidado/{identifiers}";
                Console.WriteLine("Preparando la solicitud....");
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    ConsolidadoAOIDTO instrumentsData = JsonConvert.DeserializeObject<ConsolidadoAOIDTO>(jsonString);
                    Console.WriteLine(url + " | Sensores recibidos: " + instrumentsData);
                    return instrumentsData;
                }
                else
                {
                    return (new ConsolidadoAOIDTO());
                }
            }

        }
    }
    
    
}
