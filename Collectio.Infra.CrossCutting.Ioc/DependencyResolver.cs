using AutoMapper;
using Collectio.Application.Base;
using Collectio.Application.Base.Commands;
using Collectio.Domain.Base;
using Collectio.Infra.CrossCutting.Services;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using Collectio.Infra.Data;
using Collectio.Infra.Data.Repositories.Base;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Collectio.Application.Profiles;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Collectio.Infra.CrossCutting.Ioc
{
    public static class DependencyResolver
    {
        public static void RegisterDependencies(this IServiceCollection serviceCollectio, IConfiguration configuration)
        {
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-BR");
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            serviceCollectio.AddScoped<IDatabaseMigrator, DatabaeMigrator>();
            serviceCollectio.AddValidatorsFromAssembly(typeof(CommandValidator<,>).Assembly);
            serviceCollectio.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
            serviceCollectio.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidator<,>));
            serviceCollectio.AddMediatR(typeof(LoggingPipeline<,>).Assembly, typeof(IDomainEventHandler<>).Assembly);
            serviceCollectio.AddAutoMapper(e =>
            {
                e.ShouldMapProperty = p =>
                {
                    var setMethod = p.GetSetMethod(true);
                    return !(setMethod == null || setMethod.IsPrivate || setMethod.IsFamily);
                };
            }, typeof(MappingProfile).Assembly);
            serviceCollectio.AddScoped<ICommandQuerySender, CommandQuerySender>();
            serviceCollectio.AddDbContext<ApplicationContext>(e => e.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            serviceCollectio.AddScoped<IUnitOfWork, ApplicationContext>(e => e.GetService<ApplicationContext>());
            serviceCollectio.AddScoped<IDomainEventEmitter, DomainEventEmitter>();

            serviceCollectio.RegisterRepositories();
        }

        private static void RegisterRepositories(this IServiceCollection serviceCollection)
        {
            var interfaceRepositoryTypes = typeof(IRepository<>).Assembly.GetTypes().Where(e => e.IsInterface);

            var implementationRepositoryTypes = typeof(BaseRepository<>).Assembly.GetTypes()
                .Where(t => t.IsClass && t != typeof(BaseRepository<>) && interfaceRepositoryTypes.Any(ir => t.Implements(ir)));

            foreach (var implementationRepository in implementationRepositoryTypes)
            {
                var interfaceType =
                    interfaceRepositoryTypes.FirstOrDefault(ir => implementationRepository.Implements(ir));
                if (interfaceType.Implements(typeof(IRepository<>)))
                {
                    var iRepositoryInterface = interfaceType.GetInterfaces()
                        .FirstOrDefault(i => i.Name == typeof(IRepository<>).Name);
                    serviceCollection.AddScoped(iRepositoryInterface, implementationRepository);
                }

                serviceCollection.AddScoped(interfaceType, implementationRepository);
            }
        }

        private static bool Implements(this Type @this, Type @interface)
        {
            if (@this == null || @interface == null) return false;
            return @interface.GenericTypeArguments.Length > 0
                ? @interface.IsAssignableFrom(@this)
                : @this.GetInterfaces().Any(c => c.Name == @interface.Name);
        }
    }
}
