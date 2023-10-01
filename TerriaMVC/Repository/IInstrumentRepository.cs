using Newtonsoft.Json;

using TerriaMVC.DTO;
using TerriaMVC.Entities;

namespace TerriaMVC.Repository
{
    public interface IInstrumentRepository
    {
        Task<Instrument> findById(int id);
        Task<Instrument> findBySensorId(int id);
    }
    public class InstrumentRepository : IInstrumentRepository
    {
        public async Task<Instrument> findById(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"http://localhost:7294/api/instrument/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Deserializar el contenido JSON en un objeto Instrument
                    Instrument instrument = JsonConvert.DeserializeObject<Instrument>(content);

                   
              

                    return instrument;
                }
                else
                {
                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                    // ...
                    return null; // Aquí podrías lanzar una excepción o manejar el error según tu lógica
                }
            }
        }


        public async Task<Instrument> findBySensorId(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"http://localhost:7294/api/instrument/bySensorId/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Deserializar el contenido JSON en un objeto Instrument
                    Instrument instrumentId = JsonConvert.DeserializeObject<Instrument>(content);

                    Instrument instrument = await findById(instrumentId.getId());


                    return instrument;
                }
                else
                {
                    // Manejar el caso de error de la solicitud HTTP, si es necesario
                    // ...
                    return null; // Aquí podrías lanzar una excepción o manejar el error según tu lógica
                }
            }
        }


    }
}

