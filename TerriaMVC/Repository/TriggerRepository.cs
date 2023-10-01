using Newtonsoft.Json;
using TerriaMVC.DTO;
using TerriaMVC.Entities;

namespace TerriaMVC.Repository
{
    public class TriggerRepository : RepositoryBaseMethods
    {

        public async Task<TriggerDTO> TriggerTypeGraph1(Instrument instrument, GenerateGraphDTO generateGraphDTO)
        {
            using (var httpClient = new HttpClient())
            {
                string url = "";

                if (instrument is Trigger)
                {
                    if (generateGraphDTO.getReDrawGraph())
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/ReDrawGraph{generateGraphDTO.getReDrawGraph()}/{generateGraphDTO.getNombreSensor()},{generateGraphDTO.getIdInstrumento()},{generateGraphDTO.getFechaCalendario()}";
                    }
                    else if (generateGraphDTO.getReDrawSelect())
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/Select{generateGraphDTO.getReDrawSelect()}/{generateGraphDTO.getIdSensor()},{generateGraphDTO.getFechaCalendario()}";

                    }
                    else
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{generateGraphDTO.getNombreSensor()},{generateGraphDTO.getIdInstrumento()}";
                    }


                }

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    TriggerDTO triggerDTO = JsonConvert.DeserializeObject<TriggerDTO>(jsonResponse);
                    return triggerDTO;
                }
                else
                {
                    return new TriggerDTO();
                }



            }
        }
        /*
        public async Task<SelectTriggerDTO> getDataSelectTrigger(Instrument instrument, GenerateGraphDTO generateGraphDTO)
        {
            using (var httpClient = new HttpClient())
            {
                string url = "";

                if (instrument is Trigger)
                {
                    if (generateGraphDTO.getReDrawGraph())
                    {

                    }else if (gener)
                    else
                    {
                        url = $"http://localhost:7294/api/{instrument.getInstrumentTypeName()}/TypeGraph{generateGraphDTO.getIdTypeGraph()}/{generateGraphDTO.getNombreSensor()},{generateGraphDTO.getIdInstrumento()}";

                    }
                }

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    TriggerDTO triggerDTO = JsonConvert.DeserializeObject<TriggerDTO>(jsonResponse);
                    return triggerDTO;
                }
                else
                {
                    return new TriggerDTO();
                }



            }
  
        }
              */
    }
}
