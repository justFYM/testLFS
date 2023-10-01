using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Services;

namespace TerriaMVC.APIControllers
{
    [Route("api/featureinformationpopup")]
    public class FeatureInformationPopUpController : ControllerBase
    {
        private readonly IInstrumentService instrumentService;
        private readonly IViewService viewService;

        public FeatureInformationPopUpController(IInstrumentService instrumentService, IViewService viewService)
        {
            this.instrumentService = instrumentService;
            this.viewService = viewService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InstrumentPopUpDTO dataRequest)
        {
            try
            {
                int idInstrumento = dataRequest.IdInstrumento;
                int idServicio = dataRequest.IdServicio;
                int idSensor = dataRequest.IdSensor;
                var nombreTipoInstrumento = dataRequest.NombreTipoInstrumento;
                var nombreSensor = dataRequest.NombreSensor;
                var nombreVariable = dataRequest.NombreVariable;

                /*
                 * -----Se cliquea un punto---------
                 * 1)El controlador recibe el tipo de instrumento.
                 * 2)Según el tipo de instrumento, se ocupará el servicio del instrumento que corresponde.
                 * 3)Se devolverán los botones de ese tipo de instrumento.
                 * -----Cuando se entregan los botones---------
                 * 1)Cada botón llamará a una función JavaScript para el tipo de instrumento que lo solicitó.
                 * 2)La función JavaScript se comunicará con el controlador del tipo de instrumento que corresponde.
                 * 3)El controlador recibirá la idTypeGraph (0: oneLine, 1:moreLines, 2: moreLinesAverage), idInstrumento, nombreTipoInstrumento, nombreVariable
                 * -----Se entregará el gráfico------------
                 */

                //idServicio, idSensor, nombreTipoInstrumento
                Console.WriteLine(nombreVariable);
            //    (SensorDataDTO sensorData, string json) = await instrumentService.GetSensorData(idInstrumento);
             //   var view = await viewService.GetView(sensorData, idServicio, idInstrumento, idSensor, nombreTipoInstrumento, nombreSensor, nombreVariable);
                
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar la respuesta JSON:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Respuesta JSON recibida:");
                return StatusCode(500);
            }
        }
    }
}
