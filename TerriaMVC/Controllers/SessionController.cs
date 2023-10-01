using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using TerriaMVC.DTO;
using TerriaMVC.Models;

namespace TerriaMVC.Controllers
{
    public class SessionController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SessionController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
           this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://localhost:7274/login", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                      
                        // Si la respuesta es exitosa, procesa según sea necesario
                        var successContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(successContent);
                        //Obtener los datos relevantes de la respuesta de la API externa
                        //Luego, descomentar línea 55,56,57
                        var usuario = JsonConvert.DeserializeObject<UsuarioDTO>(successContent);
                        var userId = usuario.UserId.ToString(); //
                        var userName = usuario.UserName;

                        /*
                        var usuario = new Usuario();
                        usuario.UserName = "Probando";
                        usuario.UserId = "123456";
                        var userId = usuario.UserId.ToString(); //
                        var userName = usuario.UserName;
                        */

                        // Crear la lista de reclamaciones
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName)
                };

                        // Crear el ClaimsIdentity con las reclamaciones
                        var identity = new ClaimsIdentity(claims, "Application");

                        // Crear el ClaimsPrincipal con el ClaimsIdentity
                        var principal = new ClaimsPrincipal(identity);

                        // Crear la cookie de autenticación manualmente
                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                        // Redirigir a la página de inicio o la ruta deseada
                        return RedirectToAction("Index", "Home");
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // Si la respuesta es un BadRequest (400), obtén el contenido
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, errorContent);
                        return View(modelo);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos");
                        return View(modelo);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de la llamada a la API externa
                ModelState.AddModelError(string.Empty, "El endpoint externo no se encuentra operativo");
                return View(modelo);
            }
        }













        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }
            return View();
        }


        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        /*
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            var usuario = new IdentityUser()
            {
                Email = modelo.Email,
                UserName = modelo.Email
            };
            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);
            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(modelo);
            }
        }
        */

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }


        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null, string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/");
            var mensaje = "";

            if (remoteError is not null)
            {
                mensaje = $"Error del proveedor externo: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                mensaje = $"Error cargando la data de login externo";
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            //Obtener email del Usuario.
            string email = "";
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = "Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            var requestData = new
            {
                Provider = info.LoginProvider,
                ProviderKey = info.ProviderKey,
                Email = email,
                ProviderDisplayName = info.ProviderDisplayName
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var endpointUrl = "https://localhost:7274/registrar-usuario";
            try
            {
                using (var httpClient = httpClientFactory.CreateClient())
                {
                    var response = await httpClient.PostAsync(endpointUrl, jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        //var successContent = await response.Content.ReadAsStringAsync();
                        // Console.WriteLine(successContent);
                        var successContent = await response.Content.ReadAsStringAsync();
                        var usuario = JsonConvert.DeserializeObject<UsuarioDTO>(successContent);
                        var userId = usuario.UserId.ToString(); // Asegúrate de que UserId sea de tipo string
                        var userName = usuario.UserName;

                        /*
                        var usuario = new Usuario();
                        usuario.UserName = "Probando";
                        usuario.UserId = "123456";
                        var userId = usuario.UserId.ToString(); // Asegúrate de que UserId sea de tipo string
                        var userName = usuario.UserName;
                        */

                        // Crear la lista de reclamaciones
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName)
                };

                        // Crear el ClaimsIdentity con las reclamaciones
                        var identity = new ClaimsIdentity(claims, "Application");

                        // Crear el ClaimsPrincipal con el ClaimsIdentity
                        var principal = new ClaimsPrincipal(identity);
                        // Crear la cookie de autenticación manualmente
                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
                        return LocalRedirect(urlRetorno);
                    }
                    else
                    {
                        mensaje = "Ha ocurrido un error en la API externa";
                        return LocalRedirect(urlRetorno);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de la llamada a la API externa
                mensaje = "La API externa no se encuentra disponible";
                return RedirectToAction("login", routeValues: new { mensaje });
            }
        }

        /*
        [AllowAnonymous]
        [HttpGet]

        public async Task<IActionResult> VerificarExisteUsuario(string email)
        {
            //var usuarioId = servicioUsuarios.ObtenerUsuarioId(); ;
            Console.WriteLine("Hola " + email.ToUpper());

            var emailUpper = email.ToUpper();
            var yaExisteTipoCuenta = await userManager.FindByEmailAsync(email);
            if (yaExisteTipoCuenta != null)
            {
                return Json($"El correo {yaExisteTipoCuenta.Email} ya existe");
            }


            return Json(true);
        }
        */



    }
}
