using Proyecto_Evo_RRLL.Configuracion;
using Proyecto_Evo_RRLL.Filtros;
using Proyecto_Evo_RRLL.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(opciones => opciones.Filters.Add<RegistrarExcepcionFilter>());
builder.Services.AgregarBaseDeDatos(builder.Configuration);
builder.Services.AgregarAutenticacionPorCookies();
builder.Services.AgregarCapasDeNegocio();
builder.Services.AgregarServiciosDeAplicacion();

var app = builder.Build();

// Usuario administrador inicial sobre una base de datos ya existente.
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<SembradorAdministrador>().SembrarAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Home/PaginaNoEncontrada", "?statusCode={0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
