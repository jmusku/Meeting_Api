using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using FluentValidation.WebApi;
using Meeting_App.ErrorHandeling;
using Meeting_App.Filters;
using Meeting_App.Interface;
using Meeting_App.Services;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;


namespace Meeting_App
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //oauth
            config.EnableCors();

            // Web API configuration and services

            var _container = new UnityContainer();
            _container.RegisterType<IUser, UserService>(new HierarchicalLifetimeManager());
            _container.RegisterType<IMeeting, MeetingService>(new HierarchicalLifetimeManager());
            _container.RegisterType<IAttendee, AttendeeService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityDependencyResolver(_container);

            //errror handeler
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());


            // Fluent Validation
            // config.Filters.Add(new ValidateModelStateFilter());
            config.Filters.Add(new ValidateModelAttribute());
            FluentValidationModelValidatorProvider.Configure(config);

            // Web API routes

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            // Attribute routing.
            config.MapHttpAttributeRoutes();
            // Convention-based routing.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{Id}",
                defaults: new { Id = RouteParameter.Optional, }
            );
        }
    }
}
