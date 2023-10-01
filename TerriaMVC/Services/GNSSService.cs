

using Microsoft.AspNetCore.Hosting;
using System.Diagnostics.Metrics;
using TerriaMVC.DTO;
using TerriaMVC.Entities;
using TerriaMVC.Repository;

namespace TerriaMVC.Services
{
    public class GNSSService : ServiceBaseMethods
    {
        

        public GNSSService(IInstrumentService instrumentService, IRepositoryBaseMethods repositoryBaseMethods, IWebHostEnvironment webHostEnvironment) : base(instrumentService, repositoryBaseMethods, webHostEnvironment)
        {
        }
 
    }


}
