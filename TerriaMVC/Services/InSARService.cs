using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public class InSARService : ServiceBaseMethods
    {


        public InSARService(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, IWebHostEnvironment webHostEnvironment) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
        }
    }

}
