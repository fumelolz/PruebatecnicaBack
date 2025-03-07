using PruebatecnicaBack.Api;
using PruebatecnicaBack.Application;
using PruebatecnicaBack.Infrastructure.Persistence.SignalR;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddSignalR();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200") // Reemplaza con tu dominio de Angular
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials(); // Necesario para SignalR
            });
    });
    builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

    builder.Services.AddDirectoryBrowser();
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();

    var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "images"));
    var requestPath = "/images";

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = fileProvider,
        RequestPath = requestPath
    });

    app.UseDirectoryBrowser(new DirectoryBrowserOptions
    {
        FileProvider = fileProvider,
        RequestPath = requestPath
    });

    app.UseCors("AllowAngular");

    app.UseAuthorization();
    app.MapControllers();
    app.MapHub<OrderHub>("/orderHub");
    app.MapHub<NotificationHub>("/hubs/notifications");

    app.Run();
}

