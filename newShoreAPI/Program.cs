using Autofac;
using Autofac.Extensions.DependencyInjection;
using newShoreAPI.IOC;
using NLog;
using NLog.Web;

var _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
_logger.Debug("init main");


try
{
    var builder = WebApplication.CreateBuilder(args);

    //const to do ANGULAR
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new AutofacBusinessModule());
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("*"
                                                  );
                          });
    });


    // Add services to the container.

    //Corps
    builder.Services.AddCors();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Configure logs
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    //End configure logs






    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    _logger.Error(e, "The program stoped :( ");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}



