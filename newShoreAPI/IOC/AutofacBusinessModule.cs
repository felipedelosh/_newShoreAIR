using Autofac;
using AutoMapper;
using Business.Availability;
using Business.Mapper;
using Models;
using Models.Contracts;
using Models.Third;
using Helper.RoutesCalculator;

namespace newShoreAPI.IOC
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AvailabilityBusiness>().As<IAvailability>().SingleInstance();

            builder.RegisterType<RouteCalculator>().As<IRouteCalculator>().SingleInstance();

            builder.RegisterType<APIResponseFlights<List<Flight>, List<GetJsonFlightResponse>>>().As<IMap<List<Flight>, List<GetJsonFlightResponse>>>().SingleInstance();

            builder.RegisterType<Flight_Transport<Transport, GetJsonFlightResponse>>().As<IMap<Transport, GetJsonFlightResponse>>().SingleInstance();

           

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<List<GetJsonFlightResponse>, List<Flight>>();
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

        }
    }
}
