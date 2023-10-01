using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public class PrismaService : ServiceBaseMethods
    {
        public PrismaService(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, IWebHostEnvironment webHostEnvironment) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
        }
    }



}
