using Microsoft.AspNetCore.Mvc;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Services;

namespace TerriaMVC.APIControllers
{
  
    [Route("api/generateAOI")]
    public class GenerateAOIController : ControllerBase
    {
        private readonly ClinoextensometroService clinoextensometroService;
        private readonly GNSSService gnssService;
        private readonly PiezometroService piezometroService;
        private readonly PrismaService prismaService;
        private readonly SensorHumedadService sensorHumedadService;
        private readonly TriggerService triggerService;
        private readonly IInstrumentService instrumentService;
        private readonly IServiceBaseMethods serviceBaseMethods;
        private readonly Radar01Service radar01Service;
        private readonly InSARService inSARService;
        private readonly IAOIService iAOIService;

        public GenerateAOIController(
            ClinoextensometroService clinoextensometroService,
            GNSSService gnssService,
            PiezometroService piezometroService,
            PrismaService prismaService,
            SensorHumedadService sensorHumedadService,
            TriggerService triggerService,
            IInstrumentService instrumentService,
            IServiceBaseMethods serviceBaseMethods,
            Radar01Service radar01Service,
            InSARService inSARService,
            IAOIService iAOIService)
        {
            this.clinoextensometroService = clinoextensometroService;
            this.gnssService = gnssService;
            this.piezometroService = piezometroService;
            this.prismaService = prismaService;
            this.sensorHumedadService = sensorHumedadService;
            this.triggerService = triggerService;
            this.instrumentService = instrumentService;
            this.serviceBaseMethods = serviceBaseMethods;
            this.radar01Service = radar01Service;
            this.inSARService = inSARService;
            this.iAOIService = iAOIService;
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenerateAOIDTO dataRequest)
        {
            try
            {

                var idsToRemove = new List<int>();

                foreach (var id in dataRequest.idInstrumentos)
                {
                    Console.WriteLine(id.ToString());
                    Instrument instrument = await instrumentService.findById(id);

                    if (instrument is InSAR || instrument is Radar01)
                    {
                        // Agregar el ID del instrumento a la lista de IDs a eliminar.
                        idsToRemove.Add(id);
                    }
                }

                // Eliminar los IDs de instrumentos que son de tipo InSAR o Radar01.
                foreach (var id in idsToRemove)
                {
                    dataRequest.idInstrumentos = dataRequest.idInstrumentos.Where(x => x != id).ToArray();
                }
                if(dataRequest.idInstrumentos.Count() > 0){
                    string content = await iAOIService.getContent(dataRequest);
                    return Ok(content);
                }
                else
                {
                    string content = await iAOIService.getContentNotAvailable(dataRequest);
                    return Ok(content);
                    return Ok("No disponible.");
                }
                

             
            }
            catch (Exception ex)
            {
                return BadRequest("Error al procesar los números: " + ex.Message);
            }
        }


    }
}
