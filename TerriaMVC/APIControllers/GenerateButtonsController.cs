using Microsoft.AspNetCore.Mvc;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;
using TerriaMVC.Services;

namespace TerriaMVC.APIControllers
{

    [Route("api/generateButtons")]
    public class GenerateButtonsController : ControllerBase
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

        public GenerateButtonsController(
            ClinoextensometroService clinoextensometroService,
            GNSSService gnssService,
            PiezometroService piezometroService,
            PrismaService prismaService,
            SensorHumedadService sensorHumedadService,
            TriggerService triggerService,
            IInstrumentService instrumentService,
            IServiceBaseMethods serviceBaseMethods,
            Radar01Service radar01Service,
            InSARService inSARService)
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
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenerateButtonsDTO dataRequest)
        {
            try
            {
       
                Instrument instrument = await instrumentService.findById(dataRequest.IdInstrumento);
                Console.WriteLine("-----------------");
             
       Console.WriteLine("Nombre: " + instrument.getInstrumentName());
       Console.WriteLine("TypeId: " + instrument.getInstrumentTypeId());
       Console.WriteLine("TypeName: " + instrument.getInstrumentTypeName());
       Console.WriteLine("Id: " + instrument.getId());
       Console.WriteLine("Sensores: "+instrument.getSensors().Count());
      
                /*
                foreach (var sensor in instrument.Sensors)
                {
                    Console.WriteLine("Nombre: " + sensor.sensor_name);
                    Console.WriteLine("Id: " + sensor.sensor_id);
                }
                */

                if (instrument is GNSS)
                {
                    string buttons = await gnssService.generateButtons(instrument,dataRequest);
                    return Ok(buttons);
                }
                if (instrument is Piezometro)
                {
                    string buttons = await piezometroService.generateButtons(instrument, dataRequest);
                    return Ok(buttons);
                }
                if (instrument is SensorHumedad)
                {
                    string buttons = await sensorHumedadService.generateButtons(instrument, dataRequest);
                    return Ok(buttons);
                }
                if (instrument is Clinoextensometro)
                {
                    string buttons = await serviceBaseMethods.generateButtons(instrument, dataRequest);
                    return Ok(buttons);
                }
                if (instrument is Trigger)
                {
                    string buttons = await triggerService.generateButtons(instrument, dataRequest);
                    return Ok(buttons);
                }
                if (instrument is Prisma)
                {
                    string buttons = await prismaService.generateButtons(instrument, dataRequest);
                    return Ok(buttons);

                }
                if (instrument is Radar01)
                {
                     string buttons = await radar01Service.generateButtons(instrument, dataRequest);
                     return Ok(buttons);
                    //Console.WriteLine("Radar!");
                    //return Ok();

                }
                if (instrument is InSAR)
                {
                    Console.WriteLine("Es InSAR");
                    string buttons = await inSARService.generateButtons(instrument, dataRequest);
                    Console.WriteLine("TypeButton: " + dataRequest.IdTypeButtons);
                    return Ok(buttons);
                 
                    //return Ok();

                }
                Console.WriteLine("-----------------");


                /*
                  if (nombreTipoInstrumento.Equals("Piezometro"))
                  {
                      Console.WriteLine(nombreTipoInstrumento);
                  }
                  else if(nombreTipoInstrumento.Equals("Sensor_Humedad"))
                  {
                      Console.WriteLine(nombreTipoInstrumento);
                  }
                  else if (nombreTipoInstrumento.Equals("Prisma"))
                  {
                      Console.WriteLine(nombreTipoInstrumento);
                  }
                  else if (nombreTipoInstrumento.Equals("GNSS"))
                  {
                      Console.WriteLine(nombreTipoInstrumento);
                  }
                  else if (nombreTipoInstrumento.Equals("Clinoextensometro"))
                  {
                      string texto= await clinoextensometroService.generateButtons(idInstrumento, nombreTipoInstrumento);
                      Console.WriteLine(texto);
                  }
                  else if (nombreTipoInstrumento.Equals("Trigger"))
                  {
                      Console.WriteLine(nombreTipoInstrumento);
                  }
                */


                /*
                 * Realizar findById.
                 * Llamar a Servicio
                 
                 */

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
                // Console.WriteLine(nombreVariable);
                //(SensorDataDTO sensorData, string json) = await instrumentService.GetSensorData(idInstrumento);
                //  var view = await viewService.GetView(sensorData, idServicio, idInstrumento, idSensor, nombreTipoInstrumento, nombreSensor, nombreVariable);

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
