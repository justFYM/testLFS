
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public interface IInstrumentService
    {
        Task<Instrument> findById(int id);
        Task<Instrument> findBySensorId(int id);

        Task<string> selectView(Instrument instrument, string archivo);
    }
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public InstrumentService(IInstrumentRepository instrumentRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.instrumentRepository = instrumentRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<Instrument> findById(int id)
        {
            Instrument instrumentBd = await instrumentRepository.findById(id);

            if (instrumentBd.getInstrumentTypeName().Equals("Piezometro"))
            {
                Piezometro piezometro = new Piezometro();
                piezometro.setData(instrumentBd);
                return piezometro;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Trigger"))
            {
                Trigger trigger = new Trigger();
                trigger.setData(instrumentBd);
                return trigger;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("GNSS"))
            {
                GNSS gnss = new GNSS();
                gnss.setData(instrumentBd);
                return gnss;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Prisma"))
            {
                Prisma prisma = new Prisma();
                prisma.setData(instrumentBd);
                return prisma;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Sensor_Humedad"))
            {
                SensorHumedad sensorHumedad = new SensorHumedad();
                sensorHumedad.setData(instrumentBd);
                return sensorHumedad;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Clinoextensometro"))
            {
                Clinoextensometro clinoextensometro = new Clinoextensometro();
                clinoextensometro.setData(instrumentBd);
                return clinoextensometro;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Radar01"))
            {
                Radar01 radar01 = new Radar01();
                radar01.setData(instrumentBd);
                return radar01;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("InSAR"))
            {
                InSAR inSAR = new InSAR();
                inSAR.setData(instrumentBd);
                return inSAR;
            }
            return instrumentBd;

        }
        public async Task<string> selectView(Instrument instrument, string archivo)
        {
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "htmltemplates", $"{instrument.getInstrumentTypeName()}", $"{archivo}");
            return filePath;
        }

        /*
        public async Task<string> insertGraphData(SensorDataDTO sensorData, string vista)
        {
            var content = await System.IO.File.ReadAllTextAsync(vista);
            var startTag = "[[ITERAR]]";
            var endTag = "[[FIN_ITERAR]]";

            var startIndex = content.IndexOf(startTag);
            var endIndex = content.IndexOf(endTag) + endTag.Length;
            if (startIndex >= 0 && endIndex >= 0)
            {
                var startContent = content.Substring(0, startIndex);
                var endContent = content.Substring(endIndex);

                var iteratedContent = new StringBuilder();
                Sensor sensorMayor = new Sensor();
                foreach (var sensor in sensorData.sensores)
                {

                    var sensorEjeYFormatted = string.Join(",", sensor.sensor_eje_y.Select(value => value != null ? value.ToString().Replace(',', '.') : "null"));

                    var sensorDataString = "";


                    sensorDataString = $@"{{name: '{sensor.sensor_name}',data: [{sensorEjeYFormatted}]}},";


                    iteratedContent.AppendLine(sensorDataString);
                }

                // Reemplaza la sección [[ITERAR]] y [[FIN_ITERAR]] con el contenido iterado para cada sensor
                content = startContent + iteratedContent.ToString() + endContent;


                content = content.Replace("{{sensor.sensor_eje_x}}", $"{string.Join(",", sensorData.sensores[0].sensor_eje_x.Select(date => $"'{date}'"))}");

                content = content.Replace("{{nombreTipoInstrumento}}", nombreTipoInstrumento);
                content = content.Replace("{{nombreSensor}}", nombreSensor);
                if (nombreVariable.Equals("tasa_mensual_de_asentamiento"))
                {
                    content = content.Replace("{{nombreVariable}}", "Tasa Mensual de Asentamiento");
                }
                else if (nombreVariable.Equals("tasa_semanal_de_asentamiento"))
                {
                    content = content.Replace("{{nombreVariable}}", "Tasa Semanal de Asentamiento");
                }
                else if (nombreVariable.Equals("deformación_acumulada_vertical"))
                {
                    content = content.Replace("{{nombreVariable}}", "Deformación Acumulada Vertical");
                }

                // Console.WriteLine(content);
                return content;
            }
       
        }
         */


        public async Task<Instrument> findBySensorId(int id)
        {
            Instrument instrumentBd = await instrumentRepository.findBySensorId(id);

            if (instrumentBd.getInstrumentTypeName().Equals("Piezometro"))
            {
                Piezometro piezometro = new Piezometro();
                piezometro.setData(instrumentBd);
                return piezometro;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Trigger"))
            {
                Trigger trigger = new Trigger();
                trigger.setData(instrumentBd);
                return trigger;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("GNSS"))
            {
                GNSS gnss = new GNSS();
                gnss.setData(instrumentBd);
                return gnss;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Prisma"))
            {
                Prisma prisma = new Prisma();
                prisma.setData(instrumentBd);
                return prisma;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Sensor_Humedad"))
            {
                SensorHumedad sensorHumedad = new SensorHumedad();
                sensorHumedad.setData(instrumentBd);
                return sensorHumedad;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Clinoextensometro"))
            {
                Clinoextensometro clinoextensometro = new Clinoextensometro();
                clinoextensometro.setData(instrumentBd);
                return clinoextensometro;
            }
            if (instrumentBd.getInstrumentTypeName().Equals("Radar01"))
            {
                Radar01 radar01 = new Radar01();
                radar01.setData(instrumentBd);
                return radar01;
            }
            return instrumentBd;

        }


    }
}

