using Autofac;
using Autofac.Integration.WebApi;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Reflection;
using System.Web.Http;

namespace Project.Web.Reports
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            builder.Register(c => new SynchronizedConverter(new PdfTools())).As<IConverter>().SingleInstance();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
