using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Services;

namespace TerriaMVC.APIControllers
{
    public class GenerateLogBookController: ControllerBase
    {
        private readonly ClinoextensometroService clinoextensometroService;
        private readonly PiezometroService piezometroService;
        private readonly PrismaService prismaService;
        private readonly SensorHumedadService sensorHumedadService;
        private readonly TriggerService triggerService;
        private readonly IInstrumentService instrumentService;
        private readonly Radar01Service radar01Service;
        private readonly InSARService inSARService;
        private readonly IServiceBaseMethods serviceBaseMethods;
        private readonly GNSSService gnssService;

        public GenerateLogBookController(ClinoextensometroService clinoextensometroService, GNSSService gnssService, PiezometroService piezometroService
            , PrismaService prismaService, SensorHumedadService sensorHumedadService, TriggerService triggerService, IInstrumentService instrumentService, Radar01Service radar01Service, InSARService inSARService, IServiceBaseMethods serviceBaseMethods)
        {
            this.clinoextensometroService = clinoextensometroService;
            this.piezometroService = piezometroService;
            this.prismaService = prismaService;
            this.sensorHumedadService = sensorHumedadService;
            this.triggerService = triggerService;
            this.instrumentService = instrumentService;
            this.radar01Service = radar01Service;
            this.inSARService = inSARService;
            this.serviceBaseMethods = serviceBaseMethods;
            this.gnssService = gnssService;
        }
        [HttpPost("api/GenerateLogBookController", Name = "LogBookController")]
        public async Task<IActionResult> LogBookController([FromBody] GenerateLogBookDTO dataRequest)
        {
            /*
            
            Console.WriteLine("Nombre variable: "+dataRequest.getNombreVariable());
            Console.WriteLine("Promedio: " + dataRequest.getAverageRequired());
            Console.WriteLine("Id instrumento: " + dataRequest.getIdInstrumento());
            Console.WriteLine("Id tipo de gráfico: " + dataRequest.getIdTypeGraph());
            Console.WriteLine("Nombre sensor: " + dataRequest.getNombreSensor());
            Console.WriteLine("Id sensor: " + dataRequest.getIdSensor());
            
            Console.WriteLine(dataRequest.getReDrawSelect());
            Console.WriteLine(dataRequest.getHourToAverage());
            */

            Instrument instrument = await instrumentService.findById(dataRequest.getIdInstrumento());
            /*
            if (instrument is GNSS)
            {
                
            }
            */
            if (instrument is Piezometro || instrument is GNSS || instrument is SensorHumedad || instrument is Clinoextensometro
                || instrument is Trigger|| instrument is Prisma || instrument is InSAR)
            {
                if ((bool)dataRequest.SaveNote)
                {
                    Console.WriteLine("Entró acá");
                    Console.WriteLine(dataRequest.Note);
                    Console.WriteLine(dataRequest.IdSensor);
                   
                    if (DateTime.TryParseExact(dataRequest.DateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                    {
                        // dateTime ahora contiene la fecha parseada
                        Console.WriteLine(dateTime);
                        string formattedDate = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
                        Console.WriteLine(formattedDate);
                       IActionResult result= await serviceBaseMethods.SaveLog(dataRequest);
                        return Ok(result);
                    }
                 
                }
                else
                {
                    GenerateLogBookDTO generateLogBookDTO = await piezometroService.findLogBookByInstrumentId(dataRequest);
                    Console.WriteLine(generateLogBookDTO.SensorNotes.Count());
                    string logBook = await serviceBaseMethods.generateLogBook(generateLogBookDTO);
                    return Ok(logBook);
                }
                
            } else if (instrument is Radar01)
            {
               
                if ((bool)dataRequest.SaveNote)
                {
                    Console.WriteLine("Entró acá");
                    Console.WriteLine(dataRequest.Note);
                    Console.WriteLine(dataRequest.IdSensor);
                    if (DateTime.TryParseExact(dataRequest.DateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                    {
                        Console.WriteLine(dateTime);
                        string formattedDate = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
                        Console.WriteLine(formattedDate);
                        IActionResult result = await serviceBaseMethods.SaveLog(dataRequest);
                        return Ok(result);
                    }
                }
                else
                {
                   
                   // Console.WriteLine(generateLogBookDTO.SensorNotes.Count());
                    if (instrument.getInstrumentName().Equals("Coordinador 01"))
                    {
                        GenerateLogBookDTO generateLogBookDTO = await radar01Service.FindLogBookCoordinadorByInstrumentId(dataRequest);
                        Console.WriteLine("GenerateLogBook Coordinador Test!");
                        string logBook = await radar01Service.generateLogBook(generateLogBookDTO);
                        // return Ok(logBook);
                        return Ok(logBook);
                    }
                    else
                    {
                        GenerateLogBookDTO generateLogBookDTO = await radar01Service.findLogBookByInstrumentId(dataRequest);
                        string logBook = await serviceBaseMethods.generateLogBook(generateLogBookDTO);
                        return Ok(logBook);
                    }
                    
                }
            }
            /*
            if (instrument is SensorHumedad)
            {
            
            }
            if (instrument is Clinoextensometro)
            {
               
            }
            if (instrument is Trigger)
            {
              
            }
            if (instrument is Prisma)
            {

            }
            if (instrument is Radar01)
            {
                
            }
            if (instrument is InSAR)
            {
              
            }
            */


            return Ok();
        }
    }
}
