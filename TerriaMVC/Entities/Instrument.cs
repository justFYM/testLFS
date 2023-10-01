using TerriaMVC.DTO;

namespace TerriaMVC.Entities
{
    public class Instrument
    {
        public int Id { get; set; }
        public int InstrumentTypeId { get; set; }
        public string InstrumentName { get; set; }

        public string InstrumentTypeName { get; set; }

        public List<Sensor> Sensors { get; set; }

        public void setId(int id)
        {
            this.Id = id;
        }
        public int getId()
        {
            return Id;
        }
        public void setInstrumentTypeId(int instrumentTypeId)
        {
            this.InstrumentTypeId = instrumentTypeId;
        }
        public int getInstrumentTypeId()
        {
            return this.InstrumentTypeId;
        }
        public void setInstrumentName(string instrumentName)
        {
            this.InstrumentName = instrumentName;
        }
        public string getInstrumentName()
        {
            return this.InstrumentName;
        }
        public void setInstrumenTypetName(string instrumentTypeName)
        {
            this.InstrumentTypeName = instrumentTypeName;
        }
        public string getInstrumentTypeName()
        {
            return this.InstrumentTypeName;
        }
        public void setSensors(List<Sensor> Sensors)
        {
            this.Sensors = Sensors;
        }
        public List<Sensor> getSensors()
        {
            return Sensors;
        }
        public void setData(Instrument instrument)
        {
            setId(instrument.getId());
            setInstrumentName(instrument.getInstrumentName());
            setInstrumentTypeId(instrument.getInstrumentTypeId());
            setInstrumenTypetName(instrument.getInstrumentTypeName());
            setSensors(instrument.getSensors());

        }

    }
}
