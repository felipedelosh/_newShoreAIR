using Autofac;
using Business.Availability;
using Models;
using Models.Contracts;

namespace newShoreAPI.IOC
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AvailabilityBusiness>().As<IAvailability>().SingleInstance();


            /*
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<List<FlightResponse>, List<Flight>>();
                //etc...
            })).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
            */
            
        }
    }
}
