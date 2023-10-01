using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public interface IAOIService
    {
        Task<string> getContent(GenerateAOIDTO dataRequest);
        Task<string> getContentNotAvailable(GenerateAOIDTO dataRequest);
    }
    public class AOIService : IAOIService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IInstrumentService instrumentService;
        private readonly ClinoextensometroService clinoextensometroService;
        private readonly GNSSService gnssService;
        private readonly PiezometroService piezometroService;
        private readonly PrismaService prismaService;
        private readonly SensorHumedadService sensorHumedadService;
        private readonly TriggerService triggerService;
        private readonly IServiceBaseMethods serviceBaseMethods;
        private readonly IRepositoryBaseMethods repositoryBaseMethods;

        public AOIService(IWebHostEnvironment webHostEnvironment, IInstrumentService instrumentService, ClinoextensometroService clinoextensometroService, GNSSService gnssService, PiezometroService piezometroService
            , PrismaService prismaService, SensorHumedadService sensorHumedadService, TriggerService triggerService, IServiceBaseMethods serviceBaseMethods, IRepositoryBaseMethods repositoryBaseMethods)
        {
         
            this.webHostEnvironment = webHostEnvironment;
            this.instrumentService = instrumentService;
            this.clinoextensometroService = clinoextensometroService;
            this.gnssService = gnssService;
            this.piezometroService = piezometroService;
            this.prismaService = prismaService;
            this.sensorHumedadService = sensorHumedadService;
            this.triggerService = triggerService;
            this.serviceBaseMethods = serviceBaseMethods;
            this.repositoryBaseMethods = repositoryBaseMethods;
        }
        public async Task<string> getContent(GenerateAOIDTO dataRequest)
        {
            if (dataRequest.defaultContent)
            {
                string content = await getView(dataRequest); //Generar la estructura html del popup con los botónes top y bot listos.
                return content;
            }
            else if (dataRequest.isForTop == true)
            {
                string content = await getViewForTop(dataRequest); //Ya se sabe que es para un tipo específico de instrumento
                return content;
            }
            else if (dataRequest.isForTopConsolidado == true)
            {
                Console.WriteLine("Entró acá.");
                string test = await getViewForTopConsolidado(dataRequest);
                return test;
            }
            else if (dataRequest.isForBotConsolidado == true)
            {
                Console.WriteLine("Entró en isForBotConsolidado.");
                string test = await getViewForBotConsolidado(dataRequest);
                return test;
            }
            else if (dataRequest.isForTop == false)
            {
                string content = await getViewForBot(dataRequest); //Ya se sabe que es para un tipo específico de instrumento
                return content;
            }
            return "";
        }
          public async Task<string> getContentNotAvailable(GenerateAOIDTO dataRequest)
        {
            var view = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", "NotAvailable.html");
            var content = await System.IO.File.ReadAllTextAsync(view);
            return content;
         
        }
        public async Task<List<Instrument>> getInstruments(GenerateAOIDTO dataRequest)
        {
            List<Instrument> instruments = new List<Instrument>();
            foreach (var id in dataRequest.idInstrumentos)
            {
                Instrument instrument = await instrumentService.findById(id);
                if (instrument is Clinoextensometro || instrument is Piezometro || instrument is GNSS || instrument is SensorHumedad || instrument is Trigger || instrument is Prisma)
                {
                    instruments.Add(instrument);
                }

                

              
            }
            var instrumentCounts = instruments
                    .GroupBy(i => i.getInstrumentTypeName()) // Agrupa por nombre del tipo (por ejemplo, "GNSS")
                    .Select(g => new
                    {
                        InstrumentType = g.Key,
                        Count = g.Count()
                    })
                    .ToList();
            /*
            foreach (var count in instrumentCounts)
            {
                Console.WriteLine($"{count.InstrumentType}: {count.Count}");
            }
            */
            return instruments;
        }
        public async Task<string> selectView(GenerateAOIDTO dataRequest)
        {
            if (dataRequest.defaultContent)
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", "DefaultContent.html");
                return filePath;
            }else if(dataRequest.isForTop == true)
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", "ForTop.html");
                return filePath;
            }
            else if (dataRequest.isForTopConsolidado == true)
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", "ForTopConsolidado.html");
                return filePath;
            }
            else if (dataRequest.isForBotConsolidado == true)
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", "ForBotConsolidado.html");
                return filePath;
            }
            else if (dataRequest.isForTop == false)

            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", "ForBot.html");
                return filePath;
            }
            return "";
        }
        public async Task<string> selectView(Instrument instrument, GenerateGraphDTO dataRequest)
        {
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", "AOI", $"{instrument.getInstrumentTypeName()}", $"GraphType{dataRequest.getIdTypeGraph()}.html");
            return filePath;
        }

        public async Task<string> getView(GenerateAOIDTO dataRequest)
        {
            List<Instrument> instruments = await getInstruments(dataRequest);
            var view = await selectView(dataRequest);
            var content = await System.IO.File.ReadAllTextAsync(view);
            var topButtons = await getTopIdentifiers(content, instruments);
            var botButtons = await getBotIdentifiers(topButtons, instruments);
            var graph = await getGraph(botButtons, instruments);
            botButtons = botButtons.Replace("[[GRAPH]]", $"{graph}");

            if (instruments[0].InstrumentTypeName.Equals("Trigger"))
            {
                botButtons = botButtons.Replace("[[CHECKMARGINTOP]]", "34.91px");
            }
            else
            {
                botButtons = botButtons.Replace("[[CHECKMARGINTOP]]", "auto");
            }
            bool hasPiezometroOrClinoextensometro = instruments.Any(inst =>
      inst.InstrumentTypeName.Equals("Piezometro", StringComparison.OrdinalIgnoreCase) ||
      inst.InstrumentTypeName.Equals("Clinoextensometro", StringComparison.OrdinalIgnoreCase)); 
            var startTag = "[[CHECK_PiezometroORClinoextensometro]]";
            var endTag = "[[END_CHECK_PiezometroORClinoextensometro]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;
            string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);
            int[] ConsolidadoInstrumentIdentifiers;
            if (hasPiezometroOrClinoextensometro)
            {

                botButtons = botButtons.Replace(startTag, "");
                botButtons = botButtons.Replace(endTag, "");
                // Filtrar los instrumentos por tipo "Piezometro" o "Clinoextensometro" y seleccionar las IDs únicas.
                ConsolidadoInstrumentIdentifiers = instruments
                    .Where(inst => inst.InstrumentTypeName.Equals("Piezometro", StringComparison.OrdinalIgnoreCase) ||
                                   inst.InstrumentTypeName.Equals("Clinoextensometro", StringComparison.OrdinalIgnoreCase))
                    .Select(inst => inst.Id)
                    .Distinct()
                    .ToArray();

                botButtons = botButtons.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtonsConsolidado([{string.Join(",", ConsolidadoInstrumentIdentifiers)}], false, false, true, \"Consolidado\")'");
                //Colocar la función onClick con el array de instrumentos del tipo que corresponde. Para ello, generar un array con las ids y dentro dle if colocar ese array.
            }
            else
            {
                botButtons = botButtons.Replace(extractedContent, "");
                botButtons = botButtons.Replace(startTag, "");
                botButtons = botButtons.Replace(endTag, "");
            }


            return botButtons;
        }
        public async Task<string> getViewForTop(GenerateAOIDTO dataRequest)
        {
            List<Instrument> instruments = await getInstruments(dataRequest);
            var view = await selectView(dataRequest); 
            var content = await System.IO.File.ReadAllTextAsync(view);
          //  var topButtons = await getTopIdentifiers(content, instruments);
            var botButtons = await getBotIdentifiers(content, instruments);
            var graph = await getGraph(botButtons, instruments);
            botButtons = botButtons.Replace("[[GRAPH]]", $"{graph}");
            if (instruments[0].InstrumentTypeName.Equals("Trigger"))
            {
                botButtons = botButtons.Replace("[[CHECKMARGINTOP]]", "34.91px");
            }
            else
            {
                botButtons = botButtons.Replace("[[CHECKMARGINTOP]]", "auto");
            }

           
            return botButtons;
        }
        public async Task<string> getViewForTopConsolidado(GenerateAOIDTO dataRequest)
        {
            List<Instrument> instruments = await getInstruments(dataRequest);
            /*
            var view = await selectView(dataRequest);
            var content = await System.IO.File.ReadAllTextAsync(view);
            //  var topButtons = await getTopIdentifiers(content, instruments);
            var botButtons = await getBotIdentifiers(content, instruments);
            var graph = await getGraph(botButtons, instruments);
            botButtons = botButtons.Replace("[[GRAPH]]", $"{graph}");
            if (instruments[0].InstrumentTypeName.Equals("Trigger"))
            {
                botButtons = botButtons.Replace("[[CHECKMARGINTOP]]", "34.91px");
            }
            else
            {
                botButtons = botButtons.Replace("[[CHECKMARGINTOP]]", "auto");
            }


            return botButtons;
            */
           
            var view = await selectView(dataRequest);
            var content = await System.IO.File.ReadAllTextAsync(view);
            //  var topButtons = await getTopIdentifiers(content, instruments);
            var botButtons = await getBotConsolidadoIdentifiers(content, instruments);
            var graph = await getGraphConsolidado(botButtons, instruments);
            botButtons = botButtons.Replace("[[GRAPH]]", $"{graph}");
          
            return botButtons;
        }

        public async Task<string> getViewForBotConsolidado(GenerateAOIDTO dataRequest)
        {
            List<Instrument> instruments = await getInstruments(dataRequest);
            var view = await selectView(dataRequest);
            var content = await System.IO.File.ReadAllTextAsync(view);
           // var botButtons = await getBotConsolidadoIdentifiers(content, instruments);
            var graph = await getGraphConsolidado(content, instruments);
            content = content.Replace("[[GRAPH]]", $"{graph}");

            return content;
        }
        


        public async Task<string> getViewForBot(GenerateAOIDTO dataRequest)
        {
            List<Instrument> instruments = await getInstruments(dataRequest);
            var view = await selectView(dataRequest);
            var content = await System.IO.File.ReadAllTextAsync(view);
        
            var graph = await getGraph(content, instruments);
            graph = graph.Replace("[[GRAPH]]", $"{graph}");
            return graph;
        }

        public async Task<string> getTopIdentifiers(string content, List<Instrument> instruments)
        {
            var startTag = "[[ITERAR_TOP]]";
            var endTag = "[[FIN_ITERAR_TOP]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;
            var activeButton = false;

            if (startIndex >= 0 && endIndex >= 0)
            {
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);
                var iteratedContent = new StringBuilder();

                var instrumentCounts = instruments
                      .GroupBy(i => i.getInstrumentTypeName()) // Agrupa por nombre del tipo (por ejemplo, "GNSS")
                      .Select(g => new
                      {
                          InstrumentType = g.Key,
                          Count = g.Count()
                      })
                      .ToList();
                foreach (var iterador in instrumentCounts)
                {
                    string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);
                    if (iterador.InstrumentType.Equals("GNSS"))
                    {
                        // Filtra la lista de instrumentos por el tipo "GNSS" y obtiene las IDs
                        var gnssInstrumentIds = string.Join(",", instruments
                            .Where(i => i.getInstrumentTypeName() == "GNSS")
                            .Select(i => i.Id));
                        // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtons([{string.Join(",", gnssInstrumentIds)}], false, true, \"{iterador.InstrumentType.ToString()}\")'");
                        extractedContent = extractedContent.Replace("[[TAB]]", "GNSS");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                    }
                    else if (iterador.InstrumentType.Equals("Prisma"))
                    {
                        // Filtra la lista de instrumentos por el tipo "GNSS" y obtiene las IDs
                        var prismaInstrumentIds = string.Join(",", instruments
                            .Where(i => i.getInstrumentTypeName() == "Prisma")
                            .Select(i => i.Id));
                        // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtons([{string.Join(",", prismaInstrumentIds)}], false , true, \"{iterador.InstrumentType.ToString()}\")'");
                        extractedContent = extractedContent.Replace("[[TAB]]", "Prisma");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                    }
                    else if (iterador.InstrumentType.Equals("Piezometro"))
                    {
                        var piezometroInstrumentIds = string.Join(",", instruments
                        .Where(i => i.getInstrumentTypeName() == "Piezometro")
                        .Select(i => i.Id));
                        // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtons([{string.Join(",", piezometroInstrumentIds)}], false , true, \"{iterador.InstrumentType.ToString()}\")'");
                        extractedContent = extractedContent.Replace("[[TAB]]", "Piezometro");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                       
                    }
                    else if (iterador.InstrumentType.Equals("Sensor_Humedad"))
                    {
                        var sensorHumedadInstrumentIds = string.Join(",", instruments
                            .Where(i => i.getInstrumentTypeName() == "Sensor_Humedad")
                            .Select(i => i.Id));
                        // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtons([{string.Join(",", sensorHumedadInstrumentIds)}], false , true, \"{iterador.InstrumentType.ToString()}\")'");
                        extractedContent = extractedContent.Replace("[[TAB]]", "SensorHumedad");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                       
                    }
                    else if (iterador.InstrumentType.Equals("Clinoextensometro"))
                    {
                        // Filtra la lista de instrumentos por el tipo "GNSS" y obtiene las IDs
                        var clinoextensometroInstrumentIds = string.Join(",", instruments
                            .Where(i => i.getInstrumentTypeName() == "Clinoextensometro")
                            .Select(i => i.Id));
                        // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtons([{string.Join(",", clinoextensometroInstrumentIds)}], false , true, \"{iterador.InstrumentType.ToString()}\")',");
                        extractedContent = extractedContent.Replace("[[TAB]]", "Clinoextensometro");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                        
                    }
                    else if (iterador.InstrumentType.Equals("Trigger"))
                    {
                        // Filtra la lista de instrumentos por el tipo "GNSS" y obtiene las IDs
                        var triggerInstrumentIds = string.Join(",", instruments
                            .Where(i => i.getInstrumentTypeName() == "Trigger")
                            .Select(i => i.Id));
                        // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateChartAndButtons([{string.Join(",", triggerInstrumentIds)}], false , true, \"{iterador.InstrumentType.ToString()}\")',");
                        extractedContent = extractedContent.Replace("[[TAB]]", "Trigger");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }

                        

                    }
                    iteratedContent.AppendLine(extractedContent);
                    activeButton = true;
                }
                content = startContent + iteratedContent.ToString() + endContent;

            }
            return content;
        }


        public async Task<string> getBotIdentifiers(string content, List<Instrument> instruments)
        {
            var startTag = "[[ITERAR_BOT]]";
            var endTag = "[[FIN_ITERAR_BOT]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;

            if (startIndex >= 0 && endIndex >= 0)
            {
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);
                var iteratedContent = new StringBuilder();
                var activeButton = false;
                if (instruments.Any())
                {
                    var firstInstrumentType = instruments[0].getInstrumentTypeName();
                    var filteredInstruments = instruments.Where(i => i.getInstrumentTypeName() == firstInstrumentType).ToList();
               
                    foreach (var iterador in filteredInstruments)
                    {
                        string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);
                        if (iterador.getInstrumentTypeName().Equals("GNSS"))
                        {
                        
                            // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                            extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIContent({iterador.getId()}, false,false, \"{iterador.getInstrumentName().ToString()}\")'");
                            extractedContent = extractedContent.Replace("[[TAB]]", $"{iterador.getInstrumentName()}");
                            if (activeButton == true)
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                            }
                            else
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                            }
                        }
                        else if (iterador.getInstrumentTypeName().Equals("Prisma"))
                        {
                     
                            // Reemplaza [[FUNCTION]] con el botón onClick que contiene la lista de IDs GNSS
                            extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIContent({iterador.getId()}, false,false, \"{iterador.getInstrumentName().ToString()}\")'");
                            extractedContent = extractedContent.Replace("[[TAB]]", $"{iterador.getInstrumentName()}");
                            if (activeButton == true)
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                            }
                            else
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                            }
                        }
                        else if (iterador.getInstrumentTypeName().Equals("Piezometro"))
                        {
                           
                            extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIContent({iterador.getId()}, false,false, \"{iterador.getInstrumentName().ToString()}\")'");
                            extractedContent = extractedContent.Replace("[[TAB]]", $"{iterador.getInstrumentName()}");
                            if (activeButton == true)
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                            }
                            else
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                            }
                        }
                        else if (iterador.getInstrumentTypeName().Equals("Sensor_Humedad"))
                        {
                            extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIContent({iterador.getId()}, false,false, \"{iterador.getInstrumentName().ToString()}\")'");
                            extractedContent = extractedContent.Replace("[[TAB]]", $"{iterador.getInstrumentName()}");
                            if (activeButton == true)
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                            }
                            else
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                            }
                        }
                        else if (iterador.getInstrumentTypeName().Equals("Clinoextensometro"))
                        {
                            extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIContent({iterador.getId()}, false,false, \"{iterador.getInstrumentName().ToString()}\")'");
                            extractedContent = extractedContent.Replace("[[TAB]]", $"{iterador.getInstrumentName()}");
                            if (activeButton == true)
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                            }
                            else
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                            }
                           
                        }
                        else if (iterador.getInstrumentTypeName().Equals("Trigger"))
                        {
                            extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIContent({iterador.getId()}, false,false, \"{iterador.getInstrumentName().ToString()}\")'");
                            extractedContent = extractedContent.Replace("[[TAB]]", $"{iterador.getInstrumentName()}");
                            if (activeButton == true)
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                            }
                            else
                            {
                                extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                            }

                        }
                        activeButton = true;
                        iteratedContent.AppendLine(extractedContent);

                    }



                   
                }

                content = startContent + iteratedContent.ToString() + endContent;
            }
            return content;
        }
        public async Task<string> getBotConsolidadoIdentifiers(string content, List<Instrument> instruments)
        {
            var startTag = "[[ITERAR_BOT]]";
            var endTag = "[[FIN_ITERAR_BOT]]";
            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;

            if (startIndex >= 0 && endIndex >= 0)
            {
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);
                var iteratedContent = new StringBuilder();
                var activeButton = false;
                var tiposDeInstrumentos = instruments.Select(i => i.InstrumentTypeName).Distinct();
                int[] identificadoresClinoextensometro = new int[0]; // Inicialmente vacío
                int[] identificadoresPiezometro = new int[0]; // Inicialmente vacío

                foreach (var tipo in tiposDeInstrumentos)
                {
                    string extractedContent = content.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length - endTag.Length);
                    Console.WriteLine($"Tipo de instrumento: {tipo}");

                    // Filtrar los instrumentos por tipo
                    var instrumentosPorTipo = instruments.Where(i => i.InstrumentTypeName == tipo).ToList();
                    
                    // Realizar acciones según el tipo de instrumento
                    if (tipo == "Clinoextensometro")
                    {
                        Console.WriteLine("Instrumentos Clinoextensometro:");
                        identificadoresClinoextensometro = instrumentosPorTipo.Select(i => i.Id).ToArray();
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIConsolidadoContent([{string.Join(",", identificadoresClinoextensometro)}], false,false,false,true,\"Vertical\")'");
                        extractedContent = extractedContent.Replace("[[TAB]]", "Vertical");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                    }
                    else if (tipo == "Piezometro")
                    {
                        Console.WriteLine("Instrumentos Piezometro:");
                        identificadoresPiezometro = instrumentosPorTipo.Select(i => i.Id).ToArray();
                        extractedContent = extractedContent.Replace("[[FUNCTION]]", $"onClick='GenerateGraphAOIConsolidadoContent([{string.Join(",", identificadoresPiezometro)}], false,false,false,true,\"Presión\")'");
                        extractedContent = extractedContent.Replace("[[TAB]]", "Presión");
                        if (activeButton == true)
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "");
                        }
                        else
                        {
                            extractedContent = extractedContent.Replace("[[ACTIVE]]", "active");
                        }
                    }
                    // Puedes agregar más casos para otros tipos de instrumentos si es necesario.
                    //return extractedContent;
                    activeButton = true;
                    iteratedContent.AppendLine(extractedContent);
                }
                content = startContent + iteratedContent.ToString() + endContent;
            }

            return content;
        }
     public async Task<string> getGraph(string content, List<Instrument> instruments)
        {
            if (instruments.Any())
            {
                // Obtenemos el tipo del primer instrumento
                Instrument instrument = instruments[0];
                GenerateGraphDTO dataRequest = new GenerateGraphDTO();
                string graph = "";
                if (instrument is GNSS)
                {
                    dataRequest.IdTypeGraph = 1;
                    dataRequest.AverageRequired = false;
                    graph = await gnssService.generateGraph(instrument, dataRequest);
                    // return graph;
                }
                if (instrument is Piezometro)
                {
                    dataRequest.IdTypeGraph = 1;
                    dataRequest.AverageRequired = false;
                    graph = await piezometroService.generateGraph(instrument, dataRequest);
                    //  return graph;
                }
                if (instrument is SensorHumedad)
                {
                    dataRequest.IdTypeGraph = 1;
                    dataRequest.AverageRequired = false;
                    graph = await sensorHumedadService.generateGraph(instrument, dataRequest);
                    // return graph;
                }
                if (instrument is Clinoextensometro)
                {
                    dataRequest.IdTypeGraph = 1;
                    dataRequest.AverageRequired = false;
                    dataRequest.NombreVariable = "deformación_acumulada_vertical";

                    graph = await clinoextensometroService.generateGraph(instrument, dataRequest);
                    //  Console.WriteLine("Hola!");
                    //   return graph;
                }
                if (instrument is Trigger)
                {
                    dataRequest.IdTypeGraph = 1;
                    dataRequest.AverageRequired = false;
                    dataRequest.IdInstrumento = instrument.getId();
                    dataRequest.NombreSensor = "Longitudinal-Transversal-Vertical";
                    dataRequest.ReDrawGraph = false;
                    dataRequest.ReDrawSelect = false;

                    graph = await triggerService.generateGraphTrigger(instrument, dataRequest);


                    //    return graph;

                    //  return Ok(graph);
                }
                if (instrument is Prisma)
                {

                    dataRequest.IdTypeGraph = 1;
                    dataRequest.AverageRequired = false;
                    graph = await prismaService.generateGraph(instrument, dataRequest);
                    //   return graph;
                    // return Ok(graph);

                }
                if (instrument is Radar01)
                {
                    return "No disponible";
                }
                if (instrument is InSAR)
                {
                    return "No disponible";
                }

                    return graph;
            }

                return "";
        }
        public async Task<string> getGraphConsolidado(string content, List<Instrument> instruments)
        {
            // Chequear si el primer elemento corresponde a "Piezometro" o "Clinoextensometro".
            var firstInstrumentType = instruments[0].getInstrumentTypeName();

            string instrumentosPorTipo = "";

            if (firstInstrumentType.Equals("Piezometro", StringComparison.OrdinalIgnoreCase))
            {
                // Filtrar instruments por piezómetro y obtener los nombres de los instrumentos.
                var instrumentNames = instruments
                    .Where(i => i.InstrumentTypeName.Equals("Piezometro", StringComparison.OrdinalIgnoreCase))
                    .Select(i => i.Id);

                // Crear una cadena separada por comas de los nombres de los instrumentos.
                instrumentosPorTipo = string.Join(",", instrumentNames);
                ConsolidadoAOIDTO instrumentsData = await piezometroService.generateDataAOIConsolidado(instrumentosPorTipo);
                var graph = await piezometroService.insertGraphDataType1ConsolidadoAOI(instrumentsData);
                return graph;

                Console.WriteLine("El primer elemento es un Piezometro.");
            }
            else if (firstInstrumentType.Equals("Clinoextensometro", StringComparison.OrdinalIgnoreCase))
            {
                // Filtrar instruments por clinoextensómetro y obtener los nombres de los instrumentos.
                var instrumentNames = instruments
                    .Where(i => i.InstrumentTypeName.Equals("Clinoextensometro", StringComparison.OrdinalIgnoreCase))
                    .Select(i => i.Id);

                // Crear una cadena separada por comas de los nombres de los instrumentos.
                instrumentosPorTipo = string.Join(",", instrumentNames);
                ConsolidadoAOIDTO instrumentsData = await clinoextensometroService.generateDataAOIConsolidado(instrumentosPorTipo);
                var graph = await clinoextensometroService.insertGraphDataType1ConsolidadoAOI(instrumentsData);
                Console.WriteLine("El primer elemento es un Clinoextensometro.");
                return graph;
            }
            else
            {
                Console.WriteLine("El primer elemento no es ni Piezometro ni Clinoextensometro.");
            }

            return "";
        }



    }
}
