using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Medical_Record_Data;

using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Data.Repository.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddResponseCaching();



//FILTRO DE SEGURIDAD
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Access/Index";
        option.LogoutPath = "/Home/LogOut";
        option.AccessDeniedPath = "/Home/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(25);
    });
//PARA PODER GUARDAR OBJETOS EN UNA SESION
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(25);
    
    //option. = TimeSpan.FromMinutes(25);
});

//CONEXION A BASE DE DATOS
builder.Services.AddDbContext<DataBaseConnection>(
    options =>
        options.UseSqlServer
        (builder.Configuration.GetConnectionString("DefaultConnection"))
);


//IMPORTACIONES PARA LOS REPOSITORIOS

builder.Services.AddScoped<IUsersRepository,UsersRepository>();

builder.Services.AddScoped<IUsuarioPerfilRepository, UsuarioPerfilRepository>();


builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IFolder_Repository, Folder_Repository>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDiseaseRepository,DiseaseRepository>();
builder.Services.AddScoped<IMedical_Conditions, Medical_Repository>();
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


app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//Rutas para las areas y Proyecto principal
/*
 app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Index}/{id?}");

});
 */
app.UseEndpoints(endpoints =>
{
    //endpoints.MapControllerRoute(
    //    name: "Users",
    //    pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}");
    
    endpoints.MapControllerRoute(
        name: "Privileges",
        pattern: "{area:exists}/{controller=Profile}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Access}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "Folders",
        pattern: "{area:exists}/{controller=Folder}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
       name: "MedicalConditions",
       pattern: "{area:exists}/{controller=Disease}/{action=Index}/{id?}");
    
    endpoints.MapControllerRoute(
        name: "Modules",
        pattern: "{area:exists}/{controller=Patient_InformationController}/{action=Index}/{id?}");

    //endpoints.MapControllerRoute(
    //   name: "MedicalConditions",
    //   pattern: "{area:exists}/{controller=Surgery}/{action=Index}/{id?}");
});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Access}/{action=Index}/{id?}");

app.Run();
