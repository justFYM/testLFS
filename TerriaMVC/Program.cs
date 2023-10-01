using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using TerriaMVC.Repository;
using TerriaMVC.Services;
using TerriaMVC.Store;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
// Add services to the container.
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
});
builder.Services.AddSingleton<IUserStore<IdentityUser>, InMemoryUserStore>();
builder.Services.AddSingleton<IUserRoleStore<IdentityUser>, InMemoryUserStore>();
builder.Services.AddSingleton<IUserPasswordStore<IdentityUser>, InMemoryUserStore>();
builder.Services.AddSingleton<IUserEmailStore<IdentityUser>, InMemoryUserStore>();
builder.Services.AddSingleton<IRoleStore<IdentityRole>, InMemoryRoleStore>();


builder.Services.AddTransient<IServiceBaseMethods, ServiceBaseMethods>();


builder.Services.AddTransient<IRepositoryBaseMethods, RepositoryBaseMethods>();
builder.Services.AddTransient<IInstrumentService, InstrumentService>();
builder.Services.AddTransient<IViewService, ViewService>();

builder.Services.AddTransient<ClinoextensometroService>();
builder.Services.AddTransient<ClinoextensometroRepository>();
builder.Services.AddTransient<GNSSService>();
builder.Services.AddTransient<PrismaService>();
builder.Services.AddTransient<PiezometroService>();
builder.Services.AddTransient<PiezometroRepository>();
builder.Services.AddTransient<SensorHumedadService>();
builder.Services.AddTransient<TriggerService>();
builder.Services.AddTransient<Radar01Service>();
builder.Services.AddTransient<Radar01Repository>();
builder.Services.AddTransient<TriggerRepository>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<InSARService>();
builder.Services.AddTransient<InSARRepository>();
builder.Services.AddTransient<IAOIService, AOIService>();
/*
builder.Services.AddTransient<GNSSService>();
builder.Services.AddTransient<PiezometroService>();
*/

//Servicios
/*
builder.Services.AddTransient<IClinoextensometroService, ClinoextensometroService>();
builder.Services.AddTransient<IGNSSService, GNSSService>();
builder.Services.AddTransient<IPrismaService, PrismaService>();
builder.Services.AddTransient<IPiezometroService, PiezometroService>();
builder.Services.AddTransient<ISensorHumedadService, SensorHumedadService>();
builder.Services.AddTransient<ITriggerService, TriggerService>();
*/

//Repositorios
/*
builder.Services.AddTransient<IClinoextensometroRepository, ServiceBaseMethods>();
builder.Services.AddTransient<IGNSSRepository, ServiceBaseMethods>();
builder.Services.AddTransient<IPrismaRepository, PrismaRepository>();
builder.Services.AddTransient<IPiezometroRepository, PiezometroRepository>();
builder.Services.AddTransient<ISensorHumedadRepository, SensorHumedadRepository>();
builder.Services.AddTransient<ITriggerRepository, TriggerRepository>();
*/
//builder.Services.AddTransient<IClinoextensometroService, IServiceBaseMethods>();

builder.Services.AddTransient<IInstrumentRepository, InstrumentRepository>();


builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.SignIn.RequireConfirmedAccount = false;
}).AddDefaultTokenProviders();

// Agregar configuraci�n de autenticaci�n con esquema predeterminado
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddMicrosoftAccount(opciones =>
{
    opciones.ClientId = builder.Configuration["MicrosoftClientId"];
    opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
});
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    opciones =>
    {
        opciones.LoginPath = "/session/login";
        opciones.AccessDeniedPath = "/session/login";
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

// Agregar middleware de autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
