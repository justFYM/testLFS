using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace TerriaMVC.APIControllers
{
   
    public class ConfigTerriaMapController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostEnvironment;
        public ConfigTerriaMapController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostEnvironment)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.hostEnvironment = hostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet("lib/TerriaMap/serverconfig")]
        public IActionResult ServerConfig()
        {
            
            object jsonObject = null; 


            jsonObject = new
            {
                allowProxyFor = new[]
          {
                "nicta.com.au",
                "gov.au",
                "csiro.au",
                "arcgis.com",
                "argo.jcommops.org",
                "www.abc.net.au",
                "geoserver.aurin.org.au",
                "mapsengine.google.com",
                "s3-ap-southeast-2.amazonaws.com",
                "adelaidecitycouncil.com",
                "www.dptiapps.com.au",
                "geoserver-123.aodn.org.au",
                "geoserver.imos.org.au",
                "nci.org.au",
                "static.nationalmap.nicta.com.au",
                "githubusercontent.com",
                "gov",
                "gov.uk",
                "gov.nz",
                "services.aremi.data61.io"
            },
                proxyAllDomains = false,
                version = "4.0.0"
            };


            return Ok(jsonObject);
        }


        [AllowAnonymous]
        [HttpGet("lib/TerriaMap/build/a22e6e4f04234b080bfffee8e4993335.DAC")]
        public IActionResult Get()
        {
            string fileName = "a22e6e4f04234b080bfffee8e4993335.DAC";
            string filePath = Path.Combine(hostEnvironment.WebRootPath, "lib/TerriaMap/build", fileName);

            if (System.IO.File.Exists(filePath))
            {
                // Devuelve el archivo como contenido en el navegador
                return PhysicalFile(filePath, "application/octet-stream");
            }

            // El archivo no existe, devuelve un código de estado 404
            return NotFound();
        }
    }
}
