using Microsoft.AspNetCore.Mvc;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Services;

namespace TerriaMVC.APIControllers
{
    public class GenerateGraphController : ControllerBase
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

        public GenerateGraphController(ClinoextensometroService clinoextensometroService, GNSSService gnssService, PiezometroService piezometroService
            , PrismaService prismaService, SensorHumedadService sensorHumedadService, TriggerService triggerService, IInstrumentService instrumentService, Radar01Service radar01Service,InSARService inSARService,IServiceBaseMethods serviceBaseMethods)
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

        [HttpPost("api/GenerateGraphController", Name = "GraphController")]
        public async Task<IActionResult> GraphController([FromBody] GenerateGraphDTO dataRequest)
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
            if (instrument is GNSS)
            {
                string graph = await gnssService.generateGraph(instrument, dataRequest);
                return Ok(graph);
            }
            if (instrument is Piezometro)
            {
                string graph = await piezometroService.generateGraph(instrument, dataRequest);
                return Ok(graph);
            }
            if (instrument is SensorHumedad)
            {
                string graph = await sensorHumedadService.generateGraph(instrument, dataRequest);
                return Ok(graph);
            }
            if (instrument is Clinoextensometro)
            {
                string graph = await clinoextensometroService.generateGraph(instrument, dataRequest);
              //  Console.WriteLine("Hola!");
                return Ok(graph);
            }
            if (instrument is Trigger)
            {
                string[] parts = dataRequest.NombreSensor.Split('-');
                Console.WriteLine("Sensores: ");
                foreach (string part in parts)
                {
                    Console.WriteLine(part);
                }
                if (dataRequest.getReDrawSelect())
                {
                    string select = await triggerService.generateSelectTrigger(instrument, dataRequest);
                    Console.WriteLine("dataRequest.getReDrawSelect()==true");
                    return Ok(select);
                }
                if (dataRequest.getReDrawGraph())
                {
                    string reDrawGraph = await triggerService.generateGraphTrigger(instrument, dataRequest);
                    return Ok(reDrawGraph);
                }
                else
                {
                   string graph = await triggerService.generateGraphTrigger(instrument, dataRequest);
                   //  string graph = await triggerService.testInstrument();

                    return Ok(graph);
                }
                /*
                StringBuilder selectBuilder = new StringBuilder();
                selectBuilder.AppendLine("<select style=\"padding: 0.5rem 0.5rem; font-size: 0.875rem;\" id=\"selectFechas\" class=\"form-control text-center\">");
                selectBuilder.AppendLine("<option value=\"1\">Opción 1</option>");
                selectBuilder.AppendLine("<option value=\"2\">Opción 2</option>");
                selectBuilder.AppendLine("<option value=\"3\">Opción 3</option>");
                selectBuilder.AppendLine("</select>");
                string select = selectBuilder.ToString();
                  */
                //  return Ok(graph);
            }
            if (instrument is Prisma)
            {
                string graph = await gnssService.generateGraph(instrument, dataRequest);
                return Ok(graph);

            }
            if (instrument is Radar01)
            {
                string graph = "";
                if (instrument.getInstrumentName().Equals("Coordinador 01"))
                {
                    Console.WriteLine("Radar Coordinador 01!");
                    dataRequest.IdTypeGraph=1;
                    // graph = await radar01Service.generateGraph(instrument, dataRequest);
                    graph = await radar01Service.generateRadarGraph(instrument, dataRequest);
                    return Ok(graph);
                }

                graph = await radar01Service.generateRadarGraph(instrument, dataRequest);
                //graph = await radar01Service.generateGraph(instrument, dataRequest);
                //  Console.WriteLine("Hola!");
                return Ok(graph);
            }
            if (instrument is InSAR)
            {
                string graph = "";
                graph = await inSARService.generateGraph(instrument, dataRequest);
                Console.WriteLine("Entró a la parte InSAR");
                return Ok(graph);
            }



            return Ok();
        }
           
    }
}