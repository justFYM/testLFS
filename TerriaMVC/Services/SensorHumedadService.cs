using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{

    public class SensorHumedadService : ServiceBaseMethods
    {
        public SensorHumedadService(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, IWebHostEnvironment webHostEnvironment) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
        }
    }

}
