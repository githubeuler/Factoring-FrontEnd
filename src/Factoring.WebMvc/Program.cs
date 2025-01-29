using System;
using System.Threading;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Factoring.Service.Common;
using Factoring.Service.Proxies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
var builder = WebApplication.CreateBuilder(args);

// Configuración de los servicios
builder.Services.AddControllersWithViews();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(builder.Configuration["DurationInMinutes"]));
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "_aspnetCoreSession";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ProxyHttpClient>();
builder.Services.AddTransient<IAuthProxy, AuthProxy>();
builder.Services.AddTransient<ICatalogoProxy, CatalogoProxy>();
builder.Services.AddTransient<IFacturaOperacionesProxy, FacturaOperacionesProxy>();
builder.Services.AddTransient<IAdquirienteProxy, AdquirienteProxy>();
builder.Services.AddTransient<IGiradorProxy, GiradorProxy>();
builder.Services.AddTransient<ICategoriaGiradorProxy, CategoriaGiradorProxy>();
builder.Services.AddTransient<IOperacionProxy, OperacionProxy>();
builder.Services.AddTransient<IFilesProxy, FilesProxy>();
builder.Services.AddTransient<IGiradorUbicacionProxy, GiradorUbicacionProxy>();
builder.Services.AddTransient<IAdquirienteUbicacionProxy, AdquirienteUbicacionProxy>();
builder.Services.AddTransient<IEvaluacionOperacionesProxy, EvaluacionOperacionesProxy>();
builder.Services.AddTransient<IContactoGiradorProxy, ContactoGiradorProxy>();
builder.Services.AddTransient<IDocumentosGiradorProxy, DocumentosGiradorProxy>();
builder.Services.AddTransient<IUbigeoProxy, UbigeoProxy>();
builder.Services.AddTransient<IFondeadorProxy, FondeadorProxy>();
builder.Services.AddTransient<ICavaliFactoringFondeadorProxy, CavaliFactoringFondeadorProxy>();
builder.Services.AddTransient<IDocumentoFondeadorProxy, DocumentoFondeadorProxy>();
builder.Services.AddTransient<IContactoAceptanteProxy, ContactoAceptanteProxy>();
builder.Services.AddTransient<IAceptanteProxy, AceptanteProxy>();
builder.Services.AddTransient<IDocumentosAceptanteProxy, DocumentosAceptanteProxy>();
builder.Services.AddTransient<IDataProxy, DataProxy>();
builder.Services.AddTransient<IFondeoProxy, FondeoProxy>();
builder.Services.AddTransient<IUsuarioProxy, UsuarioProxy>();
builder.Services.AddTransient<IPerfilMenuproxy, PerfilMenuproxy>();
builder.Services.AddTransient<IAdquirienteProxy, AdquirienteProxy>();
var app = builder.Build();

// Configuración de la aplicación
var cultureInfo = new CultureInfo("es-ES")
{
    NumberFormat =
    {
        CurrencyDecimalSeparator = ".",
        CurrencyGroupSeparator = ",",
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = ","
    }
};

Thread.CurrentThread.CurrentUICulture = cultureInfo;
Thread.CurrentThread.CurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
