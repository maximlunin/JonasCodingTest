using AutoMapper;
using BusinessLayer;
using DataAccessLayer.Database;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer.Model.Models;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace WebApi.App_Start
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Http;
    using BusinessLayer.Model.Interfaces;
    using BusinessLayer.Services;
    //using Microsoft.Extensions.Logging;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Serilog;

    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.WebApi.DependencyResolver;
    using Serilog.Configuration;
    using System.Collections.Generic;
    using System.Configuration;
    using Ninject.Web.WebApi.Validation;
    using System.Web.Http.Validation;
    using System.Web.ModelBinding;
    using System.Linq;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                //kernel.Unbind<ModelValidatorProvider>();
                //ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.Single());
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                RegisterServices(kernel);

                
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IMapper>().ToMethod(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BusinessProfile>();
                    cfg.AddProfile<AppServicesProfile>();
                    cfg.ConstructServicesUsing(t => kernel.Get(t));
                });
                return config.CreateMapper();
            }).InSingletonScope();
            kernel.Bind<ICompanyService>().To<CompanyService>();
            kernel.Bind<ICompanyRepository>().To<CompanyRepository>();
            kernel.Bind<IEmployeeService>().To<EmployeeService>();
            kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
            kernel.Bind(typeof(IDbWrapper<Company>)).To(typeof(InMemoryDatabase<>)).InSingletonScope();
            kernel.Bind(typeof(IDbWrapper<Employee>)).To(typeof(InMemoryEmployeeDatabase<>)).InSingletonScope();
            //kernel.Bind<ModelValidatorProvider>().To<NinjectDataAnnotationsModelValidatorProvider>();
            kernel.Rebind<System.Web.Http.Validation.ModelValidatorProvider>().To(typeof(NinjectDefaultModelValidatorProvider));
            kernel.Bind<Serilog.ILogger> ().ToMethod(context =>
            {
                var filename = "Serilog" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                var log = new LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .WriteTo.File(filename);
                           
                return log.CreateLogger();
            }).InSingletonScope();

        }
    }
}